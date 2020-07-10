using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tracking.Api.Helpers;
using Tracking.Common.Models;
using Tracking.Services.Interfaces;

namespace Tracking.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(ILogger<UsersController> logger, IOptions<AppSettings> appSettings, IUserService userService)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthenticateRequest model)
        {
            try
            {
                var response = await _userService.Authenticate(model, _appSettings.Secret);

                if (response == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                return Ok(response);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogTrace(e.StackTrace);
            }

            return BadRequest(new { message = "Error processing your request!" });
        }
    }
}