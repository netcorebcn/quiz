using System;
using System.Collections.Generic;
using System.Linq;
using Quiz.Domain.Events;

namespace Quiz.Domain
{
    public sealed class QuizState : Enumeration
    {
        private readonly Type[] _raisableEvents;

        private QuizState(string name, Type[] raisableEvents) : base(name) => 
            _raisableEvents = raisableEvents;

        public bool CanRaiseEvent(Type @eventType) => _raisableEvents.Any(x => x == @eventType);

        public static readonly QuizState Empty = new QuizState
        (
            nameof(Empty),
            raisableEvents: new Type[]
            {
                typeof(QuizStartedEvent)
            }
        );

        public static readonly QuizState Started = new QuizState
        (
            nameof(Started),
            raisableEvents: new Type[]
            {
                typeof(QuestionAnsweredEvent),
                typeof(QuizClosedEvent),
            }
        );

        public static readonly QuizState Closed = new QuizState
        (
            nameof(Closed),
            raisableEvents: new Type[] {}
        );
    }
}