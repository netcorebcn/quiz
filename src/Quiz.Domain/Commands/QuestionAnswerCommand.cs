using System;
using System.Collections.Generic;

namespace Quiz.Domain.Commands
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
        public Guid QuizId { get; set; }

        public List<QuestionAnswerCommand> Answers { get; }

        public QuizAnswersCommand(Guid quizId, List<QuestionAnswerCommand> answers)
        {
            QuizId = quizId;
            Answers = answers;  
        }   
    }
}