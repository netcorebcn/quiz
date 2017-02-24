using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quiz.EventSourcing.Domain
{
    public interface IRepository
    {
        Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : IAggregate, new();

        Task<int> Save(IAggregate aggregate, params KeyValuePair<string, string>[] extraHeaders);
    }
}