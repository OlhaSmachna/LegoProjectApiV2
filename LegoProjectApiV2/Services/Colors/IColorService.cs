using LegoProjectApiV2.Models.DTOs.Color;

namespace LegoProjectApiV2.Services.Colors
{
    public interface IColorService
    {
        public Task<ApiServiceResponse<List<ColorDTO>>> GetColors();
        public Task<ApiServiceResponse<List<ColorDTO>>> GetColorsByBrickId(string brickId);
    }
}
