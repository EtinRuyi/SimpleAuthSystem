namespace SimpleAuthSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserDTO>> GetUserByIdAsync(string id);
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
        Task<Result<PageList<UserDTO>>> GetAllUserAsync(int pageSize, int currentPage);
    }
}
