using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Quiz.Domain;
using Quiz.Domain.Events;
using static Quiz.Domain.QuizResultsAggregate;

namespace Quiz.Results.Api
{
    public class QuizResultsAppService
    {
        private readonly IBus _bus;

        private readonly IWebSocketPublisher _wsBus;

        private QuizResultsAggregate aggregate; 

        public QuizResultsAppService(IBus bus, IWebSocketPublisher wsBus)
        {
            _bus = bus;
            _wsBus = wsBus;
        }

        public object Get() => aggregate ?? QuizResultsAggregate.Empty;

        public void Start()
        {
            _bus.SubscribeAsync<QuizEvent>("QuizEvents", async @event => {
                switch (@event)
                {
                    case QuizStartedEvent startedEvent:
                        aggregate = Create(startedEvent.QuizId, startedEvent);
                        break; 

                    case QuizAnsweredEvent answerEvent:
                        aggregate = Reduce(aggregate, answerEvent);
                        break;
                }

                await Publish();
            });

            async Task Publish()
            {
                try
                {
                    await _wsBus.SendMessageToAllAsync(aggregate);
                }
                catch(Exception)
                {
                    // Log
                }
            }
        }
    }

    public static class QuizResultsAppServiceExtensions
    {
        public static IServiceCollection AddQuizResultsApp(this IServiceCollection services, IConfiguration configuration) => services
            .AddEasyWebSockets()
            // .AddSingleton<IBus>(RabbitHutch.CreateBus(configuration["messagebroker"] ?? "amqp://guest:guest@localhost:5672"))
            .AddSingleton<IBus>(RabbitHutch.CreateBus("amqp://guest:guest@messagebroker:5672"))
            .AddSingleton<QuizResultsAppService>();
        
        public static IApplicationBuilder UseQuizResultsApp(this IApplicationBuilder app)
        {
            app.UseEasyWebSockets();
            var quizResultsEventHandler = (QuizResultsAppService)app.ApplicationServices.GetService(typeof(QuizResultsAppService));
            Retry(quizResultsEventHandler.Start);
            return app;
        }
        private static void Retry(Action action, int retries = 5) =>
            Policy.Handle<Exception>()
                .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .Execute(action);
    }
}