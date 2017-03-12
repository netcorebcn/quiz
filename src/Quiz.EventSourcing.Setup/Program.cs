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
            var conn = EventStoreConnectionFactory.Create(options.ConnectionString);
            EventStoreSetup.Create(conn, options).Wait();

            Console.WriteLine("Event Store Quiz Setup Done!");

        }
    }
}
