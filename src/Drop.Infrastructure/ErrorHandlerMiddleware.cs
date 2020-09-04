using System;
using System.Threading.Tasks;
using Drop.Application.Exceptions;
using Drop.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Drop.Infrastructure
{
    internal class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                _logger.LogError(ex, ex.Message);
                switch (ex)
                {
                    case AppException appException:
                        await HandleExceptionAsync(context, appException.Code, appException.Message);
                        return;
                    case DomainException domainException:
                        await HandleExceptionAsync(context, domainException.Code, domainException.Message);
                        return;
                    default:
                        throw;
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string code, string message)
            => context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                code,
                message
            }));
    }
}