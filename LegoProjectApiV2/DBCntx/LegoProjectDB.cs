using LegoProjectApiV2.Models.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace LegoProjectApiV2.DBCntx
{
    public class LegoProjectDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<PartList> PartList { get; set; }

        public LegoProjectDB (DbContextOptions<LegoProjectDB> options): base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            builder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();
            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
            builder.Entity<Material>()
                .HasIndex(m => m.Name)
                .IsUnique();
            builder.Entity<Color>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<PartColor>()
                .HasOne(pc => pc.Part)
                .WithMany(p => p.Colors)
                .HasForeignKey(pc => pc.PartID);
            builder.Entity<PartColor>()
                .HasOne(pc => pc.Color)
                .WithMany(c => c.Parts)
                .HasForeignKey(pc => pc.ColorID);

            builder.Entity<PartList>()
                .HasOne(pl => pl.Part)
                .WithMany(p => p.Lists)
                .HasForeignKey(pl => pl.PartID);
            builder.Entity<PartList>()
                .HasOne(pl => pl.List)
                .WithMany(l => l.Parts)
                .HasForeignKey(pl => pl.ListID);
        }
    }
}
