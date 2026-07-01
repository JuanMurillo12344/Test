using Api.Domain.Usuarios;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Usuarios.Create;

public sealed record CreateUsuarioCommand(string Nombre, string Apellido, string Email);

public sealed record CreateUsuarioResponse(Guid Id, string Nombre, string Apellido, string Email);

public interface ICreateUsuarioCommandHandler
{
    Task<CreateUsuarioResponse> HandleAsync(CreateUsuarioCommand command, CancellationToken cancellationToken = default);
}

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

public sealed class CreateUsuarioCommandHandler(IUsuarioRepository usuarioRepository) : ICreateUsuarioCommandHandler
{
    public async Task<CreateUsuarioResponse> HandleAsync(CreateUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        CreateUsuarioValidator.Validate(command);

        var usuario = new Usuario(Guid.NewGuid(), command.Nombre, command.Apellido, command.Email);

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return usuario.ToResponse();
    }
}