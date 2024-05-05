using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Visonex.Idmg.ExternalWebApi.Services;

namespace Visonex.Idmg.ExternalWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserValidationService _userValidationService;

        public UserController(IUserValidationService userValidationService)
        {
            _userValidationService = userValidationService;
        }

        [HttpGet("{userEmail}")]
        public async Task<IActionResult> ValidateUser(string userEmail)
        {
            var userExists = await _userValidationService.ValidateUserExistsAsync(userEmail);

            if (userExists)
            {
                return Ok($"User with email '{userEmail}' exists.");
            }
            else
            {
                return NotFound($"User with email '{userEmail}' not found.");
            }
        }
    }
}
