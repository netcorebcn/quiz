using System;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Quiz.EventSourcing;
using Quiz.Messages;

namespace Quiz.Results
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var quitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eArgs) => {
                quitEvent.Set();
                eArgs.Cancel = true;
                Console.WriteLine("Application is shutting down...");
            };

            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            var configuration = builder.Build();
                        
            var options = EventStoreOptions.Create(configuration);
            var typeResolver = new EventTypeResolver(ReflectionHelper.MessagesAssembly);
            var eventBus = EventStoreConnectionFactory.Create(options.ConnectionString);
        
            eventBus.Subscribe(
                typeResolver, options.Subscription, 
                msg => 
                {
                    if (msg is QuestionRightAnsweredEvent rightEvent)
                        Console.Write($"{rightEvent.GetType().Name}-{rightEvent.OptionId}-{rightEvent.QuestionId}");
                    
                    if (msg is QuestionWrongAnsweredEvent wrongEvent)
                        Console.Write($"{wrongEvent.GetType().Name}-{wrongEvent.OptionId}-{wrongEvent.QuestionId}");
                })
            .Wait();

            Console.Write("Application started. Press Ctrl+C to shut down.");
            quitEvent.WaitOne();
        }      
     }
}
