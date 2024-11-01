using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LegoProjectApiV2.DAL.Users
{
    public class Users_DAL : Users_IDAL
    {
        private readonly LegoProjectDB db;
        public Users_DAL(LegoProjectDB context)
        {
            db = context;
        }
        public async Task<List<User>> GetUsersAsync()
        {
            List<User> allUsers = await db.Users.ToListAsync();
            return allUsers;
        }

        public Task<User> UserById(int id)
        {
            User userByID = new User();
            var searchResults = db.Users.Include(u => u.Role).Where(u => u.ID == id);
            if (searchResults.Any()) userByID = searchResults.First();
            return Task.FromResult(userByID);
        }

        public Task<User> UserByEmailAsync(string email)
        {
            User userByEmail = new User();
            var searchResults = db.Users
                    .Include(u => u.Role)
                    .Include(u => u.RefreshToken)
                    .Where(u => u.Email == email);
            if (searchResults.Any())
                userByEmail = searchResults.First();
            return Task.FromResult(userByEmail);
        }

        public async Task<User> UserAddAsync(User user)
        {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserUpdateAsync(User user)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.Users.Where(u => u.ID == user.ID);
            if (searchResults.Any())
            {
                User userFromDB = searchResults.First();
                userFromDB.PasswordHash = user.PasswordHash;
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }

        public async Task<bool> UserDeleteAsync(int id)
        {
            int numberOfDeletedRecords = 0;
            var searchResults = db.Users.Where(u => u.ID == id);
            if (searchResults.Any())
            {
                User userFromDB = searchResults.First();
                db.Users.Remove(userFromDB);
                numberOfDeletedRecords = await db.SaveChangesAsync();
            }
            return numberOfDeletedRecords > 0;
        }

        public async Task<bool> RefreshTokenAsync(User user)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.Users.Where(u => u.ID == user.ID);
            if (searchResults.Any())
            {
                User userFromDB = searchResults.First();
                userFromDB.RefreshToken = user.RefreshToken;
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }
    }
}
