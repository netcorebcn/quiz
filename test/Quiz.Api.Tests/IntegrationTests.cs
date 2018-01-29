using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Newtonsoft.Json;
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
        public async Task QuizAppService_Test_Start()
        {
            using(var session = _documentStore.OpenSession())
            {
                var appService = new QuizAppService(_documentStore);
                var result = await appService.Start(CreateQuiz());
                Assert.NotNull(result);
            }
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
    }
}
