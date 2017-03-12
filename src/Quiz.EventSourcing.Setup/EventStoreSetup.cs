using System;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;

namespace Quiz.EventSourcing.Setup
{
    public class EventStoreSetup
    {
        public static async Task Create(IEventStoreConnection conn, EventStoreOptions options)
        {
            await CreateProjections(options).DefaultRetry();
            await CreateSubscription(conn, options).DefaultRetry();
        }

        private static async Task CreateProjections(EventStoreOptions options)
        {
            var address = GetIPEndPointFromHostName(options.ManagerHost).Result;
            var projections = new ProjectionsManager(new FakeLogger(), address, TimeSpan.FromSeconds(30));
            await projections.EnableAsync("$by_category", options.Credentials );
            await projections.CreateContinuousAsync(options.Subscription.stream, Projections.QuestionAnswers, options.Credentials);
        }

        private static async Task CreateSubscription(IEventStoreConnection conn, EventStoreOptions options)
        {
            try
            {
                var settings = PersistentSubscriptionSettings.Create()
                    .ResolveLinkTos()
                    .StartFromCurrent();

                await conn.CreatePersistentSubscriptionAsync(options.Subscription.stream, options.Subscription.group, settings, options.Credentials);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() != typeof(InvalidOperationException)
                    && ex.InnerException?.Message != $"Subscription group {options.Subscription.group} on stream {options.Subscription.stream} already exists")
                {
                    throw;
                }
            }
        }

       private static async Task<IPEndPoint> GetIPEndPointFromHostName(string hostName)
       {
            var hostParts = hostName.Split(':');

            if (hostParts[0] == "localhost")
                return new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(hostParts[1]));
            
            var addresses = await System.Net.Dns.GetHostAddressesAsync(hostParts[0]);
            if (addresses.Length == 0)
            {
                throw new ArgumentException(
                    "Unable to retrieve address from specified host name.", 
                    "hostName"
                );
            }
            return new IPEndPoint(addresses[0], int.Parse(hostParts[1])); // Port gets validated here.
        } 

        private class FakeLogger : ILogger
        {
            public void Debug(string format, params object[] args)
            {
            }

            public void Debug(Exception ex, string format, params object[] args)
            {
            }

            public void Error(string format, params object[] args)
            {
            }

            public void Error(Exception ex, string format, params object[] args)
            {
            }

            public void Info(string format, params object[] args)
            {
            }

            public void Info(Exception ex, string format, params object[] args)
            {
            }
        }
    }
}
