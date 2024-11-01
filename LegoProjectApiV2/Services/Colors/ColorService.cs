using Mapster;
using LegoProjectApiV2.Models.Entities;
using LegoProjectApiV2.DAL.Colors;
using LegoProjectApiV2.Models.DTOs.Color;


namespace LegoProjectApiV2.Services.Colors
{
    public class ColorService : IColorService
    {
        private readonly Colors_IDAL _dal;
        private readonly ApiServiceResponseProducer _responseProducer;
        public ColorService(Colors_IDAL dal)
        {
            _dal = dal;
            _responseProducer = new ApiServiceResponseProducer();
        }

        public async Task<ApiServiceResponse<List<ColorDTO>>> GetColors()
        {
            List<Color> allColors = new List<Color>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                allColors = await _dal.GetColorsAsync();
                serviceMessage = "All colors displayed.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching colors. Please try again later.";
            }
            return _responseProducer.ProduceResponse<List<ColorDTO>>(serviceMessage, allColors.Adapt<List<ColorDTO>>(), serviceException);
        }

        public async Task<ApiServiceResponse<List<ColorDTO>>> GetColorsByBrickId(string brickId)
        {
            List<Color> colors = new List<Color>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                colors = await _dal.GetColorsByBrickIdAsync(brickId);
                serviceMessage = "All colors displayed.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching colors. Please try again later.";
            }
            return _responseProducer.ProduceResponse<List<ColorDTO>>(serviceMessage, colors.Adapt<List<ColorDTO>>(), serviceException);
        }
    }
}
