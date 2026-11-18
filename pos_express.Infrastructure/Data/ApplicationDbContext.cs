using Microsoft.EntityFrameworkCore;
using pos_express.Domain.Entities;
using System.Reflection;

namespace pos_express.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TiposProducto> TiposProducto => Set<TiposProducto>();
        public DbSet<ExpProductos> ExpProductos => Set<ExpProductos>();
        public DbSet<ErpProductos> ErpProductos => Set<ErpProductos>();
        public DbSet<CodigoBarras> CodigosBarras => Set<CodigoBarras>();
        public DbSet<Categorias> Categorias => Set<Categorias>();
        public DbSet<ProductosCategorias> ProductosCategorias => Set<ProductosCategorias>();
        public DbSet<VentaExpress> VentaExpress => Set<VentaExpress>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Requisito 4: Fluent API Configurations
            // Aplica automáticamente todas las configuraciones IEntityTypeConfiguration de este proyecto
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
