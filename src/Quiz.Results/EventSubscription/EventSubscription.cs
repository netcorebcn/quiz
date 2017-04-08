using System;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Quiz.EventSourcing;

namespace Quiz.Voting.Results
{
    public static class EventSubscription
    {
        public static async Task StartSubscription(this IEventStoreConnection eventBus, EventStoreOptions options, EventTypeResolver typeResolver, Func<object,Task> messageSender)
        {
            await CreateSubscription(eventBus, options);
            await eventBus.Subscribe(typeResolver, options.Subscription, async msg => await messageSender(msg));
        }

        private static async Task CreateSubscription(IEventStoreConnection conn, EventStoreOptions options)
        {
            var settings = PersistentSubscriptionSettings.Create()
                .ResolveLinkTos()
                .StartFromCurrent();

            await conn.CreatePersistentSubscriptionAsync(options.Subscription.stream, options.Subscription.group, settings, options.Credentials);
        }
    }
}