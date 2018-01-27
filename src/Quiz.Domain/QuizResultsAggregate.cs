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

        public Dictionary<Guid,(Guid correctOption, int correctAnswers, int incorrectAnswers)> QuestionResults 
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
                case QuizAnsweredEvent answered:
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
                    correctOption: q.Options.First(o => o.IsCorrect).Id, 
                    correctAnswers:0, incorrectAnswers:0)
            );

            return state;
        }

        private static QuizResultsAggregate Reduce(QuizResultsAggregate state, QuizAnsweredEvent @event)
        {
            @event.Answers.ForEach(answer => state.QuestionResults[answer.questionId] = Reduce(state.QuestionResults[answer.questionId], answer.optionId));

            (Guid correctOption, int correctAnswers, int incorrectAnswers) Reduce((Guid correctOption, int correctAnswers, int incorrectAnswers) questionResult, Guid optionId) =>
                (questionResult.correctOption, 
                    questionResult.correctOption == optionId ? questionResult.correctAnswers + 1 : questionResult.correctAnswers,
                    questionResult.correctOption == optionId ? questionResult.incorrectAnswers : questionResult.incorrectAnswers + 1);

            return state;
        }
    }    
}