using SimpleAuthSystem.Domain.Entities;

namespace SimpleAuthSystem.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByUsernameAsync(string username);
        Task<AppUser> GetByEmailAsync(string email);
        Task<AppUser> GetByIdAsync(string id);
        Task AddAsync(AppUser user);
    }
}
