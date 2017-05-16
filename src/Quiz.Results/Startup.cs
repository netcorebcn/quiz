using System;
using EasyEventSourcing;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Quiz.Messages;

namespace Quiz.Voting.Results
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
            loggerFactory.AddConsole();
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

            services.AddEasyEventSourcing(
                EventStoreOptions.Create(
                    Configuration["EVENT_STORE"], 
                    Configuration["EVENT_STORE_MANAGER_HOST"], 
                    Configuration["STREAM_NAME"]), 
                ReflectionHelper.MessagesAssembly);

            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            IEventStoreBus eventBus,
            WebSocketHandler handler)
        {
            app.UseCors("CorsPolicy");
            app.UseWebSockets();
            app.MapWebSocketManager("/ws", handler);     

            var logger = loggerFactory.CreateLogger<Startup>();     

            Policy.Handle<Exception>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(async () =>  
                await eventBus.Subscribe( 
                async (message) => {
                    logger.LogInformation(message.ToString());
                    await handler.SendMessageToAllAsync(message);    
                }))
            .Wait();
        }
    }
}
