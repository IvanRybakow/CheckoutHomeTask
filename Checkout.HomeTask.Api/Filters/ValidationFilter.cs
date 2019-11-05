using Checkout.HomeTask.Api.Contracts.v1.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.HomeTask.Api.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger logger;
        public ValidationFilter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("Checkout.HomeTask.Api.Filters.ValidationFilter");
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Where(ms => ms.Value.Errors.Count() > 0)
                    .SelectMany(kvp => kvp.Value.Errors
                    .Select(e => new ErrorModel { ErrorName = kvp.Key, Message = e.ErrorMessage }))
                    .ToList();
                var errorResponse = new ErrorResponse { Errors = errors };
                context.Result = new BadRequestObjectResult(errorResponse);
                logger.LogInformation("Incoming parameters were incorrect");
                return;
            }
            await next();
        }
    }
}
