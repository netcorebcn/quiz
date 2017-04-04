using System;

namespace Quiz.Messages
{
    public class QuestionStatisticCreatedEvent
    {
        public Guid QuestionId { get; }
        public decimal RightAnswersPercent { get; }

        public decimal WrongAnswersPercent { get; }

        public QuestionStatisticCreatedEvent(Guid questionId, decimal rightAnswersPercent, decimal wrongAnswersPercent)
        {
            QuestionId = questionId;
            RightAnswersPercent = rightAnswersPercent;
            WrongAnswersPercent = wrongAnswersPercent;
        }
    }
}