using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Quiz.Domain;

namespace Quiz.Api
{
    [Route("[controller]")]
    public class QuizResultsController
    {
        private readonly IDocumentStore _documentStore;

        public QuizResultsController(IDocumentStore documentStore) => _documentStore = documentStore;

        [HttpGet]
        public async Task<object> Get()
        {
            using(var session = _documentStore.OpenSession())
            {
                var currentQuiz = await session.Query<CurrentQuizAggregate>().FirstOrDefaultAsync();
                return await Get(currentQuiz.Id);
            }
        }

        [HttpGet("{quizId}")]
        public async Task<object> Get(Guid quizId) 
        {
            using(var session = _documentStore.OpenSession())
            {
                var events = (await session.Events.FetchStreamAsync(quizId)).Select(@event => @event.Data).ToArray();
                return QuizResultsAggregate.Create(quizId, events);
            }
        }
    }
}
