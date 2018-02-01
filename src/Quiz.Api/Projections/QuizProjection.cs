using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Marten.Events.Projections;
using Marten.Services;
using Quiz.Domain;
using Quiz.Domain.Events;

namespace Quiz.Api.Projections
{
    public class QuizProjection : ViewProjection<CurrentQuizAggregate, Guid>
    {        
        public QuizProjection()
        {
            ProjectEvent<QuizStartedEvent>
            (
                (s, e) => SelectProjection(s, e.QuizId),
                (p, e) => p.Id = e.QuizId
            );

            DeleteEvent<QuizClosedEvent>(e => e.QuizId);

            Guid SelectProjection(IDocumentSession session, Guid quizId) =>
                session.Load<CurrentQuizAggregate>(quizId)?.Id ?? quizId;
        }
    }
}   