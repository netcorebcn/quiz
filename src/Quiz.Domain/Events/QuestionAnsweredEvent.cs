using System;

namespace Quiz.Domain.Events
{
    public class QuestionAnsweredEvent: QuizEvent
    {
        public Guid QuestionId { get; }
        public Guid OptionId { get; }

        public QuestionAnsweredEvent(Guid quizId, Guid questionId, Guid optionId):base(quizId)
        {
            QuestionId = questionId;
            OptionId = optionId;
        }
    }
}