using LegoProjectApiV2.Models.DTOs.Color;

namespace LegoProjectApiV2.Models.DTOs.Brick
{
    public class BrickDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool HasImage { get; set; }
        public int ImageVersion { get; set; } = 0;
        public string Material { get; set; }
        public List<ColorDTO> Colors { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
