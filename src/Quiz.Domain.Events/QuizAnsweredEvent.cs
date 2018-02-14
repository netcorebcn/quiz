using System;
using System.Collections.Generic;

namespace Quiz.Domain.Events
{
    public class QuizAnsweredEvent: QuizEvent
    {
        public List<QuizAnswer> Answers { get; }

        public QuizAnsweredEvent(Guid quizId, List<QuizAnswer> answers) : base(quizId) 
            => Answers = answers;
    }
    
    public class QuizAnswer
    {
        public Guid QuestionId { get; set; }  
        public Guid OptionId { get; set; }
    }
}