using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Name { get; set; }

        //==========================================

        public ICollection<Part> Parts { get; set; }
    }
}
