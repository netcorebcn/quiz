using System;

namespace Quiz.Messages
{
    public class QuestionAnswerCommand
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
    }
}