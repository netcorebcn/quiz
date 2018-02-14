using System;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Marten;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Api
{
    public class QuizAppService
    {
        private readonly IDocumentStore _eventStore;
        private readonly IBus _bus;

        public QuizAppService(IDocumentStore eventStore, IBus bus) 
        {
            _eventStore = eventStore;
            _bus = bus;
        }

        public async Task<object> GetState()
        {
            using (var session = _eventStore.OpenSession())
            {
                var currentQuiz = await session.Query<CurrentQuizAggregate>().FirstOrDefaultAsync();
                if (currentQuiz != null)
                {
                    return await GetState(currentQuiz.Id);
                }
            }

            return QuizAggregate.Empty;
        }

        public async Task<object> GetState(Guid quizId)
        {
            using(var session = _eventStore.OpenSession())
            {
                var aggregate = await CreateAggregate(session, quizId);
                return aggregate.GetState();
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

                var aggregate = await CreateAggregate(session, quizId);
                command(aggregate);

                var expectedVersion = (eventStreamState?.Version ?? 0) + aggregate.GetPendingEvents().Count();
                session.Events.Append(aggregate.QuizId, expectedVersion, aggregate.GetPendingEvents().ToArray());
                await session.SaveChangesAsync();
                await Task.WhenAll(aggregate.GetPendingEvents().Select(_bus.PublishAsync));
                return aggregate.GetState();
            }
        }

        private async Task<QuizAggregate> CreateAggregate(IDocumentSession session, Guid quizId) 
        {
            var events = (await session.Events.FetchStreamAsync(quizId)).Select(@event => @event.Data).ToArray();
            return QuizAggregate.Create(quizId, events);
        }
    }
}