using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class ErpProductosConfiguration : IEntityTypeConfiguration<ErpProductos>
    {
        public void Configure(EntityTypeBuilder<ErpProductos> builder)
        {
            builder.ToTable("ErpProductos");
            
            builder.HasKey(e => e.IdProducto);
            builder.Property(e => e.IdProducto)
                   .ValueGeneratedNever();

            builder.Property(e => e.Costo)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(e => e.UniqueCodigo)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.Stock)
                   .IsRequired();

            builder.Property(e => e.FechaRegistro)
                   .IsRequired();
        }
    }
}
