namespace LegoProjectApiV2.Models.DTOs.Cloudinary
{
    public class SignatureDTO
    {
        public SignatureDTO(string signature, string timestamp)
        {
            this.signature = signature;
            this.timestamp = timestamp;
        }

        public string signature { get; set; }
        public string timestamp { get; set; }
    }
}
