using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;

namespace Order.Infrastructure.Data
{
    public sealed class OrderDbContext(
    DbContextOptions<OrderDbContext> options)
    : DbContext(options)
    {
        public DbSet<Ordering> Orders => Set<Ordering>();

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ordering>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.CustomerId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(x => x.TotalAmount)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Currency)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(x => x.Status)
                    .HasConversion<string>();

                entity.HasMany(x => x.Items)
                    .WithOne()
                    .HasForeignKey("OrderingId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Navigation(x => x.Items)
                    .HasField("_items")
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property<Guid>("OrderingId")
                    .IsRequired();

                entity.Property(x => x.ProductName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.UnitPrice)
                    .HasPrecision(18, 2);
            });
        }
    }
}
