using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Materials
{
    public interface Materials_IDAL
    {
        public Task<List<Material>> GetMaterialsAsync();
    }
}
