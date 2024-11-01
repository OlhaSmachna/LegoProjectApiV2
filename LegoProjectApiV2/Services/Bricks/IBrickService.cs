using LegoProjectApiV2.Models.DTOs.Brick;

namespace LegoProjectApiV2.Services.Bricks
{
    public interface IBrickService
    {
        public Task<ApiServiceResponse<List<BrickDTO>>> GetBricks();
        public Task<ApiServiceResponse<List<BrickDTO>>> GetBricksByCategoryId(int categoryId);
        public Task<ApiServiceResponse<List<BrickDTO>>> GetBricksByListId(int listId, string userEmail);
        public Task<ApiServiceResponse<List<BrickDTO>>> SearchBricks(string pattern);
        public Task<ApiServiceResponse<BrickDTO>> GetBrickById(string id);
        public Task<ApiServiceResponse<BrickExtendedDTO>> GetBrickDetailsById(string id);
        public Task<ApiServiceResponse<BrickDTO>> CreateBrick(BrickExtendedDTO brickDTO, string userEmail);
        public Task<ApiServiceResponse<BrickDTO>> UpdateBrick(BrickExtendedDTO brickDTO, string userEmail);
        public Task<ApiServiceResponse<bool>> DeleteBrick(string id, string userEmail);
        
        public void FillCloud();
        public void FillDB();
    }
}
