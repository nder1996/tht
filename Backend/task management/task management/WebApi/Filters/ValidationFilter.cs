using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using task_management.Application.Service;

namespace task_management.WebApi.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                var response = ResponseApiBuilderService.Failure<object>(
                    errorCode: "VALIDATION_ERROR",
                    errorDescription: string.Join(", ", errors),
                    statusCode: 400
                );

                context.Result = new ObjectResult(response)
                {
                    StatusCode = 400
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
