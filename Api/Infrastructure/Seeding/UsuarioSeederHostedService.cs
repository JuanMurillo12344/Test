using Api.Domain.Usuarios;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Seeding;

public sealed class UsuarioSeederHostedService(IServiceScopeFactory scopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Persistence.ApiDbContext>();

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        if (await dbContext.Usuarios.AnyAsync(cancellationToken))
        {
            return;
        }

        var faker = new Faker("es");

        var usuarios = Enumerable.Range(1, 12)
            .Select(_ => new Usuario(
                Guid.NewGuid(),
                faker.Name.FirstName(),
                faker.Name.LastName(),
                faker.Internet.Email()))
            .ToArray();

        await dbContext.Usuarios.AddRangeAsync(usuarios, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}