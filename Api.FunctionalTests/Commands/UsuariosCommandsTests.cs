using System.Net;
using System.Net.Http.Json;
using Api.Application.Usuarios.Create;
using Api.Application.Usuarios.Delete;
using Api.Application.Usuarios.Update;
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

    [Test]
    public async Task Put_ConDatosValidos_ActualizaUsuario()
    {
        var createResponse = await Client.PostAsJsonAsync("/api/usuarios", UsuariosFakeDataFactory.CreateCommand());
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateUsuarioResponse>();
        created.ShouldNotBeNull();

        var updateCommand = new UpdateUsuarioCommand(created!.Id, "Carlos", "Lopez", "carlos@example.com");

        var response = await Client.PutAsJsonAsync($"/api/usuarios/{created.Id}", updateCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<UpdateUsuarioResponse>();

        payload.ShouldNotBeNull();
        payload.Id.ShouldBe(created.Id);
        payload.Nombre.ShouldBe("Carlos");
        payload.Apellido.ShouldBe("Lopez");
        payload.Email.ShouldBe("carlos@example.com");
    }

    [Test]
    public async Task Put_ConUsuarioInexistente_RetornaNotFound()
    {
        var response = await Client.PutAsJsonAsync($"/api/usuarios/{Guid.NewGuid()}", new UpdateUsuarioCommand(Guid.Empty, "Carlos", "Lopez", "carlos@example.com"));

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_ConUsuarioExistente_EliminaSuavemente()
    {
        var createResponse = await Client.PostAsJsonAsync("/api/usuarios", UsuariosFakeDataFactory.CreateCommand());
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateUsuarioResponse>();
        created.ShouldNotBeNull();

        var deleteResponse = await Client.DeleteAsync($"/api/usuarios/{created!.Id}");

        deleteResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var getResponse = await Client.GetAsync($"/api/usuarios/{created.Id}");

        getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_ConUsuarioInexistente_RetornaNotFound()
    {
        var response = await Client.DeleteAsync($"/api/usuarios/{Guid.NewGuid()}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}