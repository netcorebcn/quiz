using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Quiz.Domain;
using Quiz.Domain.Commands;
using Quiz.Domain.Events;
using Xunit;

namespace Quiz.Domain.Tests
{
    public class QuizAggregateTests
    {
        private QuizModel model; 

        public QuizAggregateTests() => model = CreateQuiz();

        [Fact]
        public void Given_New_Quiz_When_Start_Then_QuizStarted() => 
            ExecuteCommand(quiz => quiz.Start(model))
                .GetPendingEvents()
                .AssertLastEventOfType<QuizStartedEvent>()
                .WithTotalCount(1);

        [Fact]
        public void Given_Started_Quiz_When_Start_Then_QuizStarted() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Start(model);
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizStartedEvent>()
                .WithTotalCount(1);

        [Fact]
        public void Given_Started_Quiz_When_Answer_Then_QuestionAnswered() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizAnsweredEvent>()
                .WithTotalCount(2);

        [Fact]
        public void Given_Started_Quiz_When_Multiple_Answer_Then_QuestionAnswered() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizAnsweredEvent>()
                .WithTotalCount(3);

        [Fact]
        public void Given_Started_Quiz_When_Closed_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                    quiz.Close();
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(3);

        [Fact]
        public void Given_Closed_Quiz_When_Closed_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                    quiz.Close();
                    quiz.Close();
                    quiz.Close();
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(3);

        [Fact]
        public void Given_Closed_Quiz_When_Open_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                    quiz.Close();
                    quiz.Start(model);
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(3);

        [Fact]
        public void Given_Closed_Quiz_When_Answer_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                    quiz.Close();
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateQuizAnswers()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(4);

        [Fact]
        public void Given_Started_Quiz_When_Answer_With_Invalid_QuizId_Then_QuizAnswered_NotRaised() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(Guid.NewGuid(), CreateQuizAnswers()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizStartedEvent>()
                .WithTotalCount(1);

        [Fact]
        public void Given_Started_Quiz_When_Answer_With_Invalid_Question_Then_QuizAnswered_NotRaised() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(model);
                    quiz.Answer(new QuizAnswersCommand(quiz.QuizId, CreateInvalidQuizAnswers()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizStartedEvent>()
                .WithTotalCount(1);

        private QuizAggregate ExecuteCommand(Action<QuizAggregate> command)
        {
            var quizId = Guid.NewGuid();
            var quiz = QuizAggregate.Create(quizId);
            command(quiz);
            return quiz;
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));

        private List<QuizAnswer> CreateQuizAnswers() => 
            model.Questions.Select(q => new QuizAnswer
            {
                QuestionId = q.Id,
                OptionId = q.Options.First().Id
            })
            .ToList();

        private List<QuizAnswer> CreateInvalidQuizAnswers() => 
            model.Questions.Select(q => new QuizAnswer
            {
                QuestionId = q.Id,
                OptionId = Guid.NewGuid()
            })
            .ToList();
    }

    public static class AggregateTestsExtensions
    {
        public static IEnumerable<object> AssertLastEventOfType<T>(this IEnumerable<object> events, int eventsCount = 1) 
        {
            var lastEvent = events.LastOrDefault();
            Assert.NotNull(lastEvent);
            Assert.Equal(typeof(T), lastEvent.GetType());
            return events;
        }

        public static IEnumerable<object> WithTotalCount(this IEnumerable<object> events, int totalCount = 1)
        {
            Assert.Equal(events.Count(), totalCount);
            return events;
        }
    }
}
