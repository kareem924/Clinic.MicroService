using System.Collections.Generic;
using System.Linq;

namespace Common.General.Dto.Result
{
    public class Result
    {
        private readonly List<ValidationError> _errors;
        public bool IsValid => !_errors.Any(); public object ObjectModel { get; set; }
        public IEnumerable<ValidationError> Errors => _errors;
        public Result()
        {
            _errors = new List<ValidationError>();
        }
        public Result(string errorMessage)
        {
            _errors.Add(new ValidationError(errorMessage));
        }

        public Result(string errorMessage, string errorMessageType)
        {
            _errors.Add(new ValidationError(errorMessage, errorMessageType));
        }

        public Result(params Result[] validationResults)
        {
            if (validationResults == null) return;
            foreach (var result in validationResults.Where(r => r != null))
                _errors.AddRange(result.Errors);
        }

        public Result (ValidationError error)
        {
            _errors.Add(error);
        }

    }
}
