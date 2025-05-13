using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleAuthSystem.Application.Services.Interfaces;
using SimpleAuthSystem.Domain.Interfaces;
using SimpleAuthSystem.Infrastructure.AppContext;
using SimpleAuthSystem.Infrastructure.Repositories;
using SimpleAuthSystem.Infrastructure.Services.Implementations;

namespace SimpleAuthSystem.Infrastructure.Extension
{
    public static class DIServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthSystemContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                 b => b.MigrationsAssembly("SimpleAuthSystem.API")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<TokenService>();
            services.AddScoped<PasswordHarshService>();

            return services;
        }
    }
}
