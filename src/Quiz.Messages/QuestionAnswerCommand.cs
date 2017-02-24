using System;

namespace Quiz.Messages
{
    public class QuestionAnswerCommand
    {
        public Guid QuizId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid OptionId { get; set; }
    }
}