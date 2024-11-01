using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Parts
{
    public interface Parts_IDAL
    {
        public Task<List<Part>> GetPartsAsync();
        public Task<List<PartList>> GetPartsByListIdAsync(int listId, int userId);
        public Task<List<Part>> GetPartsByCategoryIdAsync(int categoryId);
        public Task<List<Part>> SearchPartsAsync(string pattern);
        public Task<Part> PartById(string id);
        public Task<Part> PartAddAsync(Part part);
        public Task<bool> PartUpdateAsync(Part part);
        public Task<bool> PartDeleteAsync(string id);
        public void SetHasImgTrue(string id);
    }
}
