using EventStore.ClientAPI.SystemData;
using Microsoft.Extensions.Configuration;

namespace Quiz.EventSourcing
{
    public class EventStoreOptions
    {    
        public string ConnectionString { get; }
        public string ManagerHost { get; }
        public (string stream, string group) Subscription { get; }
        public UserCredentials Credentials { get; } = new UserCredentials("admin", "changeit");

        private EventStoreOptions(string connectionString, string managerHost, (string, string) subscription)
        {
            ConnectionString = connectionString;
            ManagerHost = managerHost;
            Subscription = subscription;
        }

        public static EventStoreOptions Create(IConfigurationRoot configuration) =>
            new EventStoreOptions(
                configuration["EVENT_STORE"] ?? "tcp://admin:changeit@localhost:1113",
                configuration["EVENT_STORE_MANAGER_HOST"] ?? "localhost:2113",
                (configuration["STREAM_NAME"] ?? "QuestionAnswers",  configuration["GROUP_NAME"] ?? "Default"));
    }
}