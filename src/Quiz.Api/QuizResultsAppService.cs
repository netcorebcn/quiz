using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Api
{
    public class QuizResultsAppService
    {
        private readonly IDocumentStore _eventStore;

        public QuizResultsAppService(IDocumentStore eventStore) => _eventStore = eventStore;

        public async Task<object> Get()
        {
            using(var session = _eventStore.OpenSession())
            {
                var currentQuiz = await session.Query<CurrentQuizAggregate>().FirstOrDefaultAsync();
                if (currentQuiz == null)
                {
                    return QuizResultsAggregate.Empty;
                }

                return await Get(currentQuiz.Id);
            }
        }

        public async Task<object> Get(Guid quizId) 
        {
            using(var session = _eventStore.OpenSession())
            {
                var events = (await session.Events.FetchStreamAsync(quizId)).Select(@event => @event.Data).ToArray();
                return QuizResultsAggregate.Create(quizId, events);
            }
        }
    }
}