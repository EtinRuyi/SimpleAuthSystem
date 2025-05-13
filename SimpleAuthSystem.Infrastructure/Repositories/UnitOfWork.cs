using SimpleAuthSystem.Domain.Interfaces;
using SimpleAuthSystem.Infrastructure.AppContext;

namespace SimpleAuthSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthSystemContext _context;
        public IUserRepository Users { get; }

        public UnitOfWork(AuthSystemContext context, IUserRepository userRepository)
        {
            _context = context;
            Users = userRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
