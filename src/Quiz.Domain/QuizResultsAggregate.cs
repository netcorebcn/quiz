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

        private Dictionary<Guid,QuestionResult> _results; 

        public List<QuestionResult> Questions 
        {
            get => _results.Values.ToList();
        }

        private QuizResultsAggregate(Guid quizId) => QuizId = quizId;

        public static QuizResultsAggregate Create(Guid quizId, params object[] events) =>
            events.Aggregate(new QuizResultsAggregate(quizId), Reduce);
        
        private static QuizResultsAggregate Reduce(QuizResultsAggregate state, object @event)
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
            state._results = @event.QuizModel.Questions.ToDictionary(
                q => q.Id,
                q => new QuestionResult(q) 
            );

            return state;
        }

        private static QuizResultsAggregate Reduce(QuizResultsAggregate state, QuizAnsweredEvent @event)
        {
            @event.Answers.ForEach(answer => state._results[answer.QuestionId].Reduce(answer.OptionId));
            return state;
        }
    }    

    public class QuestionResult
    {
        public Guid Id { get; }
        public string Description { get; }
        private Guid _correctOption; 
        private decimal _correctAnswers;
        private decimal _incorrectAnswers; 

        public decimal CorrectAnswersPercent { get; private set; }
        public decimal IncorrectAnswersPercent { get; private set; }

        public QuestionResult(Question question)
        {
            Id = question.Id;
            Description = question.Description;
            _correctOption = question.Options.First(o => o.IsCorrect).Id; 
        }

        public void Reduce(Guid selectedOption)
        {
            if (_correctOption == selectedOption)
            {
                _correctAnswers =+ 1;
            }
            else
            {
                _incorrectAnswers =+ 1;
            }

            var total = _correctAnswers + _incorrectAnswers;
            CorrectAnswersPercent = Percent(_correctAnswers);
            IncorrectAnswersPercent = Percent(_incorrectAnswers);

            decimal Percent(decimal answers) => (answers/total) * 100;
        }
    }
}