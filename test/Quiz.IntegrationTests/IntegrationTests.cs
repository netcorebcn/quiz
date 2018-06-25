using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Marten;
using Newtonsoft.Json;
using Quiz.Domain;
using Quiz.Domain.Commands;
using Xunit;

namespace Quiz.Api.Tests
{
    [Collection("IntegrationTests")]
    public class IntegrationTests
    {
        private readonly IBus _bus;
        private readonly IDocumentStore _documentStore;

        public IntegrationTests(DocumentStoreFixture documentStoreFixture, BusFixture busFixture)
        {
            _bus = busFixture.Bus;
            _documentStore = documentStoreFixture.DocumentStore;
        } 

        [Fact]
        public async Task QuizAppService_Test_Start()
        {
            var appService = new QuizAppService(_documentStore, _bus);
            var result = await appService.Start(CreateQuiz());
            Assert.NotNull(result);
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
    }
}
