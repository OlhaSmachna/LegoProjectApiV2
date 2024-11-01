using LegoProjectApiV2.Models.DTOs.Role;

namespace LegoProjectApiV2.Models.DTOs.User
{
    public class UserDTO
    {
        public string Email { get; set; }
        public RoleDTO Role { get; set; }
    }
}
