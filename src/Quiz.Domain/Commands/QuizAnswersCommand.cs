using System;
using System.Collections.Generic;

namespace Quiz.Domain.Commands
{
    public class QuizAnswersCommand
    {
        public Guid QuizId { get; set; }

        public List<(Guid questionId, Guid optionId)> Answers { get; }

        public QuizAnswersCommand(Guid quizId, List<(Guid questionId, Guid optionId)> answers)
        {
            QuizId = quizId;
            Answers = answers;  
        }   
    }
}