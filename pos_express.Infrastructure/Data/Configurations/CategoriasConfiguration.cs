using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pos_express.Domain.Entities;

namespace pos_express.Infrastructure.Data.Configurations
{
    public class CategoriasConfiguration : IEntityTypeConfiguration<Categorias>
    {
        public void Configure(EntityTypeBuilder<Categorias> builder)
        {
            builder.ToTable("Categorias");
            builder.HasKey(c => c.IdCategoria);

            builder.Property(c => c.Descripcion)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Activo)
                   .HasDefaultValue(true);

            builder.HasOne<Categorias>()
                   .WithMany()
                   .HasForeignKey(c => c.IdCategoriaPadre)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
        }
    }
}
