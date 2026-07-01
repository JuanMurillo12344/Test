using Api.Application.Usuarios.Create;
using Api.Application.Usuarios.Delete;
using Api.Application.Usuarios.GetById;
using Api.Application.Usuarios.List;
using Api.Application.Usuarios.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(
    IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UsuarioDetailResponse>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var usuario = await mediator.Send(new GetUsuarioByIdQuery(id), cancellationToken);

        return usuario is null ? NotFound() : Ok(usuario);
    }

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

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateUsuarioResponse>> Put(
        [FromRoute] Guid id,
        [FromBody] UpdateUsuarioCommand command,
        CancellationToken cancellationToken)
    {
        var usuario = await mediator.Send(command with { Id = id }, cancellationToken);

        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteUsuarioCommand(id), cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}