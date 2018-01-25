using System;
using Quiz.Domain.Events;
using Xunit;

namespace Quiz.Domain.Tests
{
    public class QuizStateTests 
    {
        [Theory]
        [InlineData(typeof(QuizStartedEvent))]
        public void Given_QuizState_When_Empty_Then_CanRaiseEvent(Type eventType) => 
            Assert.True(QuizState.Empty.CanRaiseEvent(eventType));

        [Theory]
        [InlineData(typeof(QuizClosedEvent))]
        [InlineData(typeof(QuestionAnsweredEvent))]
        public void Given_QuizState_When_Empty_Then_CanNotRaiseEvent(Type eventType) => 
            Assert.False(QuizState.Empty.CanRaiseEvent(eventType));

        [Theory]
        [InlineData(typeof(QuizClosedEvent))]
        [InlineData(typeof(QuestionAnsweredEvent))]
        public void Given_QuizState_When_Started_Then_CanRaiseEvent(Type eventType) => 
            Assert.True(QuizState.Started.CanRaiseEvent(eventType));

        [Theory]
        [InlineData(typeof(QuizStartedEvent))]
        public void Given_QuizState_When_Started_Then_CanNotRaiseEvent(Type eventType) => 
            Assert.False(QuizState.Started.CanRaiseEvent(eventType));

        [Theory]
        [InlineData(typeof(QuizClosedEvent))]
        [InlineData(typeof(QuizStartedEvent))]
        [InlineData(typeof(QuestionAnsweredEvent))]
        public void Given_QuizState_When_Closed_Then_CanNotRaiseEvent(Type eventType) => 
            Assert.False(QuizState.Closed.CanRaiseEvent(eventType));

    [Fact]
        public void Given_Two_QuizStates_When_Comparing_Then_True()
        {
            var state = QuizState.Started;

            Assert.True(state == QuizState.Started);
            Assert.Equal(state, QuizState.Started);
            Assert.False(state == QuizState.Closed);
            Assert.NotEqual(state, QuizState.Closed);
        }
    }
}