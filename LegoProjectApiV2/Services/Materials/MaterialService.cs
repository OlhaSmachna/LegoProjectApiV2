using Mapster;
using LegoProjectApiV2.Models.Entities;
using LegoProjectApiV2.Models.DTOs.Material;
using LegoProjectApiV2.DAL.Materials;

namespace LegoProjectApiV2.Services.Materials
{
    public class MaterialService : IMaterialService
    {
        private readonly Materials_IDAL _dal;
        private readonly ApiServiceResponseProducer _responseProducer;
        public MaterialService(Materials_IDAL dal)
        {
            _dal = dal;
            _responseProducer = new ApiServiceResponseProducer();
        }

        public async Task<ApiServiceResponse<List<MaterialDTO>>> GetMaterials()
        {
            List<Material> allMaterials = new List<Material>();
            Exception serviceException = null;
            string serviceMessage;
            try
            {
                allMaterials = await _dal.GetMaterialsAsync();
                serviceMessage = "All materials displayed.";
            }
            catch (Exception ex)
            {
                serviceException = ex;
                serviceMessage = "Something went wrong while fetching materials. Please try again later.";
            }
            return _responseProducer.ProduceResponse<List<MaterialDTO>>(serviceMessage, allMaterials.Adapt<List<MaterialDTO>>(), serviceException);
        }
    }
}
