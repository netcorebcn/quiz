using System;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;

namespace Quiz.EventSourcing
{
    public class EventStoreProjections
    {
        public static async Task<string> GetStateAsync(EventStoreOptions options, string projectionName)
        {
            var projectionsClient = await CreateProjectionsClient(options);
            return await projectionsClient.GetStateAsync(projectionName, options.Credentials);
        }        

        public static async Task CreateProjection(EventStoreOptions options, string projectionName)
        {
            var projectionsClient = await CreateProjectionsClient(options);
            await Task.WhenAll(
                projectionsClient.EnableAsync("$by_category", options.Credentials),
                projectionsClient.CreateContinuousAsync(options.Subscription.stream, projectionName, options.Credentials)
            );
        }

        private static async Task<ProjectionsManager> CreateProjectionsClient(EventStoreOptions options)
        {
            var address = await GetIPEndPointFromHostName(options.ManagerHost);
            return new ProjectionsManager(new FakeLogger(), address, TimeSpan.FromSeconds(90));
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
