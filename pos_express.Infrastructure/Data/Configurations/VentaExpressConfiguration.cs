using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class VentaExpressConfiguration : IEntityTypeConfiguration<VentaExpress>
    {
        public void Configure(EntityTypeBuilder<VentaExpress> builder)
        {
            builder.ToTable("VentaExpress");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Fecha)
                   .IsRequired();

            builder.Property(v => v.Cliente)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(v => v.Producto)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(v => v.UniqueProducto)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(v => v.Cantidad)
                   .IsRequired();

            builder.Property(v => v.Precio)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(v => v.Descuento)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(v => v.Total)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne<ExpProductos>()
                   .WithMany()
                   .HasForeignKey(v => v.IdProducto)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
