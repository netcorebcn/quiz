using System;
using Microsoft.Extensions.Configuration;
using Polly;

namespace Quiz.EventSourcing.Setup
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var configuration = builder.Build();

            Console.WriteLine("Starting Event Store Quiz Setup.");

            var options = EventStoreOptions.Create(configuration);
            var projections = new EventStoreProjectionsClient(options);

            Policy
            .Handle<Exception>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                (exception, timeSpan, context) => 
                    projections.CreateAsync(Projections.QuestionAnswers).Wait()
            );
            
            Console.WriteLine("Event Store Quiz Setup Done!");
        }
    }
}
