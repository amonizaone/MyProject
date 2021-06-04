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
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<User> Users { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.BarcodeNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Barcode_No");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_DT");

                entity.Property(e => e.ProductCategoryId).HasColumnName("Product_Category_Id");

                entity.Property(e => e.ProductCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Product_Code");

                entity.Property(e => e.ProductDesc)
                    .HasMaxLength(1000)
                    .HasColumnName("Product_Desc");

                entity.Property(e => e.ProductLive).HasColumnName("Product_Live");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("Product_Name");

                entity.Property(e => e.ProductPrice)
                    .HasColumnType("money")
                    .HasColumnName("Product_Price");

                entity.Property(e => e.ProductStrock).HasColumnName("Product_Strock");

                entity.Property(e => e.ProductWeight).HasColumnName("Product_Weight");

                entity.Property(e => e.UnlimitedFlag)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("Unlimited_Flag")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("Product_Categories");

                entity.Property(e => e.CategoryNameEn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Category_Name_En");

                entity.Property(e => e.CategoryNameTh)
                    .HasMaxLength(150)
                    .HasColumnName("Category_Name_Th");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_DT");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_DT");

                entity.Property(e => e.ParentCategoryId).HasColumnName("Parent_Category_Id");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("Product_Images");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Created_By");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Created_DT");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Image_Name");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_DT");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.ProductImage1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Image");

                entity.Property(e => e.ProductImageThumb)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Image_Thumb");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(200);

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

                entity.Property(e => e.FaxNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Fax_No");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Mobile_No");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Modified_By");

                entity.Property(e => e.ModifiedDt)
                    .HasColumnType("datetime")
                    .HasColumnName("Modified_DT");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Phone_No");

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
