using System.Threading.Tasks;
using Drop.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Drop.Api
{
    public class DummyMiddleware : IMiddleware
    {
        private readonly IMessenger _messenger;
        private readonly ILogger<DummyMiddleware> _logger;

        public DummyMiddleware(IMessenger messenger, ILogger<DummyMiddleware> logger)
        {
            _messenger = messenger;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _logger.LogInformation("I'm the dummy middleware.");
            await next(context);
        }
    }
}