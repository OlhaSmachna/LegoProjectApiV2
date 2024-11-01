using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegoProjectApiV2.Services;
using LegoProjectApiV2.Services.Categories;
using LegoProjectApiV2.Models.DTOs.Category;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : LegoProjectController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: lego_project_api/Categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<List<CategoryDTO>>> Get()
        {
            return await _categoryService.GetCategories();
        }

        // GET lego_project_api/Categories/5
        [HttpGet("{id}")]
        public async Task<ApiServiceResponse<CategoryDTO>> Get(int id)
        {
            return await _categoryService.GetCategoryById(id);
        }

        // POST lego_project_api/Categories
        [HttpPost]
        public async Task<ApiServiceResponse<CategoryDTO>> Post([FromBody] NewCategoryDTO newCategoryDTO)
        {
            return await _categoryService.CreateCategory(newCategoryDTO, EmailFromClaims());
        }

        // PUT lego_project_api/Categories/5
        [HttpPut("{id}")]
        public async Task<ApiServiceResponse<bool>> Put([FromBody] CategoryDTO categoryToEdit)
        {
            return await _categoryService.UpdateCategory(categoryToEdit, EmailFromClaims());
        }

        // DELETE lego_project_api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ApiServiceResponse<bool>> Delete(int id)
        {
            return await _categoryService.DeleteCategory(id, EmailFromClaims());
        }
    }
}
