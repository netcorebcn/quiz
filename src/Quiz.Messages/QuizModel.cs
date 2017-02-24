using System;
using System.Collections.Generic;

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
        public Guid Id { get; }
        
        public string Description { get; }
        public IEnumerable<QuestionOption> Options { get; }

        public Question(string description, IEnumerable<QuestionOption> options)
        {
            Id = Guid.NewGuid();
            Description = description;
            Options = options;
        }
    }

    public class QuestionOption
    {
        public string Description { get; }

        public bool IsCorrect { get; }

        public Guid Id { get; }

        public QuestionOption(string description, bool isCorrect = false)
        {
            Id = Guid.NewGuid();
            Description = description;
            IsCorrect = isCorrect;
        }
    }

    public static class QuizModelFactory
    {
        public static QuizModel Create(int quizId) =>
            new QuizModel(new List<Question>{
                new Question("What .NET Standard implements net461", 
                    new List<QuestionOption> {
                        new QuestionOption(".NET Standard 1.8"),
                        new QuestionOption(".NET Standard 1.6"),
                        new QuestionOption(".NET Standard 2.0", true),
                    })
            });
    }
}