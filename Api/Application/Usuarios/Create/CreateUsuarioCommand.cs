namespace Api.Application.Usuarios.Create;

public sealed record CreateUsuarioCommand(string Nombre, string Apellido, string Email);