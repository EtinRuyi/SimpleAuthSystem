using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthSystem.Application.ApiResponse;
using SimpleAuthSystem.Application.DTOs.RequestDTOs;
using SimpleAuthSystem.Application.DTOs.ResponseDTOs;
using SimpleAuthSystem.Application.Services.Interfaces;

namespace SimpleAuthSystem.API.Controllers.V1
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("validatetoken")]
        [Authorize]
        public async Task<ActionResult<bool>> ValidateToken()
        {
            _logger.LogInformation("Token validation successful for user: {User}", User.Identity.Name);
            return Ok(Result<bool>.Success(true, "Token is valid"));
        }
    }
}
