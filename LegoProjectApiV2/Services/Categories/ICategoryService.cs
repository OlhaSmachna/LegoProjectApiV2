using LegoProjectApiV2.Models.DTOs.Category;

namespace LegoProjectApiV2.Services.Categories
{
    public interface ICategoryService
    {
        public Task<ApiServiceResponse<List<CategoryDTO>>> GetCategories();
        public Task<ApiServiceResponse<CategoryDTO>> GetCategoryById(int id);
        public Task<ApiServiceResponse<CategoryDTO>> CreateCategory(NewCategoryDTO newCategoryDTO, string userEmail);
        public Task<ApiServiceResponse<bool>> UpdateCategory(CategoryDTO categoryToEdit, string userEmail);
        public Task<ApiServiceResponse<bool>> DeleteCategory(int id, string userEmail);
    }
}
