using System;
using System.Collections.Generic;
using System.Linq;
using Quiz.Messages;

namespace Quiz.Domain
{
    public class Question
    {
        public IEnumerable<QuestionOption> Options { get; }

        public void Apply(QuestionAnsweredEvent @event)
        {
            // var option = Options.FirstOrDefault(@event.OptionId);
            // if (option != null)
            //     option.Vote();
        }
    }

    public class QuestionOption
    {
        public int Votes { get; }

        public void Vote()
        {
            // Votes++;
        }
    }
}