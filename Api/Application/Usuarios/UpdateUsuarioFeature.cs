using Api.Domain.Usuarios;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Usuarios.Update;

public sealed record UpdateUsuarioCommand(Guid Id, string Nombre, string Apellido, string Email) : IRequest<UpdateUsuarioResponse?>;

public sealed record UpdateUsuarioResponse(Guid Id, string Nombre, string Apellido, string Email);

public static class UpdateUsuarioValidator
{
    public static void Validate(UpdateUsuarioCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.Id == Guid.Empty)
        {
            throw new ValidationException("El id del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(command.Nombre))
        {
            throw new ValidationException("El nombre del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(command.Apellido))
        {
            throw new ValidationException("El apellido del usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            throw new ValidationException("El email del usuario es obligatorio.");
        }
    }
}

public static class UpdateUsuarioMapping
{
    public static UpdateUsuarioResponse ToResponse(this Usuario usuario)
    {
        return new UpdateUsuarioResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email);
    }
}

public sealed class UpdateUsuarioCommandHandler(ApiDbContext dbContext) : IRequestHandler<UpdateUsuarioCommand, UpdateUsuarioResponse?>
{
    public async Task<UpdateUsuarioResponse?> Handle(UpdateUsuarioCommand command, CancellationToken cancellationToken)
    {
        UpdateUsuarioValidator.Validate(command);

        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == command.Id, cancellationToken);

        if (usuario is null)
        {
            return null;
        }

        usuario.Update(command.Nombre, command.Apellido, command.Email);

        await dbContext.SaveChangesAsync(cancellationToken);

        return usuario.ToResponse();
    }
}