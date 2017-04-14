using System;
using System.Collections.Generic;

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
    public class QuizAnswersCommand
    {
        public List<QuestionAnswerCommand> Answers { get; }

        public QuizAnswersCommand(List<QuestionAnswerCommand> answers) =>
            Answers = answers;   
    }
}