using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Usuarios.Delete;

public sealed record DeleteUsuarioCommand(Guid Id) : IRequest<bool>;

public static class DeleteUsuarioValidator
{
    public static void Validate(DeleteUsuarioCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.Id == Guid.Empty)
        {
            throw new ValidationException("El id del usuario es obligatorio.");
        }
    }
}

public sealed class DeleteUsuarioCommandHandler(ApiDbContext dbContext) : IRequestHandler<DeleteUsuarioCommand, bool>
{
    public async Task<bool> Handle(DeleteUsuarioCommand command, CancellationToken cancellationToken)
    {
        DeleteUsuarioValidator.Validate(command);

        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == command.Id, cancellationToken);

        if (usuario is null)
        {
            return false;
        }

        usuario.Delete();

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}