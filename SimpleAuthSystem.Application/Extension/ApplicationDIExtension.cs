namespace SimpleAuthSystem.Application.Extension
{
    public static class ApplicationDIExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ApplicationDIExtension).Assembly);
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.AddValidatorsFromAssemblyContaining<LoginValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

            return services;
        }
    }
}
