using Api.Application.Usuarios.Create;
using Api.Application.Usuarios.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(
    IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedUsuariosResponse>> Get(
        [FromQuery] GetUsuariosQuery query,
        CancellationToken cancellationToken)
    {
        var usuarios = await mediator.Send(query, cancellationToken);
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<ActionResult<CreateUsuarioResponse>> Post(
        [FromBody] CreateUsuarioCommand command,
        CancellationToken cancellationToken)
    {
        var usuario = await mediator.Send(command, cancellationToken);
        return Ok(usuario);
    }
}