using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using LegoProjectApiV2.Models.DTOs.Cloudinary;

namespace LegoProjectApi.Controllers
{
    [Route("lego_project_api/[controller]")]
    [ApiController]
    public class CloudinaryController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private string Cloudinary_name;
        private string Cloudinary_key;
        private string Cloudinary_secret;
        private string Cloudinary_URL;
        public CloudinaryController(IConfiguration configuration)
        {
            Configuration = configuration;
            Cloudinary_name = Environment.GetEnvironmentVariable("CLOUD_NAME") ?? String.Empty;
            Cloudinary_key = Environment.GetEnvironmentVariable("CLOUD_KEY") ?? String.Empty;
            Cloudinary_secret = Environment.GetEnvironmentVariable("CLOUD_SECRET") ?? String.Empty;
            Cloudinary_URL = "cloudinary://" + Cloudinary_key + ":" + Cloudinary_secret + "@" + Cloudinary_name;
        }

        [HttpPost]
        public SignatureDTO Sign([FromBody] CloudinaryDTO cloudinaryDto)
        {
            Cloudinary cloudinary = new Cloudinary(Cloudinary_URL);
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("folder", cloudinaryDto.folder);
            parameters.Add("public_id", cloudinaryDto.public_id);
            parameters.Add("timestamp", Timestamp.ToString()); 
            parameters.Add("upload_preset", cloudinaryDto.upload_preset);
            string signature = cloudinary.Api.SignParameters(parameters);
            return new SignatureDTO(signature, Timestamp.ToString());
        }
    }
}
