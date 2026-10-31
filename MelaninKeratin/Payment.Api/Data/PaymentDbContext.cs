using Microsoft.EntityFrameworkCore;
using Payment.Api.Models;

namespace Payment.Api.Data
{
    public sealed class PaymentDbContext(DbContextOptions<PaymentDbContext> options) : DbContext(options)
    {
        public DbSet<PaymentInfo> Payments => Set<PaymentInfo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var payment = modelBuilder.Entity<PaymentInfo>();

            payment.HasKey(p => p.Id);

            payment.Property(p => p.OrderId)
                .HasMaxLength(100)
                .IsRequired();

            payment.Property(p => p.CustomerId)
                .HasMaxLength(450)
                .IsRequired();

            payment.Property(p => p.Reference)
                .HasMaxLength(100)
                .IsRequired();

            payment.HasIndex(p => p.Reference)
                .IsUnique();

            payment.Property(p => p.Amount)
                .HasPrecision(18, 2);

            payment.Property(p => p.Currency)
                .HasMaxLength(3)
                .IsRequired();

            payment.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(30);
        }
    }
}
