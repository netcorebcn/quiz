using System;

namespace Quiz.Messages
{
    public class QuestionAnsweredEvent
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
    }
}