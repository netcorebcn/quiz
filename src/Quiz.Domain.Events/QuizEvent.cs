using System;

namespace Quiz.Domain.Events
{
    public abstract class QuizEvent
    {
        public Guid QuizId { get; }

        protected QuizEvent(Guid quizId) => QuizId = quizId;
    }
}