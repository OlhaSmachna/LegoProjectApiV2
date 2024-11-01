using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Categories
{
    public interface Categories_IDAL
    {
        public Task<List<Category>> GetCategoriesAsync();
        public Task<Category> CategoryById(int id);
        public Task<Category> CategoryByName(string name);
        public Task<Category> CategoryAddAsync(Category category);
        public Task<bool> CategoryUpdateAsync(Category category);
        public Task<bool> CategoryDeleteAsync(int id);
    }
}
