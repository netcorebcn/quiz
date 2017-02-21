using System;
using System.Collections.Generic;
using System.Linq;

namespace Quiz.Messages
{
    public class QuizModel
    {
        public IEnumerable<Question> Questions { get; }

        public QuizModel (IEnumerable<Question> questions)
        {
            Questions = questions;
        }
    }

    public class Question
    {
        public string Description { get; }
        public IEnumerable<QuestionOption> Options { get; }

        public Question(string description, IEnumerable<QuestionOption> options)
        {
            Description = description;
            Options = options;
        }
    }

    public class QuestionOption
    {
        public string Description { get; }

        public QuestionOption(string description)
        {
            Description = description;
        }
    }
}