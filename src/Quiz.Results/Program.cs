using System;
using System.Text;
using System.Threading;
using EventStore.ClientAPI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using Quiz.EventSourcing;
using Quiz.Messages;

namespace Quiz.Results
{
    public class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) => {
                _quitEvent.Set();
                eArgs.Cancel = true;
                Console.WriteLine("Application is shutting down...");
            };

            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var configuration = builder.Build();
            var connectionString = configuration["EVENT_STORE"] ?? "tcp://admin:changeit@localhost:1113";
            var stream = configuration["STREAM_NAME"] ?? "QuestionAnswers";
            var group = configuration["GROUP_NAME"] ?? "Default";

            var retry = Policy
                .Handle<Exception>()
                .WaitAndRetry(3, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) 
                );

            var settings = ConnectionSettings.Create();//.EnableVerboseLogging().UseConsoleLogger();
            using (var conn = EventStoreConnection.Create(settings, new System.Uri(connectionString)))
            {
                conn.ConnectAsync().Wait();

                Console.Write("Application started. Press Ctrl+C to shut down.");
                retry.Execute(() =>
                {
                    conn.ConnectToPersistentSubscription(stream, group, (_, x) =>
                    {
                        var myEvent = DeserializeEvent(x.Event);
                        var data = Encoding.ASCII.GetString(x.Event.Data);
                        Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
                        Console.WriteLine(data);
                    });
                });
                
                _quitEvent.WaitOne();
            }      
        }

        private static object DeserializeEvent(RecordedEvent evt)
        {
            var eventTypeResolver = new EventTypeResolver(ReflectionHelper.MessagesAssembly);
            var targetType = eventTypeResolver.GetTypeForEventName(evt.EventType);
            var json = Encoding.UTF8.GetString(evt.Data);
            return JsonConvert.DeserializeObject(json, targetType);
        }
    }
}
