namespace LegoProjectApiV2.Models.DTOs.User
{
    public class RefreshTokenRequestUserDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
