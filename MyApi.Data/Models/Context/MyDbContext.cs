using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MyApi.Data.Models.Context
{
    public partial class MyDbContext : DbContext
    { 

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_DT");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Product_Code");

                entity.Property(e => e.ProductNameEn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Name_En");

                entity.Property(e => e.ProductNameTh)
                    .HasMaxLength(200)
                    .HasColumnName("Product_Name_Th");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_DT");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Platform)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("Register,Line,Facebook,Google");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
