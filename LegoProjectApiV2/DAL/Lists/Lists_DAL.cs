using Microsoft.EntityFrameworkCore;
using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Lists
{
    public class Lists_DAL : Lists_IDAL
    {
        private readonly LegoProjectDB db;
        public Lists_DAL(LegoProjectDB context)
        {
            db = context;
        }
        public async Task<List<List>> GetListsByUserIdAsync(int userId)
        {
            List<List> listsByUserId = await db.Lists
                    .Include(l => l.Parts)
                    .Where(l => l.UserID == userId)
                    .ToListAsync();
            return listsByUserId;
        }

        public Task<List> ListById(int id, int userId)
        {
            List listByID = new List();
            var searchResults = db.Lists.Where(l => l.ID == id && l.UserID == userId);
            if (searchResults.Any()) listByID = searchResults.First();
            return Task.FromResult(listByID);
        }

        public async Task<List> ListAddAsync(List list)
        {
            await db.Lists.AddAsync(list);
            await db.SaveChangesAsync();
            return list;
        }

        public async Task<bool> ListUpdateAsync(List list, int userId)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.Lists.Where(l => l.ID == list.ID && l.UserID == userId);
            if (searchResults.Any())
            {
                List listFromDB = searchResults.First();
                listFromDB.Name = list.Name;
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }

        public async Task<bool> ListDeleteAsync(int id, int userId)
        {
            int numberOfDeletedRecords = 0;
            var searchResults = db.Lists.Where(l => l.ID == id && l.UserID == userId);
            if (searchResults.Any())
            {
                List listFromDB = searchResults.First();
                db.Lists.Remove(listFromDB);
                numberOfDeletedRecords = await db.SaveChangesAsync();
            }
            return numberOfDeletedRecords > 0;
        }

        public async Task<bool> ListAddPartAsync(PartList partList, int userId)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.Lists
                    .Include(l => l.Parts)
                    .Where(l => l.ID == partList.ListID && l.UserID == userId);
            if (searchResults.Any())
            {
                List listFromDB = searchResults.First();
                var searchResultsInner = listFromDB.Parts
                    .Where(p => p.PartID == partList.PartID && p.ColorID == partList.ColorID);
                if (searchResultsInner.Any())
                {
                    PartList partListInner = searchResultsInner.First();
                    partListInner.Quantity += partList.Quantity;
                }
                else
                {
                    listFromDB.Parts.Add(partList);
                }
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }

        public async Task<bool> ListEditPartAsync(PartList partList, int userId)
        {
            int numberOfUpdatedRecords = 0;
            var searchResults = db.PartList
                    .Include(pl => pl.List)
                    .Where(pl =>
                    pl.List.UserID == userId
                    && pl.PartID == partList.PartID
                    && pl.ColorID == partList.ColorID
                    );
            if (searchResults.Any())
            {
                PartList partListFromDB = searchResults.First();
                partListFromDB.Quantity = partList.Quantity;
                numberOfUpdatedRecords = await db.SaveChangesAsync();
            }
            return numberOfUpdatedRecords > 0;
        }

        public async Task<bool> ListDeletePartAsync(PartList partList, int userId)
        {
            int numberOfDeletedRecords = 0;
            var searchResults = db.PartList
                    .Include(pl => pl.List)
                    .Where(pl =>
                    pl.List.UserID == userId
                    && pl.PartID == partList.PartID
                    && pl.ColorID == partList.ColorID
                    );
            if (searchResults.Any())
            {
                PartList partListFromDB = searchResults.First();
                db.PartList.Remove(partListFromDB);
                numberOfDeletedRecords = await db.SaveChangesAsync();
            }
            return numberOfDeletedRecords > 0;
        }
    }
}
