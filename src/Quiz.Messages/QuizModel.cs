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
                new Question(Guid.NewGuid(), "Which is the most awesome cloud provider?", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), "AWS"),
                        new QuestionOption(Guid.NewGuid(), "Azure", true),
                        new QuestionOption(Guid.NewGuid(), "GCE")
                    }),
                new Question(Guid.NewGuid(), "Which is the Docker native built-in orchestrator?", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), "DC/OS"),
                        new QuestionOption(Guid.NewGuid(), "Kubernetes"),
                        new QuestionOption(Guid.NewGuid(), "Swarm", true),
                        new QuestionOption(Guid.NewGuid(), "CoreOS"),
                    }),
                new Question(Guid.NewGuid(), "Which is the more managed option for using Swarm in Azure?", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), "Azure Container Service", true),
                        new QuestionOption(Guid.NewGuid(), "Docker for Azure"),
                        new QuestionOption(Guid.NewGuid(), "Docker VM Extension for Microsoft Azures")
                    }),
                 new Question(Guid.NewGuid(), "What is the average size of a .NET Core runtime image?", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), "1 GB"),
                        new QuestionOption(Guid.NewGuid(), "100 MB"),
                        new QuestionOption(Guid.NewGuid(), "300 MB", true)
                    }),
                new Question(Guid.NewGuid(), "How do you upload your awesome-app image to a Container Registry?", 
                    new List<QuestionOption> {
                        new QuestionOption(Guid.NewGuid(), "docker push awesome-app", true),
                        new QuestionOption(Guid.NewGuid(), "git push awesome-app"),
                        new QuestionOption(Guid.NewGuid(), "docker pull awesome-app")
                    })
            });
    }
}
