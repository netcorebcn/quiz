using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Quiz.Domain;
using Xunit;

namespace Quiz.Api.Tests
{
    [Collection("IntegrationTests")]
    public class IntegrationTests
    {
        private readonly IDocumentStore _documentStore;

        public IntegrationTests(DocumentStoreFixture documentStoreFixture) => 
            _documentStore = documentStoreFixture.DocumentStore;

        [Fact]
        public async Task Test()
        {
        }
    }
}
