using Microsoft.EntityFrameworkCore;
using OnlineSalesSystem.Core.Entities;

namespace OnlineSalesSystem.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração das entidades
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            
            // Configurações adicionais do Customer
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Email).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");
            
            // Configurações adicionais do Order
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderDate).IsRequired();
            entity.Property(o => o.Total)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            // Relacionamento
            entity.HasOne(o => o.Customer)
                  .WithMany(c => c.Orders)
                  .HasForeignKey(o => o.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict); // ou Cascade, conforme sua regra de negócio
        });

    }
}