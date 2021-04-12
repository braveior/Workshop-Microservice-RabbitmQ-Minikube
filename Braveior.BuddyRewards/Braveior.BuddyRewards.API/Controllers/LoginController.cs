using Braveior.BuddyRewards.DTO;
using Braveior.BuddyRewards.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braveior.BuddyRewards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;
        public LoginController(ILoginService service)
        {
            _service = service;
        }
        /// <summary>
        /// Endpoint for User Authentication with Email and password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO user)
        {
            return Ok(_service.Login(user));
        }
        /// <summary>
        /// Endpoint for User authentication using Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpPost("GetUserByAccessToken")]
        public async Task<IActionResult> GetUserByAccessToken([FromBody] string accessToken)
        {
            var user = await _service.GetUserFromAccessToken(accessToken);
            return Ok(user);
        }

    }

}
