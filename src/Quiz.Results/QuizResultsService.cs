using System.Threading.Tasks;
using EasyEventSourcing;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Quiz.Domain;

namespace Quiz.Results
{
    public static class QuizResultsServiceExtensions
    {
        public static IApplicationBuilder UseQuizResultsService(this IApplicationBuilder app)
        {
            var votingResult = (QuizResultsService)app.ApplicationServices.GetService(typeof(QuizResultsService));
            votingResult.Start();
            return app;
        }
    }
    public class QuizResultsService
    {
        private readonly IEventStoreBus _eventStoreBus;
        private readonly ILogger<QuizResultsService> _logger;
        private readonly IEventStoreProjections _projections;
        private readonly IWebSocketPublisher _wsPublisher;

        public QuizResultsService(IEventStoreBus eventStoreBus, 
            IEventStoreProjections projections, 
            IWebSocketPublisher wsPublisher,
            ILogger<QuizResultsService> logger)
        {
            _eventStoreBus = eventStoreBus;
            _logger = logger;
            _projections = projections;
            _wsPublisher = wsPublisher;
        }

        public void Start()
        {
            _projections.CreateAsync(nameof(Projections.QuestionAnswers), Projections.QuestionAnswers).Wait();
            
            _eventStoreBus.Subscribe(
                nameof(Projections.QuestionAnswers),
                async (message) => { 
                    _logger.LogInformation(message.ToString());
                    await _wsPublisher.SendMessageToAllAsync(message);
                })
                .Wait();

        }
    }}