using EventStore.ClientAPI;
using System.Net;

namespace Quiz.EventSourcing
{
    public static class EventStoreConnectionFactory
    {
        public static IEventStoreConnection Create()
        {
            var connection = EventStoreConnection.Create(IPEndPointFactory.DefaultTcp());
            connection.ConnectAsync().Wait();
            return connection;
        }
    }

    internal class IPEndPointFactory
    {
        public static IPEndPoint DefaultTcp()
        {
            return CreateIPEndPoint(1113);
        }

        private static IPEndPoint CreateIPEndPoint(int port)
        {
            var address = IPAddress.Parse("127.0.0.1");
            return new IPEndPoint(address, port);
        }
    }
}