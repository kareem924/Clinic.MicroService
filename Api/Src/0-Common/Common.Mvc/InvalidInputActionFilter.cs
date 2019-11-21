using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Common.Mvc
{
    public class InvalidInputActionFilter : Attribute, IActionFilter, IResultFilter
    {
        private readonly ILogger _logger;

        public InvalidInputActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ValidationErrors");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var parameters = context.ActionDescriptor
                .Parameters
                .OfType<ControllerParameterDescriptor>()
                .TakeWhile(parameter => !parameter.ParameterInfo.HasDefaultValue)
                .ToArray();
            if (context.ActionArguments.Count < parameters.Count())
            {
                var message = "Invalid parameters count, " +
                    $"expected:{context.ActionArguments.Count()}, actual:{parameters.Count()}";
                _logger.LogError(message);
                context.Result = new BadRequestResult();
            }
            var nullableParameters = parameters
                .Join(
                    context.ActionArguments,
                    descriptor => descriptor.ParameterInfo.Name, pair => pair.Key, (descriptor, pair) => pair)
                .Where(argument => argument.Value is null);
            if (nullableParameters.Any())
            {
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestResult();
            }
            if (!context.ModelState.IsValid)
            {
                //LogValidationErrors(context.ModelState);
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is UnprocessableEntityObjectResult)
            {
                LogValidationErrors(context.ModelState);
            }
        }

        private void LogValidationErrors(ModelStateDictionary modelState)
        {
            var errors = modelState
                .ToDictionary(
                    error => error.Key,
                    error => error.Value.Errors.Select(modelError => modelError.ErrorMessage))
                .ToArray();
            var errorKeys = errors.Select(error => $"{error.Key} = {{{error.Key}}} ,");
            var errorValues = errors.Select(error => error.Value).Cast<object>().ToArray();
            _logger.LogError($"Invalid model state with the following errors {errorKeys}", errorValues);
        }
    }
}
