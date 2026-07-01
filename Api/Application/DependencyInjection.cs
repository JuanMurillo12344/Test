using Microsoft.Extensions.DependencyInjection;

namespace Api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<Usuarios.Create.ICreateUsuarioCommandHandler, Usuarios.Create.CreateUsuarioCommandHandler>();
        services.AddScoped<Usuarios.List.IGetUsuariosQueryHandler, Usuarios.List.GetUsuariosQueryHandler>();

        return services;
    }
}