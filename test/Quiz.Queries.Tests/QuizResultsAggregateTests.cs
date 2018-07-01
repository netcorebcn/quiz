using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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

            Assert.Equal(quizModel.Questions.Count, aggregate.Questions.Count);
            Assert.True(
                aggregate.Questions.All(
                    q => q.CorrectAnswersPercent == 0 && q.IncorrectAnswersPercent == 0));
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

            Assert.Equal(quizModel.Questions.Count, aggregate.Questions.Count);
            Assert.Equal(100, aggregate.Questions.First(q => q.Id == firstQuestion.Id).CorrectAnswersPercent);
            Assert.Equal(100, aggregate.Questions.First(q => q.Id == lastQuestion.Id).IncorrectAnswersPercent);

            Assert.True(aggregate.Questions.Where(q => q.Id != firstQuestion.Id && q.Id != lastQuestion.Id)
                    .All(q => q.CorrectAnswersPercent == 0 && q.IncorrectAnswersPercent == 0));
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
    }
}