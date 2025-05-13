namespace SimpleAuthSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
