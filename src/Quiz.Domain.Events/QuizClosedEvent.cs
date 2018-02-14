using System;

namespace Quiz.Domain.Events
{
    public class QuizClosedEvent: QuizEvent
    {
        public QuizClosedEvent(Guid quizId) : base(quizId)
        {
        }
    }
}