using Api.Application.Usuarios.Create;
using Api.Application.Usuarios.List;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(
    IGetUsuariosQueryHandler getUsuariosQueryHandler,
    ICreateUsuarioCommandHandler createUsuarioCommandHandler) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Api.Application.Usuarios.UsuarioResponse>>> Get(CancellationToken cancellationToken)
    {
        var usuarios = await getUsuariosQueryHandler.HandleAsync(new GetUsuariosQuery(), cancellationToken);
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<ActionResult<Api.Application.Usuarios.UsuarioResponse>> Post(
        [FromBody] CreateUsuarioCommand command,
        CancellationToken cancellationToken)
    {
        var usuario = await createUsuarioCommandHandler.HandleAsync(command, cancellationToken);
        return Ok(usuario);
    }
}