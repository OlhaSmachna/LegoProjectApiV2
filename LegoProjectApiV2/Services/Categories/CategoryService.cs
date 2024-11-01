using System.ComponentModel.DataAnnotations;
using Mapster;
using LegoProjectApiV2.DAL.Categories;
using LegoProjectApiV2.Models.Entities;
using LegoProjectApiV2.Models.DTOs.Category;
using LegoProjectApiV2.Services.Users;
using LegoProjectApiV2.Tools;
using LegoProjectApiV2.Exceptions;

namespace LegoProjectApiV2.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly Categories_IDAL _dal;
        private readonly IUserService _userService;
        private readonly ApiServiceResponseProducer _responseProducer;
        public CategoryService(Categories_IDAL dal, IUserService userService)
        {
            _dal = dal;
            _responseProducer = new ApiServiceResponseProducer();
            _userService = userService;
        }

        public async Task<ApiServiceResponse<List<CategoryDTO>>> GetCategories()
        {
            List<Category> allCategories = new List<Category>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                allCategories = await _dal.GetCategoriesAsync();
                serviceMessage = "All categories displayed.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching categories. Please try again later.";
            }
            return _responseProducer.ProduceResponse<List<CategoryDTO>>(serviceMessage, allCategories.Adapt<List<CategoryDTO>>(), serviceException);
        }

        public async Task<ApiServiceResponse<CategoryDTO>> GetCategoryById(int id)
        {
            Category categoryById = null;
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                categoryById = await _dal.CategoryById(id);
                if(categoryById.ID != 0)
                    serviceMessage = $"Category with #ID: {categoryById.ID} was found";
                else
                    serviceMessage = $"Category with #ID: {id} wasn`t found";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = $"An error occured while searching for category #{id}. Please try again later.";
            }
            return _responseProducer.ProduceResponse(serviceMessage, categoryById.Adapt<CategoryDTO>(), serviceException);
        }

        public async Task<ApiServiceResponse<CategoryDTO>> CreateCategory(NewCategoryDTO newCategoryDTO, string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            if (await _userService.CheckAdminRole(userEmail))
            {
                Category newCategory = newCategoryDTO.Adapt<Category>();
                Category categoryFromDB = null;
                if (ModelValidator.Validate(newCategory))
                {
                    try
                    {
                        categoryFromDB = await _dal.CategoryByName(newCategory.Name);
                        if (categoryFromDB.ID == 0)
                        {
                            categoryFromDB = await _dal.CategoryAddAsync(newCategory);
                            serviceMessage = $"New category added successfully.";
                        }
                        else
                        {
                            categoryFromDB = new Category();
                            serviceMessage = $"Category already exists.";
                            serviceException = new Exception(serviceMessage);
                        }

                    }
                    catch (Exception ex)
                    {
                        serviceException = ex;
                        serviceMessage = "Can`t save changes. Please try again later.";
                    }
                }
                else
                {
                    serviceMessage = "This category data is incorrect.";
                    serviceException = new ValidationException("ModelState is invalid.");
                }
                return _responseProducer.ProduceResponse(serviceMessage, categoryFromDB.Adapt<CategoryDTO>(), serviceException);
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
                return _responseProducer.ProduceResponse(serviceMessage, new CategoryDTO(), serviceException);
            }
        }

        public async Task<ApiServiceResponse<bool>> UpdateCategory(CategoryDTO categoryToEdit, string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            bool saveResult = false;
            if (await _userService.CheckAdminRole(userEmail))
            {
                Category categoryToUpdate = categoryToEdit.Adapt<Category>();
                Category categoryFromDB = null;
                if (ModelValidator.Validate(categoryToUpdate))
                {
                    categoryFromDB = await _dal.CategoryByName(categoryToUpdate.Name);
                    try
                    {
                        if (categoryFromDB.ID == 0)
                        {
                            saveResult = await _dal.CategoryUpdateAsync(categoryToUpdate);
                            serviceMessage = $"Changes saved successfully.";
                        }
                        else
                        {
                            serviceMessage = $"Category already exists.";
                            serviceException = new Exception(serviceMessage);
                        } 
                    }
                    catch (Exception ex)
                    {
                        serviceException = ex;
                        serviceMessage = "Cant save changes. Please try again later.";
                    }
                }
                else
                {
                    serviceMessage = "This brick data is incorrect.";
                    serviceException = new ValidationException("ModelState is invalid.");
                }
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
            }
            return _responseProducer.ProduceResponse(serviceMessage, saveResult, serviceException);
        }

        public async Task<ApiServiceResponse<bool>> DeleteCategory(int id, string userEmail)
        {
            Exception serviceException = null;
            string serviceMessage;
            bool deleteResult = false;
            if (await _userService.CheckAdminRole(userEmail))
            {
                try
                {
                    deleteResult = await _dal.CategoryDeleteAsync(id);
                    serviceMessage = $"Category with #ID: {id} was deleted";
                }
                catch (Exception ex)
                {
                    serviceException = ex;
                    serviceMessage = "Cant save changes. Please try again later.";
                }
            }
            else
            {
                serviceMessage = "Only admins can execute this task.";
                serviceException = new InvalidAccessLevelException("User role check failed.");
            }
            return _responseProducer.ProduceResponse(serviceMessage, deleteResult, serviceException);
        }
    }
}
