using EventStore.ClientAPI;

namespace Quiz.EventSourcing
{
    public static class EventStoreConnectionFactory
    {
        public static IEventStoreConnection Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = "tcp://user:password@localhost:1113";

            var connection = EventStoreConnection.Create(new System.Uri(connectionString));
            connection.ConnectAsync().Wait();
            return connection;
        }
    }
}