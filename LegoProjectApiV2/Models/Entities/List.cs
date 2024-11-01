using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    public class List
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //========================================================

        public User User { get; set; }
        public ICollection<PartList> Parts { get; set; }
    }
}
