using System.Net;
using System.Net.Http.Json;
using Api.Application.Usuarios.Create;
using Api.Application.Usuarios.GetById;
using Api.Application.Usuarios.List;
using Api.FunctionalTests.TestInfrastructure;
using Shouldly;

namespace Api.FunctionalTests.Queries;

[TestFixture]
public sealed class UsuariosQueriesTests : FunctionalTestBase
{
    [Test]
    public async Task GetById_ConUsuarioExistente_RetornaUsuario()
    {
        var createResponse = await Client.PostAsJsonAsync("/api/usuarios", UsuariosFakeDataFactory.CreateCommand());
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateUsuarioResponse>();

        created.ShouldNotBeNull();

        var response = await Client.GetAsync($"/api/usuarios/{created!.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<UsuarioDetailResponse>();

        payload.ShouldNotBeNull();
        payload.Id.ShouldBe(created.Id);
        payload.Nombre.ShouldBe(created.Nombre);
        payload.Apellido.ShouldBe(created.Apellido);
        payload.Email.ShouldBe(created.Email);
    }

    [Test]
    public async Task GetById_ConUsuarioInexistente_RetornaNotFound()
    {
        var response = await Client.GetAsync($"/api/usuarios/{Guid.NewGuid()}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_ConPaginacion_RetornaSoloLaPaginaSolicitada()
    {
        await SeedUsuariosAsync(12);

        var response = await Client.GetAsync("/api/usuarios?page=2&pageSize=5");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<PagedUsuariosResponse>();

        payload.ShouldNotBeNull();
        payload.Page.ShouldBe(2);
        payload.PageSize.ShouldBe(5);
        payload.TotalCount.ShouldBe(12);
        payload.TotalPages.ShouldBe(3);
        payload.Items.Count.ShouldBe(5);
    }

    [Test]
    public async Task Get_ConPaginaMayorAlTotal_RetornaSinElementos()
    {
        await SeedUsuariosAsync(3);

        var response = await Client.GetAsync("/api/usuarios?page=4&pageSize=2");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<PagedUsuariosResponse>();

        payload.ShouldNotBeNull();
        payload.TotalCount.ShouldBe(3);
        payload.TotalPages.ShouldBe(2);
        payload.Items.ShouldBeEmpty();
    }

    [Test]
    public async Task Get_ExcluyeUsuariosEliminados()
    {
        var createResponse = await Client.PostAsJsonAsync("/api/usuarios", UsuariosFakeDataFactory.CreateCommand());
        createResponse.EnsureSuccessStatusCode();

        var created = await createResponse.Content.ReadFromJsonAsync<CreateUsuarioResponse>();
        created.ShouldNotBeNull();

        var deleteResponse = await Client.DeleteAsync($"/api/usuarios/{created!.Id}");
        deleteResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var response = await Client.GetAsync("/api/usuarios?page=1&pageSize=10");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<PagedUsuariosResponse>();

        payload.ShouldNotBeNull();
        payload.TotalCount.ShouldBe(0);
        payload.Items.ShouldBeEmpty();
    }

    [Test]
    public async Task Get_ConPaginaMenorAUno_RetornaBadRequest()
    {
        var response = await Client.GetAsync("/api/usuarios?page=0&pageSize=10");

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Get_ConPageSizeCero_RetornaBadRequest()
    {
        var response = await Client.GetAsync("/api/usuarios?page=1&pageSize=0");

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Get_ConPageSizeMayorA100_RetornaBadRequest()
    {
        var response = await Client.GetAsync("/api/usuarios?page=1&pageSize=101");

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}