using EventStore.ClientAPI;
using Microsoft.Extensions.DependencyInjection;
using Quiz.EventSourcing;
using Quiz.Messages;

namespace Quiz.Voting.Results
{
    public static class EventStoreExtensions
    {
        public static IServiceCollection AddEventStoreSubscription(this IServiceCollection services, EventStoreOptions options)
        {
            services.AddSingleton<IEventStoreConnection>(EventStoreConnectionFactory.Create(options.ConnectionString));
            services.AddSingleton<EventTypeResolver>(new EventTypeResolver(ReflectionHelper.MessagesAssembly));
            
            return services;
        }
    }
}