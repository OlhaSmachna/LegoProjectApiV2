using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    public class RefreshToken
    {
        [Key]
        public int ID { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Token { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Expires { get; set; }       
    }
}
