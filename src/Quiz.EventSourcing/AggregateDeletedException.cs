using System;

namespace Quiz.EventSourcing
{
    internal class AggregateDeletedException : Exception
    {
        private object id;
        private Type type;

        public AggregateDeletedException()
        {
        }

        public AggregateDeletedException(string message) : base(message)
        {
        }

        public AggregateDeletedException(object id, Type type)
        {
            this.id = id;
            this.type = type;
        }

        public AggregateDeletedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}