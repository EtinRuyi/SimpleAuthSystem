namespace SimpleAuthSystem.Infrastructure.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;
        private readonly PasswordHarshService _passwordService;
        private readonly ILogger<AuthService> _logger;
        

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, 
            TokenService tokenService, PasswordHarshService passwordService, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation("Registering user with username: {Username}", registerDto.Username);
                var uniqueEmail = await _unitOfWork.Users.IsEmailUniqueAsync(registerDto.Email);
                if (!uniqueEmail)
                {
                    return Result<AuthResponseDto>.Failure("Email is already taken");
                }

                var uniqueUsername = await _unitOfWork.Users.IsUsernameUniqueAsync(registerDto.Username);
                if (!uniqueUsername)
                {
                    return Result<AuthResponseDto>.Failure("Username is already taken");
                }

                var user = _mapper.Map<AppUser>(registerDto);
                user.Id = Guid.NewGuid().ToString();
                user.PasswordHash = _passwordService.HashPassword(registerDto.Password);
                user.CreatedAt = DateTime.UtcNow;
                user.LastLogin = DateTime.UtcNow;

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                var token = _tokenService.GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDTO>(user);


                _logger.LogInformation("User registered successfully: {Username}", registerDto.Username);
                return Result<AuthResponseDto>.Success(new AuthResponseDto {User = userDto, Token = token}, "User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user: {Username}", registerDto.Username);
                throw new BadRequestException("Registration failed: " + ex.Message);
            }
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login failed: Invalid email {Email}", loginDto.Email);
                    return Result<AuthResponseDto>.Failure("Invalid email or password");
                }

                if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Login failed: Invalid password for email {Email}", loginDto.Email);
                    return Result<AuthResponseDto>.Failure("Invalid username or password");
                }

                user.LastLogin = DateTime.UtcNow;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                var token = _tokenService.GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDTO>(user);

                _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
                return Result<AuthResponseDto>.Success(new AuthResponseDto
                {
                    Token = token,
                    User = userDto
                }, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user: {Email}", loginDto.Email);
                throw new UnauthorizedException("Login failed: " + ex.Message);
            }
        }

        public async Task<Result<bool>> ValidateTokenAsync(string token)
        {
            try
            {
                bool isValid = _tokenService.ValidateToken(token);
                if (isValid)
                {
                    return Result<bool>.Success(true, "Token is valid");
                }
                else
                {
                    return Result<bool>.Failure("Invalid token");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating token: {Message}", ex.Message);
                return Result<bool>.Failure("Token validation failed: " + ex.Message);
            }
        }
    }
}
