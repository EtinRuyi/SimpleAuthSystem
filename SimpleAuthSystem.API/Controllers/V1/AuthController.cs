namespace SimpleAuthSystem.API.Controllers.V1
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;


        public AuthController(IAuthService authService, 
            ILogger<AuthController> logger, IUserService userService)
        {
            _authService = authService;
            _logger = logger;
            _userService = userService;
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

        [HttpGet("currentuser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("No user ID found in token");
                    return BadRequest(Result<UserDTO>.Failure("Invalid token, No user ID found in token"));
                }

                var result = await _userService.GetUserByIdAsync(userId);
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("User not found for ID: {UserId}", userId);
                    return NotFound(result);
                }

                _logger.LogInformation("Successfully retrieved current user: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current user");
                return BadRequest(Result<UserDTO>.Failure("Error retrieving current user: " + ex.Message));
            }
        }
    }
}
