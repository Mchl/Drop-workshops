using Drop.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Drop.Api.Controllers
{
    [Route("api")]
    public class HomeController : ControllerBase
    {
        private readonly IMessenger _messenger;

        public HomeController(IMessenger messenger)
        {
            _messenger = messenger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return _messenger.GetMessage();
        }
    }
}