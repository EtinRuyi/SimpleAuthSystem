using SimpleAuthSystem.Domain.Common;

namespace SimpleAuthSystem.Domain.Entities
{
    public class AppUser : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
