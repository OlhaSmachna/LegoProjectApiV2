using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    public class Part
    {
        [Key]
        [Required(AllowEmptyStrings = false)]
        [MaxLength(450)]
        public string ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required]
        public bool HasImage { get; set; }

        public int ImageVersion { get; set; }

        public int MaterialID { get; set; }

        public int? CategoryID { get; set; }

        //==========================================

        public Material Material { get; set; }

        public Category Category { get; set; }

        public ICollection<PartColor> Colors { get; set; }
        public ICollection<PartList> Lists { get; set; }
    }
}
