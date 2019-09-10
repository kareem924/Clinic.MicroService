using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.General.Exceptions
{
    [Serializable]
    public class EventHandlerNotFoundException : Exception
    {
        public EventHandlerNotFoundException()
        {

        }

        public EventHandlerNotFoundException(string message) : base(message)
        {

        }

        public EventHandlerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected EventHandlerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
