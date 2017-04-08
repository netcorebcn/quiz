using System;
using System.Threading;
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

            Policy.Handle<Exception>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(async () => await projections.CreateAsync(Projections.QuestionAnswers))
            .Wait();

            Console.WriteLine("Event Store Quiz Setup Done!");
        }
    }
}
