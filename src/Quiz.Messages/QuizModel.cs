using System;
using System.Collections.Generic;

namespace Quiz.Messages
{
    public class QuizReadModel
    {
        public Guid QuizId { get; set; }
        public QuizModel QuizModel { get; set; }
        public List<QuestionStatisticCreatedEvent> Questions { get; set;}
    }

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
        public static QuizModel Create() =>
            new QuizModel(new List<Question>{
                new Question(Guid.NewGuid(), "What .NET Standard implements net461", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), ".NET Standard 1.8"),
                        new QuestionOption(Guid.NewGuid(), ".NET Standard 1.6"),
                        new QuestionOption(Guid.NewGuid(), ".NET Standard 2.0", true)
                    }),
                new Question(Guid.NewGuid(), "Which is the managed option for using swarm in azure", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), "ACS", true),
                        new QuestionOption(Guid.NewGuid(), "Docker for Azure"),
                        new QuestionOption(Guid.NewGuid(), "VM extensions")
                    })
            });
    }
}
