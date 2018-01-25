using System;
using System.Collections.Generic;
using System.Linq;
using Quiz.Domain.Commands;
using Quiz.Domain.Events;

namespace Quiz.Domain
{
    public class QuizAggregate 
    {
        private readonly List<object> _pendingEvents = new List<object>();

        public Guid QuizId { get; }

        public QuizState State { get; private set; } = QuizState.Empty;

        public QuizAggregate(Guid quizId) => QuizId = quizId;
        
        public void Start(QuizModel quizModel) => 
            TryRaiseEvent(new QuizStartedEvent(QuizId, quizModel));

        public void Close() =>  
            TryRaiseEvent(new QuizClosedEvent(QuizId));

        public void Answer(QuestionAnswerCommand command) => 
            TryRaiseEvent(new QuestionAnsweredEvent(QuizId, command.QuestionId, command.OptionId));

        public static QuizAggregate Reduce(QuizAggregate state, object @event)
        {
            switch (@event)
            {
                case QuizStartedEvent started:
                    state.State = QuizState.Started;
                    break;
                case QuizClosedEvent closed:
                    state.State = QuizState.Closed;
                    break;
            }

            return state;
        }
        
        public IReadOnlyList<object> GetPendingEvents() => _pendingEvents;

        public void ClearEvent() => _pendingEvents.Clear();

        private void TryRaiseEvent(object @event) 
        {
            if (@event != null && State.CanRaiseEvent(@event.GetType()))
            {
                _pendingEvents.Add(@event);
                Reduce(this, @event);
            }
        }
    }    
}