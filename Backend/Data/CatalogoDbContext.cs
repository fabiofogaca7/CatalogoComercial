using CatalogoComercial.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoComercial.Api.Data;

public class CatalogoDbContext : DbContext
{
    public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
    {
    }
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Produtos)
            .WithOne(p => p.Categoria)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Categoria>()
            .Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Produto>()
            .Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Produto>()
            .Property(p => p.Preco)
            .HasColumnType("decimal(18,2)");
    }
}
