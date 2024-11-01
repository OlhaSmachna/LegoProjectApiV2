using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    public class PartColor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string PartID { get; set; }

        [Required]
        public int ColorID { get; set; }


        //=================================

        public Part Part { get; set; }
        public Color Color { get; set; }
    }
}
