using System;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Quiz.EventSourcing
{
    public static class EventStoreConnectionFactory
    {
        public static IEventStoreConnection Create(string connectionString)
        {
            connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            
            var settings = ConnectionSettings.Create().KeepReconnecting().KeepRetrying();
            var connection = EventStoreConnection.Create(settings, new System.Uri(connectionString));
            connection.ConnectAsync().Wait();
            return connection;
        }

        public static async Task Subscribe(
            this IEventStoreConnection @this, 
            EventTypeResolver typeResolver,
            (string streamName, string groupName) subscription, 
            Action<object> action) =>
            await @this.ConnectToPersistentSubscriptionAsync(
                subscription.streamName, 
                subscription.groupName, 
                (_, x) => action(x.Event.Deserialize(typeResolver)));

        public static object Deserialize(this RecordedEvent evt, EventTypeResolver eventTypeResolver)
        {
            var targetType = eventTypeResolver.GetTypeForEventName(evt.EventType);
            var json = Encoding.UTF8.GetString(evt.Data);
            return JsonConvert.DeserializeObject(json, targetType);
        }
    }
}