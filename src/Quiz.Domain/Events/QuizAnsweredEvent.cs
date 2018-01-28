using System;
using System.Collections.Generic;
using Quiz.Domain.Commands;

namespace Quiz.Domain.Events
{
    public class QuizAnsweredEvent: QuizEvent
    {
        public List<QuizAnswer> Answers { get; }

        public QuizAnsweredEvent(Guid quizId, List<QuizAnswer> answers) : base(quizId) 
            => Answers = answers;
    }
}