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
            RightAnswersPercent = Math.Round(rightAnswersPercent,2);
            WrongAnswersPercent = Math.Round(wrongAnswersPercent,2);
        }

        public override string ToString() => $@"
            QuestionId:{QuestionId},
            RightAnswers:{RightAnswersPercent},
            WrongAnswers:{WrongAnswersPercent}";
    }
}