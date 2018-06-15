using EasyNetQ;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Api.EventStore;

namespace Quiz.Api.Tests
{
    public class BusFixture
    {
        public BusFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            
            var services = new ServiceCollection();
            services.AddSingleton<IBus>(RabbitHutch.CreateBus(configuration["messagebroker"].Trim() ?? "amqp://guest:guest@localhost:5672"));
            Bus = services.BuildServiceProvider().GetService<IBus>();
        }

        public IBus Bus{ get; }      
    }
}