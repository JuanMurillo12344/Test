using Api.Domain.Usuarios;
using MediatR;
using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Usuarios.Create;

public sealed record CreateUsuarioCommand(string Nombre, string Apellido, string Email) : IRequest<CreateUsuarioResponse>;

public sealed record CreateUsuarioResponse(Guid Id, string Nombre, string Apellido, string Email);

public static class CreateUsuarioValidator
{
    public static void Validate(CreateUsuarioCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

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

public static class CreateUsuarioMapping
{
    public static CreateUsuarioResponse ToResponse(this Usuario usuario)
    {
        return new CreateUsuarioResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email);
    }
}

public sealed class CreateUsuarioCommandHandler(ApiDbContext dbContext) : IRequestHandler<CreateUsuarioCommand, CreateUsuarioResponse>
{
    public async Task<CreateUsuarioResponse> Handle(CreateUsuarioCommand command, CancellationToken cancellationToken)
    {
        CreateUsuarioValidator.Validate(command);

        var usuario = new Usuario(Guid.NewGuid(), command.Nombre, command.Apellido, command.Email);

        await dbContext.Usuarios.AddAsync(usuario, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return usuario.ToResponse();
    }
}