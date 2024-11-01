using Microsoft.EntityFrameworkCore;
using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Parts
{
    public class Parts_DAL : Parts_IDAL
    {
        private readonly LegoProjectDB db;
        public Parts_DAL(LegoProjectDB context)
        {
            db = context;
        }
        public async Task<List<Part>> GetPartsAsync()
        {
            List<Part> allParts = await db.Parts
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Material)
                    .Include(p => p.Colors)
                    .ThenInclude(pc => pc.Color)
                    .ToListAsync<Part>();
            return allParts;
        }

        public async Task<List<PartList>> GetPartsByListIdAsync(int listId, int userId)
        {
            List<PartList> partLists = await db.PartList
                    .Include(pl => pl.Part)
                    .ThenInclude(p => p.Material)
                    .Include(pl => pl.List)
                    .Include(pl => pl.Color)
                    .Where(pl => pl.ListID == listId && pl.List.UserID == userId)
                    .ToListAsync();
            return partLists;
        }

        public async Task<List<Part>> GetPartsByCategoryIdAsync(int categoryId)
        {
            List<Part> allParts = await db.Parts
                    .AsNoTracking()
                    .Include(p => p.Category)
                    .Include(p => p.Material)
                    .Include(p => p.Colors)
                    .ThenInclude(pc => pc.Color)
                    .Where(p => p.CategoryID == categoryId)
                    .ToListAsync<Part>();
            return allParts;
        }

        public async Task<List<Part>> SearchPartsAsync(string pattern)
        {
            List<Part> allParts = await db.Parts
                    .Include(p => p.Category)
                    .Include(p => p.Material)
                    .Include(p => p.Colors)
                    .ThenInclude(pc => pc.Color)
                    .Where(p => p.ID.Contains(pattern) || p.Name.Contains(pattern))
                    .ToListAsync<Part>();
            return allParts;
        }

        public Task<Part> PartById(string id)
        {
            Part partByID = new Part();
            var searchResults = db.Parts
                    .Include(p => p.Material)
                    .Include(p => p.Colors)
                    .ThenInclude(pc => pc.Color)
                    .Where(p => p.ID == id);
            if (searchResults.Any()) partByID = searchResults.First();
            return Task.FromResult(partByID);
        }

        public async Task<Part> PartAddAsync(Part part)
        {
            await db.Parts.AddAsync(part);
            await db.SaveChangesAsync();
            return part;
        }

        public async Task<bool> PartUpdateAsync(Part part)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.Parts.Include(p => p.Colors).Where(p => p.ID == part.ID);
            if (searchResults.Any())
            {
                Part partFromDB = searchResults.First();

                partFromDB.Name = part.Name;
                partFromDB.HasImage = part.HasImage;
                partFromDB.ImageVersion = part.ImageVersion;
                partFromDB.MaterialID = part.MaterialID;
                partFromDB.CategoryID = part.CategoryID;
                partFromDB.Colors = part.Colors;
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }

        public async Task<bool> PartDeleteAsync(string id)
        {
            int numberOfDeletedRecords = 0;
            var searchResults = db.Parts.Include(p => p.Colors).Where(p => p.ID == id);
            if (searchResults.Any())
            {
                Part partFromDB = searchResults.First();
                db.Parts.Remove(partFromDB);
                numberOfDeletedRecords = await db.SaveChangesAsync();
            }
            return numberOfDeletedRecords > 0;
        }

        public void SetHasImgTrue(string id)
        {
            var searchResults = db.Parts.Where(p => p.ID == id);
            if (searchResults.Any())
            {
                Part partFromDB = searchResults.First();
                partFromDB.HasImage = true;
                db.SaveChanges();
            }
        }
    }
}
