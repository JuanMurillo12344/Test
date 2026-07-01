using Api.Domain.Usuarios;
using Api.Infrastructure.Persistence;
using Api.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("bd")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'bd'.");

        services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(connectionString));
        services.AddHostedService<UsuarioSeederHostedService>();

        return services;
    }
}