using Microsoft.EntityFrameworkCore;
using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Categories
{
    public class Categories_DAL : Categories_IDAL
    {
        private readonly LegoProjectDB db;
        public Categories_DAL(LegoProjectDB context)
        {
            db = context;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            List<Category> allCategories = await db.Categories.ToListAsync();
            return allCategories;
        }

        public Task<Category> CategoryById(int id)
        {
            Category categoryByID = new Category();
            var searchResults = db.Categories.Where(c => c.ID == id);
            if (searchResults.Any()) categoryByID = searchResults.First();
            return Task.FromResult(categoryByID);
        }

        public Task<Category> CategoryByName(string name)
        {
            Category categoryByID = new Category();
            var searchResults = db.Categories.Where(c => c.Name == name);
            if (searchResults.Any()) categoryByID = searchResults.First();
            return Task.FromResult(categoryByID);
        }

        public async Task<Category> CategoryAddAsync(Category category)
        {
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> CategoryUpdateAsync(Category category)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.Categories.Where(c => c.ID == category.ID);
            if (searchResults.Any())
            {
                Category categoryFromDB = searchResults.First();
                categoryFromDB.Name = category.Name;
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }

        public async Task<bool> CategoryDeleteAsync(int id)
        {
            int numberOfDeletedRecords = 0;
            var searchResults = db.Categories.Where(c => c.ID == id);
            if (searchResults.Any())
            {
                Category categoryFromDB = searchResults.First();
                db.Categories.Remove(categoryFromDB);
                numberOfDeletedRecords = await db.SaveChangesAsync();
            }
            return numberOfDeletedRecords > 0;
        }
    }
}
