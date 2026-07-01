using Api.Domain.Usuarios;

namespace Api.Application.Usuarios.Create;

public interface ICreateUsuarioCommandHandler
{
    Task<UsuarioResponse> HandleAsync(CreateUsuarioCommand command, CancellationToken cancellationToken = default);
}

public sealed class CreateUsuarioCommandHandler(IUsuarioRepository usuarioRepository) : ICreateUsuarioCommandHandler
{
    public async Task<UsuarioResponse> HandleAsync(CreateUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var usuario = new Usuario(Guid.NewGuid(), command.Nombre, command.Apellido, command.Email);

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return new UsuarioResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email);
    }
}