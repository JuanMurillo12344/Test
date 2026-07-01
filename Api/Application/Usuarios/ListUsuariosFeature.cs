using Api.Domain.Usuarios;
using MediatR;
using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Usuarios.List;

public sealed record GetUsuariosQuery(int Page = 1, int PageSize = 10) : IRequest<PagedUsuariosResponse>;

public sealed record UsuarioListItemResponse(Guid Id, string Nombre, string Apellido, string Email);

public sealed record PagedUsuariosResponse(
    IReadOnlyList<UsuarioListItemResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);

public static class GetUsuariosMapping
{
    public static UsuarioListItemResponse ToResponse(this Usuario usuario)
    {
        return new UsuarioListItemResponse(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email);
    }
}

public static class GetUsuariosValidator
{
    public static void Validate(GetUsuariosQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.Page < 1)
        {
            throw new ValidationException("La página debe ser mayor o igual a 1.");
        }

        if (query.PageSize < 1 || query.PageSize > 100)
        {
            throw new ValidationException("El tamaño de página debe estar entre 1 y 100.");
        }
    }
}

public sealed class GetUsuariosQueryHandler(ApiDbContext dbContext) : IRequestHandler<GetUsuariosQuery, PagedUsuariosResponse>
{
    public async Task<PagedUsuariosResponse> Handle(GetUsuariosQuery query, CancellationToken cancellationToken)
    {
        GetUsuariosValidator.Validate(query);

        var totalCount = await dbContext.Usuarios.CountAsync(cancellationToken);

        var usuarios = await dbContext.Usuarios
            .AsNoTracking()
            .OrderBy(usuario => usuario.Apellido)
            .ThenBy(usuario => usuario.Nombre)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var items = usuarios
            .Select(usuario => usuario.ToResponse())
            .ToArray();

        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize);

        return new PagedUsuariosResponse(items, query.Page, query.PageSize, totalCount, totalPages);
    }
}