using Api.Domain.Usuarios;

namespace Api.Application.Usuarios.List;

public sealed record GetUsuariosQuery;

public sealed record UsuarioListItemResponse(Guid Id, string Nombre, string Apellido, string Email);

public interface IGetUsuariosQueryHandler
{
    Task<IReadOnlyList<UsuarioListItemResponse>> HandleAsync(GetUsuariosQuery query, CancellationToken cancellationToken = default);
}

public static class GetUsuariosMapping
{
    public static UsuarioListItemResponse ToResponse(this Usuario usuario)
    {
        return new UsuarioListItemResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email);
    }
}

public sealed class GetUsuariosQueryHandler(IUsuarioRepository usuarioRepository) : IGetUsuariosQueryHandler
{
    public async Task<IReadOnlyList<UsuarioListItemResponse>> HandleAsync(GetUsuariosQuery query, CancellationToken cancellationToken = default)
    {
        var usuarios = await usuarioRepository.GetAllAsync(cancellationToken);

        return usuarios
            .Select(usuario => usuario.ToResponse())
            .ToArray();
    }
}