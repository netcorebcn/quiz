using System;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using EventStore.ClientAPI.Common.Log;

namespace Quiz.Voting.EventStore
{
    public class EventStoreSetup
    {
        public static void Create(IEventStoreConnection conn)
        {
            const string STREAM = "QuestionAnswers";
            const string GROUP = "Default";
            var credentials = new UserCredentials("admin", "changeit");

            var projections = new ProjectionsManager(new FakeLogger(), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2113), TimeSpan.FromSeconds(30));
            projections.EnableAsync("$by_category", credentials ).Wait();
            projections.CreateContinuousAsync(
                STREAM, 
                Projections.QuestionAnswers,
                credentials
                ).Wait();

            CreateSubscription(conn, credentials, STREAM, GROUP);
        }

        private static void CreateSubscription(IEventStoreConnection conn, 
            UserCredentials credentials, string stream, string group)
        {
            var settings = PersistentSubscriptionSettings.Create()
                .DoNotResolveLinkTos()
                .StartFromCurrent();

            try
            {
                conn.CreatePersistentSubscriptionAsync(stream, group, settings, credentials).Wait();
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
