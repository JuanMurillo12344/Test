namespace Api.Application.Usuarios;

public sealed record UsuarioResponse(Guid Id, string Nombre, string Apellido, string Email);