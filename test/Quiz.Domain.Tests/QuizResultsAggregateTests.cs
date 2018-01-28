using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Quiz.Domain.Commands;
using Quiz.Domain.Events;
using Xunit;

namespace Quiz.Domain.Tests
{
    public class QuizResultsAggregateTests
    {
        [Fact]
        public void Given_StartedEvent_When_Reduce_Then_QuizResults_Initialized()
        {
            var quizModel = CreateQuiz();
            var events = new object[]
            {
                new QuizStartedEvent(Guid.NewGuid(), quizModel) 
            };

            var aggregate = QuizResultsAggregate.Create(Guid.NewGuid(), events);

            Assert.Equal(quizModel.Questions.Count, aggregate.QuestionResults.Count);
            Assert.True(
                aggregate.QuestionResults.All(
                    q => q.Value.CorrectAnswers == 0 && q.Value.IncorrectAnswers == 0));
        }

        [Fact]
        public void Given_AnsweredQuestion_When_Reduce_Then_QuizResults()
        {
            var quizModel = CreateQuiz();
            var quizId = Guid.NewGuid();
            var firstQuestion = quizModel.Questions.First();
            var lastQuestion = quizModel.Questions.Last();
            var firstOption= firstQuestion.Options.First();

            var events = new object[]
            {
                new QuizStartedEvent(quizId, quizModel),
                new QuizAnsweredEvent(quizId, new List<QuizAnswer> 
                    { 
                        new QuizAnswer 
                        {
                            QuestionId = firstQuestion.Id, 
                            OptionId = firstQuestion.Options.First(o => o.IsCorrect).Id,
                        },
                        new QuizAnswer
                        {
                            QuestionId = lastQuestion.Id, 
                            OptionId = lastQuestion.Options.First(o => !o.IsCorrect).Id,
                        }    
                    }),
            };

            var aggregate = QuizResultsAggregate.Create(quizId, events);

            Assert.Equal(quizModel.Questions.Count, aggregate.QuestionResults.Count);
            Assert.Equal(1, aggregate.QuestionResults[firstQuestion.Id].CorrectAnswers);
            Assert.Equal(1, aggregate.QuestionResults[lastQuestion.Id].IncorrectAnswers);
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
    }
}