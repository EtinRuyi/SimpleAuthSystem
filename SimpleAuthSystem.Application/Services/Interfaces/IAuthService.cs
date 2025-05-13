using SimpleAuthSystem.Application.ApiResponse;
using SimpleAuthSystem.Application.DTOs.RequestDTOs;
using SimpleAuthSystem.Application.DTOs.ResponseDTOs;

namespace SimpleAuthSystem.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto);
        Task<Result<bool>> ValidateTokenAsync(string token);
    }
}
