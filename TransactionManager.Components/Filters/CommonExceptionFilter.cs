using TransactionManager.Components.Errors;
using TransactionManager.Components.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace TransactionManager.Components.Filters
{
    public class CommonExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CommonExceptionFilter> _logger;

        public CommonExceptionFilter(ILogger<CommonExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainException)
            {
                var errorModel = new DomainError(
                    domainException.Message,
                    domainException.Errors);

                context.Result = new BadRequestObjectResult(errorModel);

                _logger.LogError(domainException.ToString());
            }
            else
            {
                var errorModel = new DomainError(context.Exception.Message);

                context.Result = new ObjectResult(errorModel)
                {
                    StatusCode = 500
                };

                _logger.LogCritical(context.Exception, context.Exception.Message);
            }            
        }
    }
}
