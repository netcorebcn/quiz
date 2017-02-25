using System;

namespace Quiz.Messages
{
    public class QuestionWrongAnsweredEvent
    {
        public Guid QuestionId { get; }
        public Guid OptionId { get; }

        public QuestionWrongAnsweredEvent(Guid questionId, Guid optionId)
        {
            QuestionId = questionId;
            OptionId = optionId;
        }
    }

    public class QuestionRightAnsweredEvent
    {
        public Guid QuestionId { get; }
        public Guid OptionId { get; }

        public QuestionRightAnsweredEvent(Guid questionId, Guid optionId)
        {
            QuestionId = questionId;
            OptionId = optionId;
        }
    }
}