using LegoProjectApiV2.Models.DTOs.Material;

namespace LegoProjectApiV2.Services.Materials
{
    public interface IMaterialService
    {
        public Task<ApiServiceResponse<List<MaterialDTO>>> GetMaterials();
    }
}
