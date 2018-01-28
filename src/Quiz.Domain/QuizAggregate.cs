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

        private QuizState _state = QuizState.Created;

        private QuizModel _model; 

        private QuizAggregate(Guid quizId) => QuizId = quizId;

        public static QuizAggregate Create(Guid quizId, params object[] events) =>
            events.Aggregate(new QuizAggregate(quizId), Reduce);
        
        public void Start(QuizModel quizModel) => 
            TryRaiseEvent(new QuizStartedEvent(QuizId, quizModel));

        public void Close() =>  
            TryRaiseEvent(new QuizClosedEvent(QuizId));

        public void Answer(QuizAnswersCommand command) => 
            TryRaiseEvent(new QuizAnsweredEvent(QuizId, command.Answers));

        public static QuizAggregate Reduce(QuizAggregate state, object @event)
        {
            switch (@event)
            {
                case QuizStartedEvent started:
                    state._model = started.QuizModel;
                    state._state = QuizState.Started;
                    break;
                case QuizClosedEvent closed:
                    state._state = QuizState.Closed;
                    break;
            }

            return state;
        }
        
        public IReadOnlyList<object> GetPendingEvents() => _pendingEvents;

        public void ClearEvent() => _pendingEvents.Clear();

        private void TryRaiseEvent(object @event) 
        {
            if (@event != null && _state.CanRaiseEvent(@event.GetType()))
            {
                _pendingEvents.Add(@event);
                Reduce(this, @event);
            }
        }

        public object GetState()
        {
            return new {
                QuizId,
                State = _state.ToString(),
                Questions = _state == QuizState.Started ? _model.Questions : null
            };
        }
    }    
}