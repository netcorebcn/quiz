using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Api.EventStore;

namespace Quiz.Api.Tests
{
    public class DocumentStoreFixture
    {
        public DocumentStoreFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            
            var services = new ServiceCollection();
            services.AddEventStore(configuration);
            DocumentStore = services.BuildServiceProvider().GetService<IDocumentStore>();
        }

        public IDocumentStore DocumentStore { get; }      
    }
}