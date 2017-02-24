using System;
using System.Linq;
using Quiz.EventSourcing.Domain;
using Quiz.Messages;

namespace Quiz.Voting.Domain
{
    public class QuizAggregate : AggregateRoot
    {
        public QuizModel QuizModel { get; set; }

        public int CorrectAnswers { get; private set; }

        public int WrongAnswers { get; private set; }

        public void Start(QuizModel quizModel) => 
            this.RaiseEvent(new QuizStartedEvent(quizModel));

        public void Close() =>  
            this.RaiseEvent(new QuizClosedEvent(CorrectAnswers, WrongAnswers));

        public void Vote(Guid questionId, Guid optionId)
        {
            var option = QuizModel.Questions.FirstOrDefault(x => x.Id == questionId)?
                .Options.FirstOrDefault(x => x.Id == optionId);

            if (option.IsCorrect)           
                this.RaiseEvent(new QuestionRightAnsweredEvent(questionId, optionId));
            else
                this.RaiseEvent(new QuestionWrongAnsweredEvent(questionId, optionId));
        }
        public void Apply(QuizStartedEvent @event) => 
            QuizModel = @event.QuizModel;

        public void Apply(QuizClosedEvent @event)
        {
            CorrectAnswers = @event.RightAnswers;
            WrongAnswers = @event.WrongAnswers;
        }

        public void Apply(QuestionRightAnsweredEvent @event) => 
            CorrectAnswers ++;

        public void Apply(QuestionWrongAnsweredEvent @event) => 
            WrongAnswers ++;
    }    
}