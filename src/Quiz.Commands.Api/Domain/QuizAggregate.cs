using System;
using System.Collections.Generic;
using System.Linq;
using Quiz.Domain.Commands;
using Quiz.Domain.Events;

namespace Quiz.Domain
{
    public class QuizAggregate 
    {
        private readonly List<QuizEvent> _pendingEvents = new List<QuizEvent>();

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

        public void Answer(QuizAnswersCommand command) 
        {
            if (command.QuizId == QuizId &&
                command.Answers.All(a => _model.Questions
                        .Any(q => q.Options
                        .Any(o => q.Id == a.QuestionId && o.Id == a.OptionId))))
            {
                TryRaiseEvent(new QuizAnsweredEvent(QuizId, command.Answers));
            }
        } 

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
        
        public IReadOnlyList<QuizEvent> GetPendingEvents() => _pendingEvents;

        public void ClearEvent() => _pendingEvents.Clear();

        private void TryRaiseEvent(QuizEvent @event) 
        {
            if (@event != null && _state.CanRaiseEvent(@event.GetType()))
            {
                _pendingEvents.Add(@event);
                Reduce(this, @event);
            }
        }

        public static QuizAggregateState Empty 
        { 
            get => new QuizAggregateState
            {
                QuizId = Guid.Empty,
                QuizState = QuizState.Created.ToString(),
                Questions = new List<Question>()
            }; 
        }

        public QuizAggregateState GetState()
        {
            return new QuizAggregateState {
                QuizId = QuizId,
                QuizState = _state.ToString(),
                Questions = _state == QuizState.Started ? _model.Questions : null
            };
        }
    }    
}