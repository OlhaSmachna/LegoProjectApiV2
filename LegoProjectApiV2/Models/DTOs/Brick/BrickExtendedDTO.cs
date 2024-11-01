namespace LegoProjectApiV2.Models.DTOs.Brick
{
    public class BrickExtendedDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool HasImage { get; set; }
        public int ImageVersion { get; set; }
        public int CategoryID { get; set; }
        public int MaterialID { get; set; }
        public List<int> ColorIDs { get; set; }
    }
}
