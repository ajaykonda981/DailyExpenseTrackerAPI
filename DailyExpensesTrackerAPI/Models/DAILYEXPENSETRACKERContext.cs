using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DailyExpensesTrackerAPI.Models
{
    public partial class DAILYEXPENSETRACKERContext : DbContext
    {
        public DAILYEXPENSETRACKERContext()
        {
        }

        public DAILYEXPENSETRACKERContext(DbContextOptions<DAILYEXPENSETRACKERContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Dailyexpenses> Dailyexpenses { get; set; }
        public virtual DbSet<Fileuploader> Fileuploader { get; set; }
        public virtual DbSet<Paymentmode> Paymentmode { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=DAILYEXPENSETRACKER;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Dailyexpenses>(entity =>
            {
                entity.ToTable("DAILYEXPENSES");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Attachments).IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpensesDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.ReasonForDeleting)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.Dailyexpenses)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FK__DAILYEXPE__Categ__286302EC");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Dailyexpenses)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__DAILYEXPE__Creat__29572725");

                entity.HasOne(d => d.PaymentModeNavigation)
                    .WithMany(p => p.Dailyexpenses)
                    .HasForeignKey(d => d.PaymentMode)
                    .HasConstraintName("FK__DAILYEXPE__Payme__2C3393D0");
            });

            modelBuilder.Entity<Fileuploader>(entity =>
            {
                entity.ToTable("FILEUPLOADER");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ActualFileName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.GeneratedFileName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.DailyExpenses)
                    .WithMany(p => p.Fileuploader)
                    .HasForeignKey(d => d.DailyExpensesId)
                    .HasConstraintName("FK__FILEUPLOA__Daily__2F10007B");
            });

            modelBuilder.Entity<Paymentmode>(entity =>
            {
                entity.ToTable("PAYMENTMODE");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PaymentMode1)
                    .HasColumnName("PaymentMode")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
