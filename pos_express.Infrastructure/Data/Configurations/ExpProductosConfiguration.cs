using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class ExpProductosConfiguration : IEntityTypeConfiguration<ExpProductos>
    {
        public void Configure(EntityTypeBuilder<ExpProductos> builder)
        {
            builder.ToTable("ExpProductos");
            builder.HasKey(p => p.IdProducto);

            builder.Property(p => p.Nombre)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Precio)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.Activo)
                   .HasDefaultValue(true);

            builder.Property(p => p.Observaciones)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.HasOne<TiposProducto>()
                   .WithMany()
                   .HasForeignKey(p => p.IdTipoProducto)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ErpProducto)
                   .WithOne()
                   .HasForeignKey<ErpProductos>(e => e.IdProducto)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
