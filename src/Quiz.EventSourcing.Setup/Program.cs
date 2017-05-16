using System;
using System.Threading;
using EasyEventSourcing;
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

            var projections = new EventStoreProjectionsClient(
                EventStoreOptions.Create(configuration["EVENT_STORE"], configuration["EVENT_STORE_MANAGER_HOST"], configuration["STREAM_NAME"]));

            Policy.Handle<Exception>()
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(async () => await projections.CreateAsync(Projections.QuestionAnswers))
            .Wait();

            Console.WriteLine("Event Store Quiz Setup Done!");
        }
    }
}
