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

            Assert.Equal(quizModel.Questions.Count, aggregate.QuestionResults.Count);

            Assert.True(aggregate.QuestionResults.All(
                q => q.Value.options.All(
                    o => o.Value.correct == 0 && o.Value.incorrect == 0)
            ));
        }

        [Fact]
        public void Given_AnsweredQuestion_When_Reduce_Then_QuizResults()
        {
            var quizModel = CreateQuiz();
            var quizId = Guid.NewGuid();
            var question = quizModel.Questions.First();
            var firstOption= question.Options.First();
            var secondOption= question.Options.Last();

            var events = new object[]
            {
                new QuizStartedEvent(quizId, quizModel),
                new QuestionAnsweredEvent(quizId, question.Id, firstOption.Id),
                new QuestionAnsweredEvent(quizId, question.Id, firstOption.Id),
                new QuestionAnsweredEvent(quizId, question.Id, secondOption.Id),
                new QuestionAnsweredEvent(quizId, question.Id, secondOption.Id) 
            };

            var aggregate = QuizResultsAggregate.Create(quizId, events);
            var firstOptionResult = aggregate.QuestionResults[question.Id].options[firstOption.Id];
            var secondOptionResult = aggregate.QuestionResults[question.Id].options[secondOption.Id];

            Assert.Equal(quizModel.Questions.Count, aggregate.QuestionResults.Count);
            Assert.Equal(2, firstOption.IsCorrect ? firstOptionResult.correct : firstOptionResult.incorrect);
            Assert.Equal(2, secondOption.IsCorrect ? secondOptionResult.correct : secondOptionResult.incorrect);
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
    }
}