using System;
using System.Collections.Generic;
using System.Linq;
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
            var role = "admin";
            var jwt = _jwtHandler.CreateToken(userId, role, claims: new Dictionary<string, IEnumerable<string>>
            {
                ["permissions"] = new[] {"secret:read", "secret:update"}
            });

            return jwt;
        }

        [Authorize(Policy = "secret.read")]
        [HttpGet("secret")]
        public ActionResult<string> Secret()
        {
            return User.Identity.Name;
        }
    }
}