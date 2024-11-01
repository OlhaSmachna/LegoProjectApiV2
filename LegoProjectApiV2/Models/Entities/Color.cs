using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Color
    {

        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(6)]
        public string RGB { get; set; }

        [Required]
        public bool IsTrans { get; set; }

        //==========================================

        public ICollection<PartColor> Parts { get; set; }
    }
}
