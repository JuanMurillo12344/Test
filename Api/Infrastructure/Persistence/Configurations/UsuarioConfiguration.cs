using Api.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(usuario => usuario.Apellido)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(usuario => usuario.Email)
            .IsRequired()
            .HasMaxLength(150);
    }
}