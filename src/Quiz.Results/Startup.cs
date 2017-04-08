using System;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Quiz.EventSourcing;

namespace Quiz.Voting.Results
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public EventStoreOptions EventStoreOptions { get; }

        public Startup(ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
            loggerFactory.AddConsole();
            EventStoreOptions = EventStoreOptions.Create(Configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() );
            });
            services.AddEventStoreSubscription(EventStoreOptions);
            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            IEventStoreConnection eventBus,
            EventTypeResolver typeResolver,
            WebSocketHandler handler)
        {
            app.UseCors("CorsPolicy");
            app.UseWebSockets();
            app.MapWebSocketManager("/ws", handler);     

            var logger = loggerFactory.CreateLogger<Startup>();     

            Policy.Handle<Exception>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(async () =>  
                await eventBus.StartSubscription(EventStoreOptions, typeResolver, 
                async (message) => {
                    logger.LogInformation(message.ToString());
                    await handler.SendMessageToAllAsync(message);    
                }))
            .Wait();
        }
    }
}
