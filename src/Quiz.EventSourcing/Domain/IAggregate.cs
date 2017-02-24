using System;
using System.Collections.Generic;

namespace Quiz.EventSourcing.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }

        int Version { get; }

        void ClearPendingEvents();

        void ApplyEvent(object @event);
        
        ICollection<object> GetPendingEvents();
    }
}