/*
  using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AngularApp2.Models.Entity
{
    public partial class DiplomusContext : DbContext
    {
        public DiplomusContext()
        {
        }

        public DiplomusContext(DbContextOptions<DiplomusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Needs> Needs { get; set; } = null!;
        public virtual DbSet<NeedsNutrients> NeedsNutrients { get; set; } = null!;
        public virtual DbSet<Nutrients> Nutrients { get; set; } = null!;
        public virtual DbSet<Products> Products { get; set; } = null!;
        public virtual DbSet<ProductsNutrients> ProductsNutrients { get; set; } = null!;
        public virtual DbSet<Recipes> Recipes { get; set; } = null!;
        public virtual DbSet<RecipesProducts> RecipesProducts { get; set; } = null!;
        public virtual DbSet<Users> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=diplomus;Username=postgres;Password=qwerty123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Needs>(entity =>
            {
                entity.ToTable("needs");

                entity.Property(e => e.NeedsId).HasColumnName("needs_id");

                entity.Property(e => e.Desc).HasColumnName("desc");

                entity.Property(e => e.ExcludedProducts).HasColumnName("excluded_products");

                entity.Property(e => e.NeedsName)
                    .HasMaxLength(100)
                    .HasColumnName("needs_name");
            });

            modelBuilder.Entity<NeedsNutrients>(entity =>
            {
                entity.HasKey(e => e.NeedNutrientId)
                    .HasName("needs_nutrients_pkey");

                entity.ToTable("needs_nutrients");

                entity.HasIndex(e => e.NeedId, "fki_needs_nutrients_fk1");

                entity.HasIndex(e => e.NutrientId, "fki_needs_nutrients_fk2");

                entity.Property(e => e.NeedNutrientId).HasColumnName("need_nutrient_id");

                entity.Property(e => e.NeedId).HasColumnName("need_id");

                entity.Property(e => e.NutrientId).HasColumnName("nutrient_id");

                entity.Property(e => e.NutrientPercent).HasColumnName("nutrient_percent");

                entity.HasOne(d => d.Need)
                    .WithMany(p => p.NeedsNutrients)
                    .HasForeignKey(d => d.NeedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("needs_nutrients_fk1");

                entity.HasOne(d => d.Nutrient)
                    .WithMany(p => p.NeedsNutrients)
                    .HasForeignKey(d => d.NutrientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("needs_nutrients_fk2");
            });

            modelBuilder.Entity<Nutrients>(entity =>
            {
                entity.HasKey(e => e.NutrientId)
                    .HasName("Nutrients_pkey");

                entity.ToTable("nutrients");

                entity.Property(e => e.NutrientId)
                    .HasColumnName("nutrient_id")
                    .HasDefaultValueSql("nextval('\"Nutrients_Nutrient_id_seq\"'::regclass)");

                entity.Property(e => e.DailyDose).HasColumnName("daily_dose");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DoseMeasure)
                    .HasMaxLength(10)
                    .HasColumnName("dose_measure");

                entity.Property(e => e.IsHydrophobic).HasColumnName("is_hydrophobic");

                entity.Property(e => e.IsThermophobic).HasColumnName("is_thermophobic");

                entity.Property(e => e.NutrientName)
                    .HasMaxLength(50)
                    .HasColumnName("nutrient_name");

                entity.Property(e => e.NutrientNameUa)
                    .HasMaxLength(50)
                    .HasColumnName("nutrient_name_ua");
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("Products_pkey");

                entity.ToTable("products");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasDefaultValueSql("nextval('\"Products_Product_id_seq\"'::regclass)");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .HasColumnName("category");

                entity.Property(e => e.MeasureAmount1).HasColumnName("measure_amount1");

                entity.Property(e => e.MeasureAmount2).HasColumnName("measure_amount2");

                entity.Property(e => e.MeasureName1)
                    .HasMaxLength(100)
                    .HasColumnName("measure_name1");

                entity.Property(e => e.MeasureName2)
                    .HasMaxLength(100)
                    .HasColumnName("measure_name2");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .HasColumnName("product_name");
            });

            modelBuilder.Entity<ProductsNutrients>(entity =>
            {
                entity.HasKey(e => e.ProductNutrientId)
                    .HasName("Products_Nutrients_pkey");

                entity.ToTable("products_nutrients");

                entity.HasIndex(e => e.ProductId, "fki_Products_Nutrients_fk1");

                entity.HasIndex(e => e.NutrientId, "fki_Products_Nutrients_fk2");

                entity.Property(e => e.ProductNutrientId)
                    .HasColumnName("product_nutrient_id")
                    .HasDefaultValueSql("nextval('\"Products_Nutrients_Prod_Nutr_id_seq\"'::regclass)");

                entity.Property(e => e.NutrientAmount).HasColumnName("nutrient_amount");

                entity.Property(e => e.NutrientId).HasColumnName("nutrient_id");

                entity.Property(e => e.NutrientPercent).HasColumnName("nutrient_percent");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Nutrient)
                    .WithMany(p => p.ProductsNutrients)
                    .HasForeignKey(d => d.NutrientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Products_Nutrients_fk2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductsNutrients)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Products_Nutrients_fk1");
            });

            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.HasKey(e => e.RecipeId)
                    .HasName("recipes_pkey");

                entity.ToTable("recipes");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.Property(e => e.Ingredients).HasColumnName("ingredients");

                entity.Property(e => e.RecipeName)
                    .HasMaxLength(200)
                    .HasColumnName("recipe_name");
                entity.Property(e => e.RecipeImg).HasColumnName("recipe_img");

                entity.Property(e => e.Steps).HasColumnName("steps");
            });

            modelBuilder.Entity<RecipesProducts>(entity =>
            {
                entity.HasKey(e => e.RecipeProductId)
                    .HasName("recipes_products_pkey");

                entity.ToTable("recipes_products");

                entity.HasIndex(e => e.ProductId, "fki_Recipes_products_fk");

                entity.HasIndex(e => e.RecipeId, "fki_Recipes_products_fk1");

                entity.Property(e => e.RecipeProductId).HasColumnName("recipe_product_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.MeasureNumber).HasColumnName("measure_number");

                entity.Property(e => e.MeasureType)
                    .HasColumnType("char")
                    .HasColumnName("measure_type");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.RecipesProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipes_products_fk");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.RecipesProducts)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Recipes_products_fk1");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("users_pkey");

                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.DayOfBirth).HasColumnName("day_of_birth");

                entity.Property(e => e.Email)
                    .HasMaxLength(30)
                    .HasColumnName("email");

                entity.Property(e => e.Needs).HasColumnName("needs");

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(10)
                    .HasColumnName("role");

                entity.Property(e => e.UserImg).HasColumnName("user_img");

                entity.Property(e => e.UserName)
                    .HasMaxLength(30)
                    .HasColumnName("user_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

 */