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

        public Question(Guid id, string description, IEnumerable<QuestionOption> options)
        {
            Id = id;
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
            Id = id;
            Description = description;
            IsCorrect = isCorrect;
        }
    }

    public static class QuizModelFactory
    {
        public static QuizModel Create(int quizId) =>
            new QuizModel(new List<Question>{
                new Question(Guid.NewGuid(), "What .NET Standard implements net461", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), ".NET Standard 1.8"),
                        new QuestionOption(Guid.NewGuid(), ".NET Standard 1.6"),
                        new QuestionOption(Guid.NewGuid(), ".NET Standard 2.0", true),
                    }),
                new Question(Guid.NewGuid(), "What .NET Core version is LTS", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), ".NET Core 2.0"),
                        new QuestionOption(Guid.NewGuid(), ".NET Core 1.1", true),
                        new QuestionOption(Guid.NewGuid(), ".NET Core 1.0"),
                    })
            });
    }
}