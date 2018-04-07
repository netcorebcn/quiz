using System;
using Marten;
using Quiz.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Quiz.Results.Api.EventStore
{
    public static class EventStoreExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSchema = "quiz";
            Retry(() => services.AddSingleton<IDocumentStore>(
                    DocumentStore.For(_ =>
                    {
                        _.Connection(configuration["dbconnection"]?.Trim() ?? "Host=localhost;Username=admin;Password=changeit;");
                        _.Events.DatabaseSchemaName = databaseSchema;
                        _.DatabaseSchemaName = databaseSchema;
                    })));

            return services;
        }
        internal static void Retry(Action action, int retries = 5) =>
            Policy.Handle<Exception>()
                .WaitAndRetry(retries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .Execute(action);
    }
}