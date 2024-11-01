using Microsoft.AspNetCore.Mvc;
using LegoProjectApiV2.Services;
using LegoProjectApiV2.Services.Colors;
using LegoProjectApiV2.Models.DTOs.Color;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        // GET: lego_project_api/Colors
        [HttpGet]
        public async Task<ApiServiceResponse<List<ColorDTO>>> Get()
        {
            return await _colorService.GetColors();
        }

        // GET: lego_project_api/Colors/ByBrick/5
        [HttpGet("ByBrick/{brickId}")]
        public async Task<ApiServiceResponse<List<ColorDTO>>> ByBrick(string brickId)
        {
            return await _colorService.GetColorsByBrickId(brickId);
        }
    }
}
