using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Users
{
    public interface Users_IDAL
    {
        public Task<List<User>> GetUsersAsync();
        public Task<User> UserById(int id);
        public Task<User> UserByEmailAsync(string email);
        public Task<User> UserAddAsync(User user);
        public Task<bool> UserUpdateAsync(User user);
        public Task<bool> UserDeleteAsync(int id);
        public Task<bool> RefreshTokenAsync(User user);
    }
}
