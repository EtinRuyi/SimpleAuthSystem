using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthSystem.Application.Services.Interfaces;

namespace SimpleAuthSystem.API.Controllers.V1
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            return Ok(result);

        }

        [HttpGet("allusers")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = await _userService.GetAllUserAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("users/email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmailAsync(email);
            return Ok(result);

        }
    }
}
