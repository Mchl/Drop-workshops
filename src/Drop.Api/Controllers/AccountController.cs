using System;
using Convey.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Drop.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IJwtHandler _jwtHandler;

        public AccountController(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        [HttpPost("sign-in")]
        public ActionResult<JsonWebToken> SignIn()
        {
            var userId = Guid.NewGuid().ToString();
            var jwt = _jwtHandler.CreateToken(userId);

            return jwt;
        }

        [Authorize]
        [HttpGet("secret")]
        public ActionResult<string> Secret()
        {
            return User.Identity.Name;
        }
    }
}