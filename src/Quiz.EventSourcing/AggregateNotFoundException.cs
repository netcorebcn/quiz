using System;

namespace Quiz.EventSourcing
{
    internal class AggregateNotFoundException : Exception
    {
        private object id;
        private Type type;

        public AggregateNotFoundException()
        {
        }

        public AggregateNotFoundException(string message) : base(message)
        {
        }

        public AggregateNotFoundException(object id, Type type)
        {
            this.id = id;
            this.type = type;
        }

        public AggregateNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}