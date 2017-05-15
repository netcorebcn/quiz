using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using Quiz.EventSourcing.Domain;

namespace Quiz.EventSourcing
{
    public class EventStoreRepository : IRepository
    {
        public static readonly string EventClrTypeHeader = "EventClrTypeName";
        public static readonly string AggregateClrTypeHeader = "AggregateClrTypeName";
        public static readonly string CommitIdHeader = "CommitId";
        public static readonly string ServerClockHeader = "ServerClock";        
        private const int WritePageSize = 500;
        private const int ReadPageSize = 500;

        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.None};
        private readonly IEventStoreConnection _eventStoreConnection;
        private readonly EventTypeResolver _eventTypeResolver;

        public EventStoreRepository(IEventStoreConnection eventStoreConnection, EventTypeResolver eventTypeResolver)
        {
            _eventStoreConnection = eventStoreConnection;
            _eventTypeResolver = eventTypeResolver;
        }

        public async Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : IAggregate, new()
        {
            var aggregate = new TAggregate();
            var streamName = StreamName($"{typeof(TAggregate).Name }-{id}");

            var eventNumber = 0;
            StreamEventsSlice currentSlice;
            
            do
            {
                currentSlice = await _eventStoreConnection.ReadStreamEventsForwardAsync(streamName, eventNumber, ReadPageSize, false);

                if (currentSlice.Status == SliceReadStatus.StreamNotFound)
                    throw new AggregateNotFoundException(id, typeof (TAggregate));

                if (currentSlice.Status == SliceReadStatus.StreamDeleted)
                    throw new AggregateDeletedException(id, typeof (TAggregate));

                eventNumber = (int)currentSlice.NextEventNumber;

                foreach (var resolvedEvent in currentSlice.Events)
                {
                    var payload = resolvedEvent.Event.Deserialize(_eventTypeResolver);
                    aggregate.ApplyEvent(payload);
                }

            } while (!currentSlice.IsEndOfStream);

            return aggregate;
        }

        public async Task<int> Save(IAggregate aggregate)
        {
            var streamName = StreamName($"{aggregate.GetType().Name }-{aggregate.Id}");

            var pendingEvents = aggregate.GetPendingEvents();
            var originalVersion = aggregate.Version - pendingEvents.Count;

            WriteResult result;

            var commitHeaders = CreateCommitHeaders(aggregate);
            var eventsToSave = pendingEvents.Select(x => ToEventData(Guid.NewGuid(), x, commitHeaders));

            var eventBatches = GetEventBatches(eventsToSave);

            if (eventBatches.Count == 1)
            {
                // If just one batch write them straight to the Event Store
                result = await _eventStoreConnection.AppendToStreamAsync(streamName, originalVersion, eventBatches[0]);
            }
            else
            {
                // If we have more events to save than can be done in one batch according to the WritePageSize, then we need to save them in a transaction to ensure atomicity
                using (var transaction = await _eventStoreConnection.StartTransactionAsync(streamName, originalVersion))
                {
                    foreach (var batch in eventBatches)
                    {   
                        await transaction.WriteAsync(batch);
                    }

                    result = await transaction.CommitAsync();
                }
            }

            aggregate.ClearPendingEvents();
            return (int)result.NextExpectedVersion;
        }
        
        private IList<IList<EventData>> GetEventBatches(IEnumerable<EventData> events) =>
            events.Batch(WritePageSize).Select(x => (IList<EventData>) x.ToList()).ToList();

        private static IDictionary<string, string> CreateCommitHeaders(IAggregate aggregate) =>
            new Dictionary<string, string>
            {
                {CommitIdHeader, Guid.NewGuid().ToString()},
                {AggregateClrTypeHeader, aggregate.GetType().AssemblyQualifiedName},
                {ServerClockHeader, DateTime.UtcNow.ToString("o")}
            };

        private static EventData ToEventData(Guid eventId, object evnt, IDictionary<string, string> headers)
        {
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(evnt, SerializerSettings));

            var eventHeaders = new Dictionary<string, string>(headers)
            {
                {EventClrTypeHeader, evnt.GetType().AssemblyQualifiedName}
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, SerializerSettings));
            var typeName = evnt.GetType().Name;

            return new EventData(eventId, typeName, true, data, metadata);
        }

        private string StreamName(string streamName)
        {
            var sp = streamName.Split(new [] {'-'}, 2);
            return $"{sp[0]}-{sp[1].Replace("-", "")}";
        }
    }
}