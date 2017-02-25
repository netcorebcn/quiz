using System;

namespace Quiz.Messages
{
    public class QuestionAnswerCommand
    {
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }
    }
}