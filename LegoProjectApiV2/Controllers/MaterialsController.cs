using Microsoft.AspNetCore.Mvc;
using LegoProjectApiV2.Services;
using LegoProjectApiV2.Services.Materials;
using LegoProjectApiV2.Models.DTOs.Material;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        // GET: lego_project_api/Materials
        [HttpGet]
        public async Task<ApiServiceResponse<List<MaterialDTO>>> Get()
        {
            return await _materialService.GetMaterials();
        }
    }
}
