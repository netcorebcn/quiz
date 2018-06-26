using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Marten;
using Newtonsoft.Json;
using Quiz.Domain;
using Quiz.Domain.Commands;
using Quiz.Domain.Events;
using Quiz.Results.Api;
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
            Assert.Equal(QuizState.Started.ToString(), result.QuizState);
            Assert.Equal(2, result.Questions.Count);

            var resultState = await appService.Close(result.QuizId);
            Assert.NotNull(resultState);
            Assert.Equal(QuizState.Closed.ToString(), resultState.QuizState);
        }

        private async Task CleanUp()
        {
            var commandService = new QuizAppService(_documentStore, _bus);
            var state = await commandService.Start(CreateQuiz());

            var queryService = new QuizResultsAppService(_documentStore, _bus, null);
            queryService.Start();
            var result = queryService.Get();
            await commandService.Close(result.QuizId);
        }

        [Fact]
        public async Task QuizAppService_Test_CorrectAnswers()
        {
            await CleanUp();
            var appService = new QuizAppService(_documentStore, _bus);
            var state = await appService.Start(CreateQuiz());
            await appService.Answer(new QuizAnswersCommand (state.QuizId, 
                            new List<QuizAnswer> {
                                new QuizAnswer { 
                                    QuestionId = state.Questions.First().Id, 
                                    OptionId = state.Questions.First().Options.First(x => x.IsCorrect).Id 
                                },
                                new QuizAnswer { 
                                    QuestionId = state.Questions.Last().Id, 
                                    OptionId = state.Questions.Last().Options.First(x => x.IsCorrect).Id 
                                },
                            }));

            var queryService = new QuizResultsAppService(_documentStore, _bus, null);
            queryService.Start();
            var result = queryService.Get();

            Assert.NotNull(result);
            Assert.Equal(100.0M, result.TotalCorrectAnswersPercent);
            Assert.Equal(0.0M, result.TotalIncorrectAnswersPercent);
            Assert.Equal(2, result.Questions.Count);

            var resultState = await appService.Close(state.QuizId);
            Assert.NotNull(resultState);
            Assert.Equal(QuizState.Closed.ToString(), resultState.QuizState);
        }

        [Fact]
        public async Task QuizAppService_Test_WrongAnswers()
        {
            await CleanUp();
            var appService = new QuizAppService(_documentStore, _bus);
            var state = await appService.Start(CreateQuiz());
            await appService.Answer(new QuizAnswersCommand (state.QuizId, 
                            new List<QuizAnswer> {
                                new QuizAnswer { 
                                    QuestionId = state.Questions.First().Id, 
                                    OptionId = state.Questions.First().Options.First(x => x.IsCorrect).Id 
                                },
                                new QuizAnswer { 
                                    QuestionId = state.Questions.Last().Id, 
                                    OptionId = state.Questions.Last().Options.First(x => !x.IsCorrect).Id 
                                },
                            }));

            var queryService = new QuizResultsAppService(_documentStore, _bus, null);
            queryService.Start();
            var result = queryService.Get();

            Assert.NotNull(result);
            Assert.Equal(50.0M, result.TotalCorrectAnswersPercent);
            Assert.Equal(50.0M, result.TotalIncorrectAnswersPercent);
            Assert.Equal(2, result.Questions.Count);

            var resultState = await appService.Close(state.QuizId);
            Assert.NotNull(resultState);
            Assert.Equal(QuizState.Closed.ToString(), resultState.QuizState);
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
    }
}
