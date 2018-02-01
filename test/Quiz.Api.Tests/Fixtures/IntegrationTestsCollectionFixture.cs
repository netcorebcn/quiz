using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Quiz.Api.Tests
{
    [CollectionDefinition("IntegrationTests")]
    public class IntegrationTestsCollectionFixture : ICollectionFixture<DocumentStoreFixture>
    {
    }
}