using System.ComponentModel.DataAnnotations;

namespace SimpleAuthSystem.Application.DTOs.RequestDTOs
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
