﻿/*using System;
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

        public virtual DbSet<Nutrients> Nutrients { get; set; } = null!;
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

                entity.Property(e => e.IsHydrophobic).HasColumnName("is_hydrophobic");

                entity.Property(e => e.IsThermophobic).HasColumnName("is_thermophobic");

                entity.Property(e => e.NutrientName)
                    .HasMaxLength(50)
                    .HasColumnName("nutrient_name");

                entity.Property(e => e.NutrientNameUa)
                    .HasMaxLength(50)
                    .HasColumnName("nutrient_name_ua");
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