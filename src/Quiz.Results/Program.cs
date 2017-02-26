using System;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Microsoft.Extensions.Configuration;

namespace Quiz.Results
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var configuration = builder.Build();
            var connectionString = configuration["EVENT_STORE"] ?? "tcp://admin:changeit@localhost:1113";
            var stream = configuration["STREAM_NAME"] ?? "QuestionAnswers";
            const string group = "default_group";

            //uncommet to enable verbose logging in client.
            var settings = ConnectionSettings.Create();//.EnableVerboseLogging().UseConsoleLogger();
            using (var conn = EventStoreConnection.Create(settings, new System.Uri(connectionString)))
            {
                conn.ConnectAsync().Wait();

                conn.ConnectToPersistentSubscription(stream, group, (_, x) =>
                {
                    var data = Encoding.ASCII.GetString(x.Event.Data);
                    Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
                    Console.WriteLine(data);
                });

                Console.WriteLine("waiting for events. press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
