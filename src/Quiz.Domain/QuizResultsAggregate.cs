using System;
using System.Collections.Generic;
using System.Linq;
using Quiz.Domain.Commands;
using Quiz.Domain.Events;

namespace Quiz.Domain
{
    public class QuizResultsAggregate 
    {
        public Guid QuizId { get; }

        public Dictionary<Guid, (string description, Dictionary<Guid, (string description, bool isCorrect, int correct, int incorrect)> options)> QuestionResults 
        { 
            get; 
            private set; 
        }

        private QuizResultsAggregate(Guid quizId) => QuizId = quizId;

        public static QuizResultsAggregate Create(Guid quizId, params object[] events) =>
            events.Aggregate(new QuizResultsAggregate(quizId), Reduce);
        
        public static QuizResultsAggregate Reduce(QuizResultsAggregate state, object @event)
        {
            switch (@event)
            {
                case QuizStartedEvent started:
                    state = Reduce(state, started);
                    break;
                case QuestionAnsweredEvent answered:
                    state = Reduce(state, answered);
                    break;
            }

            return state;
        }
        
        private static QuizResultsAggregate Reduce(QuizResultsAggregate state, QuizStartedEvent @event)
        {
            state.QuestionResults = @event.QuizModel.Questions.ToDictionary(
                q => q.Id,
                q => (
                    description: q.Description, 
                    options: q.Options.ToDictionary(
                       o => o.Id,
                       o => (
                           description: o.Description, 
                           isCorrect: o.IsCorrect, 
                           correctAnswers:0, 
                           incorrectAnswers: 0)
                ))
            );
            return state;
        }

        private static QuizResultsAggregate Reduce(QuizResultsAggregate state, QuestionAnsweredEvent @event)
        {
            state.QuestionResults = state.QuestionResults.ToDictionary(
                q => q.Key ,
                q => (
                    description: q.Value.description, 
                    options: q.Value.options.ToDictionary(
                       o => o.Key,
                       o => o.Key == @event.OptionId 
                            ? (description: o.Value.description, 
                                isCorrect: o.Value.isCorrect, 
                                correct: o.Value.isCorrect ? o.Value.correct + 1 : o.Value.correct, 
                                incorrect: o.Value.isCorrect ? o.Value.incorrect : o.Value.incorrect + 1)
                            : o.Value
                ))
            );
            return state;
        }
    }    
}