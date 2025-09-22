using Microservice.PSE.Api.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Microservice.PSE.Api.Entities
{
    public partial class MainContext : DbContext
    {
        public MainContext()
        {
        }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = true;
        }

        public virtual DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasIndex(e => e.BankCode)
                    .HasDatabaseName("UK_Banks_BankCode")
                    .IsUnique();

                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_Banks_IsActive");

                entity.HasIndex(e => e.BankName)
                    .HasDatabaseName("IX_Banks_BankName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}