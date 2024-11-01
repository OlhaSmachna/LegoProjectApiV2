using LegoProjectApiV2.Models.DTOs.User;

namespace LegoProjectApiV2.Services.Users
{
    public interface IUserService
    {
        public Task<ApiServiceResponse<List<UserDTO>>> GetUsers();
        public Task<ApiServiceResponse<UserDTO>> GetUserById(int id);
        public Task<UserDTOExtended> GetUserByEmail(string userEmail);
        public Task<ApiServiceResponse<AddResponceUserDTO>> CreateUser(AddRequestUserDTO addRequestUserDTO);
        public Task<ApiServiceResponse<bool>> UpdateUser(EditRequestUserDTO editRequestUserDTO);
        public Task<ApiServiceResponse<bool>> DeleteUser(int id);
        public Task<ApiServiceResponse<LoginResponseUserDTO>> Login(LoginRequestUserDTO loginRequestUserDTO);
        public Task<ApiServiceResponse<LoginResponseUserDTO>> RefreshToken(RefreshTokenRequestUserDTO refreshTokenRequestUserDTO);
        public Task<bool> CheckAdminRole(string userEmail);
    }
}
