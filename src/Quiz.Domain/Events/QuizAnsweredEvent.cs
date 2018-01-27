using System;
using System.Collections.Generic;

namespace Quiz.Domain.Events
{
    public class QuizAnsweredEvent: QuizEvent
    {
        public List<(Guid questionId, Guid optionId)> Answers { get; }

        public QuizAnsweredEvent(Guid quizId, List<(Guid questionId, Guid optionId)> answers) : base(quizId) 
            => Answers = answers;
    }
}