using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class CodigoBarrasConfiguration : IEntityTypeConfiguration<CodigoBarras>
    {
        public void Configure(EntityTypeBuilder<CodigoBarras> builder)
        {
            builder.ToTable("CodigosBarras");
            builder.HasKey(c => c.IdCodigoBarra);

            builder.Property(c => c.UniqueCodigo)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(c => c.Activo)
                   .HasDefaultValue(true);

            builder.HasOne<ExpProductos>()
                   .WithMany()
                   .HasForeignKey(c => c.IdProducto)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
