using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Api
{
    public class QuizAppService
    {
        private readonly IDocumentStore _eventStore;

        public QuizAppService(IDocumentStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<object> GetState(Guid quizId)
        {
            using(var session = _eventStore.OpenSession())
            {
                var events = await GetEvents(session, quizId);
                var aggregate = QuizAggregate.Create(quizId, events);
                return GetState(quizId, aggregate, events);
            }
        }

        public async Task<object> Start(QuizModel quizModel) => 
            await ExecuteTransaction(Guid.NewGuid(), aggregate => aggregate.Start(quizModel));

        public async Task<object> Answer(QuizAnswersCommand command) => 
            await ExecuteTransaction(command.QuizId, aggregate => aggregate.Answer(command));

        public async Task<object> Close(Guid quizId) => 
            await ExecuteTransaction(quizId, aggregate => aggregate.Close());

        private async Task<object> ExecuteTransaction(Guid quizId, Action<QuizAggregate> command)
        {
            using(var session = _eventStore.OpenSession())
            {
                var eventStreamState = await session.Events.FetchStreamStateAsync(quizId);
                var events = await GetEvents(session, quizId);

                var aggregate = QuizAggregate.Create(quizId, events);
                command(aggregate);

                var expectedVersion = (eventStreamState?.Version ?? 0) + aggregate.GetPendingEvents().Count();
                session.Events.Append(aggregate.QuizId, expectedVersion, aggregate.GetPendingEvents().ToArray());
                await session.SaveChangesAsync();

                return GetState(quizId, aggregate, events);
            }
        }
        private object GetState(Guid quizId, QuizAggregate aggregate, object[] events) => 
            new
            {
                quizId,
                aggregate.State,
                QuizResultsAggregate
                    .Create(quizId, events.Concat(aggregate.GetPendingEvents()))
                    .QuestionResults
            };
            
        private async Task<object[]> GetEvents(IDocumentSession session, Guid quizId) => 
            (await session.Events.FetchStreamAsync(quizId)).Select(@event => @event.Data).ToArray();
    }
}