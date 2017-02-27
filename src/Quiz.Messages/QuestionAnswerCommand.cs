using System;

namespace Quiz.Messages
{
    public class QuestionAnswerCommand
    {
        public Guid QuestionId { get; }
        public Guid OptionId { get; }

        public QuestionAnswerCommand(Guid questionId, Guid optionId)
        {
            QuestionId = questionId;
            OptionId = optionId;
        }
    }
}