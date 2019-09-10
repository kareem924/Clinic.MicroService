using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.General.Exceptions
{
    [Serializable]
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException()
        {

        }

        public ValidationErrorException(string message) : base(message)
        {

        }

        public ValidationErrorException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected ValidationErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
