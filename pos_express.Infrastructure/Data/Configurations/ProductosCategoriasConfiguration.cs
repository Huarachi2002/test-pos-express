using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class ProductosCategoriasConfiguration : IEntityTypeConfiguration<ProductosCategorias>
    {
        public void Configure(EntityTypeBuilder<ProductosCategorias> builder)
        {
            builder.ToTable("ProductosCategorias");
            builder.HasKey(pc => pc.IdDetalle);

            builder.Property(pc => pc.FechaCreacion)
                   .IsRequired();

            builder.HasOne<ExpProductos>()
                   .WithMany()
                   .HasForeignKey(pc => pc.IdProducto)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Categorias>()
                   .WithMany()
                   .HasForeignKey(pc => pc.IdCategoria)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
