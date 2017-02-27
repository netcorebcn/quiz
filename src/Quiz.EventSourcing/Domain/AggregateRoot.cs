using System;
using System.Collections.Generic;

namespace Quiz.EventSourcing.Domain
{
    public abstract class AggregateRoot : IAggregate
    {
        private readonly List<object> _pendingEvents = new List<object>();

        public Guid Id { get; protected set; }
        public int Version { get; private set; } = -1;

        public AggregateRoot()
        {
            Id = Guid.NewGuid();
        }

        void IAggregate.ApplyEvent(object @event)
        {
            ((dynamic) this).Apply((dynamic) @event);
            Version++;
        }

        public ICollection<object> GetPendingEvents() => _pendingEvents;

        public void ClearPendingEvents()  => _pendingEvents.Clear();

        protected void RaiseEvent(object @event)
        {
            ((IAggregate) this).ApplyEvent(@event);
            _pendingEvents.Add(@event);
        }
    }
}