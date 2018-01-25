using System;
using System.Collections.Generic;
using Quiz.Domain.Events;

namespace Quiz.Domain
{
    public class QuizModel
    {
        public List<Question> Questions { get; }

        public QuizModel(List<Question> questions) => Questions = questions;
    }

    public class Question
    {
        public Guid Id { get; }
        
        public string Description { get; }

        public List<QuestionOption> Options { get; }

        public Question(Guid id, string description, List<QuestionOption> options)
        {
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
            Description = description;
            Options = options;
        }
    }

    public class QuestionOption
    {
        public string Description { get; }

        public bool IsCorrect { get; }

        public Guid Id { get; }

        public QuestionOption(Guid id, string description, bool isCorrect = false)
        {
            Id = (id == Guid.Empty) ? Guid.NewGuid() : id;
            Description = description;
            IsCorrect = isCorrect;
        }
    }
}
