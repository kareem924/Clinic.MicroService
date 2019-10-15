using System.Collections.Generic;

namespace Common.General.Dto.Result
{
    public class Result
    {
        public bool Success { get; }
        public IEnumerable<string> Messages { get; }
        public IEnumerable<Error> Errors { get; }

        protected Result(bool success = false, IEnumerable<string> message = null, IEnumerable<Error> errors = null)
        {
            Success = success;
            Messages = message;
            Errors = errors;
        }

    }
}
