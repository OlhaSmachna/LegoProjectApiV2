using LegoProjectApiV2.Models.DTOs.Brick;
using LegoProjectApiV2.Models.DTOs.List;


namespace LegoProjectApiV2.Services.Lists
{
    public interface IListService
    {
        public Task<ApiServiceResponse<List<ListDTO>>> GetListsByUserEmail(string userEmail);
        public Task<ApiServiceResponse<ListDTO>> GetListById(int id, string userEmail);
        public Task<ApiServiceResponse<ListDTO>> CreateList(NewListDTO newList, string userEmail);
        public Task<ApiServiceResponse<bool>> UpdateList(ListDTO listToEdit, string userEmail);
        public Task<ApiServiceResponse<bool>> DeleteList(int id, string userEmail);

        public Task<ApiServiceResponse<bool>> AddBrick(int id, BrickListDTO brick, string userEmail);
        public Task<ApiServiceResponse<int>> AddBricks(int id, List<BrickListDTO> bricks, string userEmail);
        public Task<ApiServiceResponse<bool>> EditBrickInList(int id, BrickListDTO brick, string userEmail);
        public Task<ApiServiceResponse<bool>> DeleteBrickFromList(int id, BrickListDeleteDTO brick, string userEmail);
    }
}
