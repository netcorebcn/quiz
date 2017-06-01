using System.Threading.Tasks;
using EasyEventSourcing;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Domain;

namespace Quiz.Results
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

            services.AddEasyEventSourcing<QuizAggregate>(
                EventStoreOptions.Create(
                    Configuration["EVENT_STORE"], 
                    Configuration["EVENT_STORE_MANAGER_HOST"]));

            services.AddWebSocketManager();
        }

        public void Configure(IApplicationBuilder app, 
            ILoggerFactory loggerFactory,
            IEventStoreBus eventBus,
            IEventStoreProjections projections,
            WebSocketHandler handler)
        {
            app.UseCors("CorsPolicy");
            app.UseWebSockets();
            app.MapWebSocketManager("/ws", handler);     

            var logger = loggerFactory.CreateLogger<Startup>();     
            
            projections.CreateAsync(nameof(Projections.QuestionAnswers), Projections.QuestionAnswers)
                .Wait();
            
            eventBus.Subscribe(
                nameof(Projections.QuestionAnswers),
                async (message) => { 
                    logger.LogInformation(message.ToString());
                    await handler.SendMessageToAllAsync(message);
                })
                .Wait();
        }
    }
}
