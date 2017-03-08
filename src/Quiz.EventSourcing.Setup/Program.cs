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
            var connectionString = configuration["EVENT_STORE"] ?? "tcp://admin:changeit@localhost:1113";

            Console.WriteLine("Starting Event Store Quiz Setup.");

            var conn = EventStoreConnectionFactory.Create(connectionString);
            EventStoreSetup.CreateWithRetry(conn, configuration);

            Console.WriteLine("Event Store Quiz Setup Done!");

        }
    }
}
