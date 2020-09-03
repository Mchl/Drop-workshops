using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drop.Api.Services
{
    public interface IMessenger
    {
        string GetMessage();
    }

    public class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly IOptions<ApiOptions> _apiOptions;
        private readonly ILogger<Messenger> _logger;

        public Messenger(IOptions<ApiOptions> apiOptions, ILogger<Messenger> logger)
        {
            _apiOptions = apiOptions;
            _logger = logger;
        }

        public string GetMessage()
        {
            _logger.LogInformation("Invoking get message");
            return $"{_apiOptions.Value.Name} [id: {_id}]";
        }
    }
}