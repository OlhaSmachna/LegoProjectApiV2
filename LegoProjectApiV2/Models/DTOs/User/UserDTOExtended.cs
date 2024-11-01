using LegoProjectApiV2.Models.DTOs.Role;

namespace LegoProjectApiV2.Models.DTOs.User
{
    public class UserDTOExtended
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public RoleDTO Role { get; set; }
    }
}
