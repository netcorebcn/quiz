using System;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using Microsoft.Extensions.Configuration;
using Polly;

namespace Quiz.EventSourcing.Setup
{
    public class EventStoreSetup
    {
        public static void CreateWithRetry(IEventStoreConnection conn, IConfigurationRoot configuration)
        {
            var retry = Policy
                .Handle<Exception>()
                .WaitAndRetry(5, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) 
                );

            retry.Execute(() => Create(conn, configuration));
        }

        public static void Create(IEventStoreConnection conn, IConfigurationRoot configuration)
        {
            var hostName = configuration["EVENT_STORE_MANAGER_HOST"] ?? "localhost:2113";
            var stream = configuration["STREAM_NAME"] ?? "QuestionAnswers";
            var group = configuration["GROUP_NAME"] ?? "Default";
            var credentials = new UserCredentials("admin", "changeit");

            try
            {
                var address = GetIPEndPointFromHostName(hostName).Result;
                var projections = new ProjectionsManager(new FakeLogger(), address, TimeSpan.FromSeconds(30));
                projections.EnableAsync("$by_category", credentials ).Wait();

                projections.CreateContinuousAsync(
                    stream, 
                    Projections.QuestionAnswers,
                    credentials).Wait();
                CreateSubscription(conn, credentials, stream, group);

            }
            catch (AggregateException ex)
            {
                if (ex.InnerException.GetType() != typeof(InvalidOperationException)
                    && ex.InnerException?.Message != $"Subscription group {group} on stream {stream} already exists")
                {
                    throw;
                }
            }
        }

        private static void CreateSubscription(IEventStoreConnection conn, 
            UserCredentials credentials, string stream, string group)
        {
            var settings = PersistentSubscriptionSettings.Create()
                .ResolveLinkTos()
                .StartFromCurrent();

            conn.CreatePersistentSubscriptionAsync(stream, group, settings, credentials).Wait();
        }

       public static async Task<IPEndPoint> GetIPEndPointFromHostName(string hostName)
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

        public class FakeLogger : ILogger
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
