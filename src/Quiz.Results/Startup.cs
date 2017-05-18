using System;
using System.Threading.Tasks;
using EasyEventSourcing;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Quiz.Domain;
using static Quiz.Voting.Results.RetryExtensions;

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
                ReflectionHelper.DomainAssembly);

            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory,
            IEventStoreBus eventBus,
            IEventStoreProjections projections,
            WebSocketHandler handler)
        {
            app.UseCors("CorsPolicy");
            app.UseWebSockets();
            app.MapWebSocketManager("/ws", handler);     

            var logger = loggerFactory.CreateLogger<Startup>();     
            
            DefaultRetryAsync(
                async () => await projections.CreateAsync(Projections.QuestionAnswers))
                .Wait();
            
            DefaultRetryAsync(SubscribeToEventStore)
                .Wait();

            async Task SubscribeToEventStore () => await eventBus.Subscribe(
                async (message) => { 
                    logger.LogInformation(message.ToString());
                    await handler.SendMessageToAllAsync(message);
                });
        }
    }
}
