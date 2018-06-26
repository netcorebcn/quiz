using System;
using System.Collections.Generic;

namespace Quiz.Domain
{
    public class QuizAggregateState
    {
        public Guid QuizId { get; set; }
        
        public string QuizState { get; set; }

        public List<Question> Questions { get; set; }
    }
}