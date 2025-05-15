namespace SimpleAuthSystem.Domain.Interfaces
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser> GetByUsernameAsync(string username);
        Task<AppUser> GetByEmailAsync(string email);
        Task<AppUser> GetByIdAsync(string Id);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUsernameUniqueAsync(string username);
    }
}
