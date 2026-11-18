using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class TiposProductoConfiguration : IEntityTypeConfiguration<TiposProducto>
    {
        public void Configure(EntityTypeBuilder<TiposProducto> builder)
        {
            builder.ToTable("TiposProducto");
            builder.HasKey(t => t.IdTipoProducto);
            builder.Property(t => t.Descripcion)
                   .IsRequired()
                   .HasMaxLength(150);
        }
    }
}
