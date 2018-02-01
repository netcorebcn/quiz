using System;

namespace Quiz.Domain.Events
{
    public class QuizStartedEvent:QuizEvent
    {
        public QuizModel QuizModel { get; }

        public QuizStartedEvent(Guid quizId, QuizModel quizModel):base(quizId)
        {
            QuizModel = quizModel;
        }
    }
}