using System;
using Microsoft.Extensions.Configuration;

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
            projections.CreateAsync(Projections.QuestionAnswers).Wait();
            
            Console.WriteLine("Event Store Quiz Setup Done!");
        }
    }
}
