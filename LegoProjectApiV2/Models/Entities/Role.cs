﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LegoProjectApiV2.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Role
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //=============================================

        public ICollection<User> Users { get; set; }
    }
}
