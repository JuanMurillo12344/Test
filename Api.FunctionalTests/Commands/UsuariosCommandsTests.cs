using System.Net;
using System.Net.Http.Json;
using Api.Application.Usuarios.Create;
using Api.FunctionalTests.TestInfrastructure;
using Shouldly;

namespace Api.FunctionalTests.Commands;

[TestFixture]
public sealed class UsuariosCommandsTests : FunctionalTestBase
{
    [Test]
    public async Task Post_ConDatosValidos_CreaUsuario()
    {
        var command = UsuariosFakeDataFactory.CreateCommand();

        var response = await Client.PostAsJsonAsync("/api/usuarios", command);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<CreateUsuarioResponse>();

        payload.ShouldNotBeNull();
        payload.Nombre.ShouldBe(command.Nombre);
        payload.Apellido.ShouldBe(command.Apellido);
        payload.Email.ShouldBe(command.Email);
        payload.Id.ShouldNotBe(Guid.Empty);
    }

    [Test]
    public async Task Post_ConNombreVacio_RetornaBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/usuarios", new CreateUsuarioCommand(string.Empty, "Perez", "juan@example.com"));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Post_ConApellidoVacio_RetornaBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/usuarios", new CreateUsuarioCommand("Juan", string.Empty, "juan@example.com"));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Post_ConEmailVacio_RetornaBadRequest()
    {
        var response = await Client.PostAsJsonAsync("/api/usuarios", new CreateUsuarioCommand("Juan", "Perez", string.Empty));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}