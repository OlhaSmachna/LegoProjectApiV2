using LegoProjectApiV2.Models.DTOs.Role;

namespace LegoProjectApiV2.Models.DTOs.User
{
    public class LoginResponseUserDTO
    {
        public string Email {get; set;}
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public RoleDTO Role { get; set; }
    }
}
