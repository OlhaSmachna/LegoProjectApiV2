using Microsoft.EntityFrameworkCore;
using LegoProjectApiV2.DBCntx;
using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Colors
{
    public class Colors_DAL : Colors_IDAL
    {
        private readonly LegoProjectDB db;
        public Colors_DAL(LegoProjectDB context)
        {
            db = context;
        }
        public async Task<List<Color>> GetColorsAsync()
        {
            List<Color> allColors = await db.Colors.ToListAsync();
            return allColors;
        }

        public async Task<List<Color>> GetColorsByBrickIdAsync(string brickId)
        {
            List<Color> allColors = await db.Colors
                    .Include(c => c.Parts)
                    .Where(c => c.Parts.Where(pc => pc.PartID == brickId).Any())
                    .ToListAsync();
            return allColors;
        }
    }
}
