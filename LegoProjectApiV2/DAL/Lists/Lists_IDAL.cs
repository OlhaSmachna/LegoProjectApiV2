using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Lists
{
    public interface Lists_IDAL
    {
        public Task<List<List>> GetListsByUserIdAsync(int userId);
        public Task<List> ListById(int id, int userId);
        public Task<List> ListAddAsync(List list);
        public Task<bool> ListUpdateAsync(List list, int userId);
        public Task<bool> ListDeleteAsync(int id, int userId);
        public Task<bool> ListAddPartAsync(PartList partList, int userId);
        public Task<bool> ListEditPartAsync(PartList partList, int userId);
        public Task<bool> ListDeletePartAsync(PartList partList, int userId);
    }
}
