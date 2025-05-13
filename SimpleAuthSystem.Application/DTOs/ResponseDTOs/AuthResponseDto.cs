namespace SimpleAuthSystem.Application.DTOs.ResponseDTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDTO User { get; set; }
    }
}
