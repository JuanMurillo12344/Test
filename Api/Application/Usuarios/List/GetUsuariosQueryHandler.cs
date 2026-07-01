using Api.Domain.Usuarios;

namespace Api.Application.Usuarios.List;

public interface IGetUsuariosQueryHandler
{
    Task<IReadOnlyList<UsuarioResponse>> HandleAsync(GetUsuariosQuery query, CancellationToken cancellationToken = default);
}

public sealed class GetUsuariosQueryHandler(IUsuarioRepository usuarioRepository) : IGetUsuariosQueryHandler
{
    public async Task<IReadOnlyList<UsuarioResponse>> HandleAsync(GetUsuariosQuery query, CancellationToken cancellationToken = default)
    {
        var usuarios = await usuarioRepository.GetAllAsync(cancellationToken);

        return usuarios
            .Select(usuario => new UsuarioResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email))
            .ToArray();
    }
}