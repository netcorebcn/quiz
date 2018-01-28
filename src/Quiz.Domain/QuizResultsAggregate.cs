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

        public Dictionary<Guid,QuestionResult> QuestionResults 
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
                q => new QuestionResult {
                    Description = q.Description,
                    CorrectOption = q.Options.First(o => o.IsCorrect).Id, 
                    CorrectAnswers = 0, 
                    IncorrectAnswers = 0
                }
            );

            return state;
        }

        private static QuizResultsAggregate Reduce(QuizResultsAggregate state, QuizAnsweredEvent @event)
        {
            @event.Answers.ForEach(answer => state.QuestionResults[answer.QuestionId] = Reduce(state.QuestionResults[answer.QuestionId], answer.OptionId));

            QuestionResult Reduce(QuestionResult questionResult, Guid optionId) =>
                new QuestionResult {
                    Description = questionResult.Description,
                    CorrectOption = questionResult.CorrectOption, 
                    CorrectAnswers = questionResult.CorrectOption == optionId ? questionResult.CorrectAnswers + 1 : questionResult.CorrectAnswers,
                    IncorrectAnswers = questionResult.CorrectOption == optionId ? questionResult.IncorrectAnswers : questionResult.IncorrectAnswers + 1
                };

            return state;
        }
    }    

    public class QuestionResult
    {
        public string Description { get; set; }
        public Guid CorrectOption { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
    }
}