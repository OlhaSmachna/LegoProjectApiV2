using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegoProjectApiV2.Services;
using LegoProjectApiV2.Services.Lists;
using LegoProjectApiV2.Models.DTOs.List;
using LegoProjectApiV2.Models.DTOs.Brick;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListsController : LegoProjectController
    {
        private readonly IListService _listService;

        public ListsController(IListService listService)
        {
            _listService = listService;
        }

        // GET: lego_project_api/Lists
        [HttpGet]
        public async Task<ApiServiceResponse<List<ListDTO>>> Get()
        {
            return await _listService.GetListsByUserEmail(EmailFromClaims());
        }

        // GET lego_project_api/Lists/5
        [HttpGet("{id}")]
        public async Task<ApiServiceResponse<ListDTO>> Get(int id)
        {
            return await _listService.GetListById(id, EmailFromClaims());
        }

        // POST lego_project_api/Lists
        [HttpPost]
        public async Task<ApiServiceResponse<ListDTO>> Post([FromBody] NewListDTO newListDTO)
        {
            return await _listService.CreateList(newListDTO, EmailFromClaims());
        }

        // POST lego_project_api/Lists/AddBrickTo/5
        [HttpPost("AddBrickTo/{id}")]
        public async Task<ApiServiceResponse<bool>> AddBrick(int id, [FromBody] BrickListDTO brick)
        {
            return await _listService.AddBrick(id, brick, EmailFromClaims());
        }

        // POST lego_project_api/Lists/AddBricksTo/5
        [HttpPost("AddBricksTo/{id}")]
        public async Task<ApiServiceResponse<int>> AddBricks(int id, [FromBody] List<BrickListDTO> bricks)
        {
            return await _listService.AddBricks(id, bricks, EmailFromClaims());
        }

        // PUT lego_project_api/Lists/5
        [HttpPut("{id}")]
        public async Task<ApiServiceResponse<bool>> Put([FromBody] ListDTO listToEdit)
        {
            return await _listService.UpdateList(listToEdit, EmailFromClaims());
        }

        // POST lego_project_api/Lists/EditIn/5
        [HttpPost("EditIn/{id}")]
        public async Task<ApiServiceResponse<bool>> EditBrickInList(int id, [FromBody] BrickListDTO brick)
        {
            return await _listService.EditBrickInList(id, brick, EmailFromClaims());
        }

        // DELETE lego_project_api/Lists/5
        [HttpDelete("{id}")]
        public async Task<ApiServiceResponse<bool>> Delete(int id)
        {
            return await _listService.DeleteList(id, EmailFromClaims());
        }

        // POST lego_project_api/Lists/DeleteFrom/5
        [HttpPost("DeleteFrom/{id}")]
        public async Task<ApiServiceResponse<bool>> DeleteBrickFromList(int id, [FromBody] BrickListDeleteDTO brick)
        {
            return await _listService.DeleteBrickFromList(id, brick, EmailFromClaims());
        }
    }
}
