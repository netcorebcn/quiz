using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EasyNetQ;
using EasyWebSockets;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
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
        private readonly IDocumentStore _eventStore;
        private QuizResultsAggregate aggregate = QuizResultsAggregate.Empty; 

        public QuizResultsAppService(IDocumentStore eventStore, IBus bus, IWebSocketPublisher wsBus)
        {
            _bus = bus;
            _wsBus = wsBus;
            _eventStore = eventStore;
        }

        public QuizResultsAggregate Get() => aggregate;

        public void Start()
        {
            aggregate = StartAggregate();
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

        private QuizResultsAggregate StartAggregate()
        {
            using (var session = _eventStore.OpenSession())
            {
                var currentQuiz = session.Query<CurrentQuizAggregate>().FirstOrDefault();
                if (currentQuiz == null)
                {
                    return QuizResultsAggregate.Empty;
                }

                var events = session.Events.FetchStream(currentQuiz.Id);
                return Create(currentQuiz.Id, events.Select(x => x.Data).ToArray());
            }
        }
    }

    public static class QuizResultsAppServiceExtensions
    {
        public static IServiceCollection AddQuizResultsApp(this IServiceCollection services, IConfiguration configuration) => services
            .AddEasyWebSockets()
            .AddSingleton<IBus>(RabbitHutch.CreateBus(configuration["messagebroker"]?.Trim() ?? "amqp://guest:guest@localhost:5672"))
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