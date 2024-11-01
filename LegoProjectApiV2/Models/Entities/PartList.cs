using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    public class PartList
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string PartID { get; set; }

        [Required]
        public int ListID { get; set; }

        [Required]
        public int ColorID { get; set; }

        [Required]
        public int Quantity { get; set; }

        //================================

        public Part Part { get; set; }
        public List List { get; set; }
        public Color Color { get; set; }
    }
}
