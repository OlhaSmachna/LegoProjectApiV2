using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LegoProjectApiV2.Services;
using LegoProjectApiV2.Services.Bricks;
using LegoProjectApiV2.Models.DTOs.Brick;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    [Authorize]
    public class BricksController : LegoProjectController
    {
        private readonly IBrickService _brickService;

        public BricksController(IBrickService brickService)
        {
            _brickService = brickService;
        }

        // GET: lego_project_api/Bricks
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<List<BrickDTO>>> Get()
        {
            return await _brickService.GetBricks();
        }

        // GET: lego_project_api/Bricks/ByCategory/5
        [HttpGet("ByCategory/{categoryId}")]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<List<BrickDTO>>> ByCategory(int categoryId)
        {
            return await _brickService.GetBricksByCategoryId(categoryId);
        }

        // GET: lego_project_api/Bricks/ByList/5
        [HttpGet("ByList/{listId}")]
        public async Task<ApiServiceResponse<List<BrickDTO>>> ByList(int listId)
        {
            return await _brickService.GetBricksByListId(listId, EmailFromClaims());
        }

        // GET lego_project_api/Bricks/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<BrickDTO>> Get(string id)
        {
            return await _brickService.GetBrickById(id);
        }

        // GET lego_project_api/Bricks/Details/5
        [HttpGet("Details/{id}")]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<BrickExtendedDTO>> GetDetails(string id)
        {
            return await _brickService.GetBrickDetailsById(id);
        }

        // POST lego_project_api/Bricks
        [HttpPost]
        public async Task<ApiServiceResponse<BrickDTO>> Post([FromBody] BrickExtendedDTO brickDTO)
        {
            return await _brickService.CreateBrick(brickDTO, EmailFromClaims());
        }

        // POST lego_project_api/Bricks/Search
        [HttpPost("Search")]
        [AllowAnonymous]
        public async Task<ApiServiceResponse<List<BrickDTO>>> Search([FromBody] SearchDTO searchDTO)
        {
            return await _brickService.SearchBricks(searchDTO.Pattern);
        }

        // PUT lego_project_api/Bricks/5
        [HttpPut("{id}")]
        public async Task<ApiServiceResponse<BrickDTO>> Put([FromBody] BrickExtendedDTO brickToEdit)
        {
            return await _brickService.UpdateBrick(brickToEdit, EmailFromClaims());
        }

        // DELETE lego_project_api/Bricks/5
        [HttpDelete("{id}")]
        public async Task<ApiServiceResponse<bool>> Delete(string id)
        {
            return await _brickService.DeleteBrick(id, EmailFromClaims());
        }
    }
}
