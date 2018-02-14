using System;
using System.Collections.Generic;
using Quiz.Domain.Events;

namespace Quiz.Domain.Commands
{
    public class QuizAnswersCommand
    {
        public Guid QuizId { get; set; }

        public List<QuizAnswer> Answers { get; }

        public QuizAnswersCommand(Guid quizId, List<QuizAnswer> answers)
        {
            QuizId = quizId;
            Answers = answers;  
        }   
    }
}