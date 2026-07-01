using Api.Domain.Usuarios;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Usuarios.GetById;

public sealed record GetUsuarioByIdQuery(Guid Id) : IRequest<UsuarioDetailResponse?>;

public sealed record UsuarioDetailResponse(Guid Id, string Nombre, string Apellido, string Email);

public static class GetUsuarioByIdValidator
{
    public static void Validate(GetUsuarioByIdQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.Id == Guid.Empty)
        {
            throw new ValidationException("El id del usuario es obligatorio.");
        }
    }
}

public static class GetUsuarioByIdMapping
{
    public static UsuarioDetailResponse ToResponse(this Usuario usuario)
    {
        return new UsuarioDetailResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email);
    }
}

public sealed class GetUsuarioByIdQueryHandler(ApiDbContext dbContext) : IRequestHandler<GetUsuarioByIdQuery, UsuarioDetailResponse?>
{
    public async Task<UsuarioDetailResponse?> Handle(GetUsuarioByIdQuery query, CancellationToken cancellationToken)
    {
        GetUsuarioByIdValidator.Validate(query);

        var usuario = await dbContext.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == query.Id, cancellationToken);

        return usuario?.ToResponse();
    }
}