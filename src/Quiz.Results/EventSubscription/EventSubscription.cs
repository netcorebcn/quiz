using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Quiz.EventSourcing;

namespace Quiz.Voting.Results
{
    public static class EventSubscription
    {
        public static async Task StartSubscription(
            this IEventStoreConnection eventBus, 
            EventStoreOptions options, 
            EventTypeResolver typeResolver,
            Func<object,Task> messageSender) =>
            await eventBus.Subscribe(typeResolver, options.Subscription, 
                async msg => await messageSender(msg));
    }
}