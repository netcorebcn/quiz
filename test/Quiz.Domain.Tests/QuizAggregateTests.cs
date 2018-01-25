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
        [Fact]
        public void Given_New_Quiz_When_Start_Then_QuizStarted() => 
            ExecuteCommand(quiz => quiz.Start(CreateQuiz()))
                .GetPendingEvents()
                .AssertLastEventOfType<QuizStartedEvent>()
                .WithTotalCount(1);

        [Fact]
        public void Given_Started_Quiz_When_Start_Then_QuizStarted() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(CreateQuiz());
                    quiz.Start(CreateQuiz());
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizStartedEvent>()
                .WithTotalCount(1);

        [Fact]
        public void Given_Started_Quiz_When_Answer_Then_QuestionAnswered() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(CreateQuiz());
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuestionAnsweredEvent>()
                .WithTotalCount(2);

        [Fact]
        public void Given_Started_Quiz_When_Multiple_Answer_Then_QuestionAnswered() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(CreateQuiz());
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuestionAnsweredEvent>()
                .WithTotalCount(4);

        [Fact]
        public void Given_Started_Quiz_When_Closed_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(CreateQuiz());
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                    quiz.Close();
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(3);

        [Fact]
        public void Given_Closed_Quiz_When_Closed_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(CreateQuiz());
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
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
                    quiz.Start(CreateQuiz());
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                    quiz.Close();
                    quiz.Start(CreateQuiz());
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(3);

        [Fact]
        public void Given_Closed_Quiz_When_Answer_Then_QuizClosed() => 
            ExecuteCommand(quiz => 
                {
                    quiz.Start(CreateQuiz());
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                    quiz.Close();
                    quiz.Answer(new QuestionAnswerCommand(Guid.NewGuid(), Guid.NewGuid()));
                })
                .GetPendingEvents()
                .AssertLastEventOfType<QuizClosedEvent>()
                .WithTotalCount(3);

        private QuizAggregate ExecuteCommand(Action<QuizAggregate> command)
        {
            var quizId = Guid.NewGuid();
            var quiz = new QuizAggregate(quizId);
            command(quiz);
            return quiz;
        }

        private QuizModel CreateQuiz() =>
            JsonConvert.DeserializeObject<QuizModel>(File.ReadAllText("quiz.json"));
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
