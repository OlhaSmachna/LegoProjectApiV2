using Microsoft.EntityFrameworkCore;
using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Materials
{
    public class Materials_DAL : Materials_IDAL
    {
        private readonly LegoProjectDB db;
        public Materials_DAL(LegoProjectDB context)
        {
            db = context;
        }
        public async Task<List<Material>> GetMaterialsAsync()
        {
            List<Material> allMaterials = await db.Materials.ToListAsync();
            return allMaterials;
        }
    }
}
