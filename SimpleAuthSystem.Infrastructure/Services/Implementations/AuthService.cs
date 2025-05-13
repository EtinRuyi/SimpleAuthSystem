using AutoMapper;
using Microsoft.Extensions.Logging;
using SimpleAuthSystem.Application.ApiResponse;
using SimpleAuthSystem.Application.DTOs;
using SimpleAuthSystem.Application.DTOs.RequestDTOs;
using SimpleAuthSystem.Application.DTOs.ResponseDTOs;
using SimpleAuthSystem.Application.Services.Interfaces;
using SimpleAuthSystem.Domain.Entities;
using SimpleAuthSystem.Domain.Interfaces;

namespace SimpleAuthSystem.Infrastructure.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly TokenService _tokenService;
        private readonly PasswordHarshService _passwordService;
        private readonly ILogger<AuthService> _logger;
        

        public AuthService(IUserRepository userRepository, IMapper mapper, 
            TokenService tokenService, PasswordHarshService passwordService, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if email is already taken
                if (!await _userRepository.IsEmailUniqueAsync(registerDto.Email))
                {
                    return Result<AuthResponseDto>.Failure("Email is already taken");
                }

                // Check if username is already taken
                if (!await _userRepository.IsUsernameUniqueAsync(registerDto.Username))
                {
                    return Result<AuthResponseDto>.Failure("Username is already taken");
                }

                // Create new user
                var user = _mapper.Map<AppUser>(registerDto);
                user.Id = Guid.NewGuid();
                user.PasswordHash = _passwordService.HashPassword(registerDto.Password);
                user.CreatedAt = DateTime.UtcNow;

                await _userRepository.AddAsync (user);

                // Generate JWT token
                var token = _tokenService.GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDTO>(user);

                return Result<AuthResponseDto>.Success(new AuthResponseDto
                {
                    Token = token,
                    User = userDto
                }, "User registered successfully");
            }
            catch (Exception ex)
            {
                // Log exception
                return Result<AuthResponseDto>.Failure("Registration failed: " + ex.Message);
            }
        }

        public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by username
                var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
                if (user == null)
                {
                    return Result<AuthResponseDto>.Failure("Invalid username or password");
                }

                // Verify password
                if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return Result<AuthResponseDto>.Failure("Invalid username or password");
                }

                // Update last login time
                user.LastLogin = DateTime.UtcNow;
                // UpdateAsync now automatically saves changes
                await _userRepository.UpdateAsync(user);

                // Generate JWT token
                var token = _tokenService.GenerateJwtToken(user);
                var userDto = _mapper.Map<AppUserDto>(user);

                return Result<AuthResponseDto>.Success(new AuthResponseDto
                {
                    Token = token,
                    User = userDto
                }, "Login successful");
            }
            catch (Exception ex)
            {
                // Log exception
                return Result<AuthResponseDto>.Failure("Login failed: " + ex.Message);
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
                return Result<bool>.Failure("Token validation failed: " + ex.Message);
            }
        }
    }
}
