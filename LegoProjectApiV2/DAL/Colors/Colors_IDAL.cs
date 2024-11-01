using LegoProjectApiV2.Models.Entities;

namespace LegoProjectApiV2.DAL.Colors
{
    public interface Colors_IDAL
    {
        public Task<List<Color>> GetColorsAsync();
        public Task<List<Color>> GetColorsByBrickIdAsync(string brickId);
    }
}
