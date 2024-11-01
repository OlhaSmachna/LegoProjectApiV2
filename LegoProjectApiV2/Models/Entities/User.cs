using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int ID { get; set; }

        public int RoleID { get; set; } = 1;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        [RegularExpression("[A-Za-z0-9._%-]+@[A-Za-z0-9._%-]+\\.[a-z]{2,3}")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public byte[] PasswordHash { get; set; }

        public int? RefreshTokenID { get; set; }

        //==========================================

        public Role Role { get; set; }

        public ICollection<List> Lists { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
