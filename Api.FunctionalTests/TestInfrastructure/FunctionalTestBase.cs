using System.Net.Http.Json;
using Api.Application.Usuarios.Create;
using Api.FunctionalTests.TestData;

namespace Api.FunctionalTests.TestInfrastructure;

public abstract class FunctionalTestBase
{
    private readonly TestDatabase database = new();

    protected ApiFunctionalTestFactory Factory = null!;

    protected HttpClient Client = null!;

    protected UsuariosFakeDataFactory UsuariosFakeDataFactory { get; } = new();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Factory = new ApiFunctionalTestFactory(database.ConnectionString);
        Client = Factory.CreateClient();
    }

    [SetUp]
    public async Task SetUp()
    {
        await database.ResetAsync(Factory.Services);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        Client.Dispose();
        Factory.Dispose();
        await database.DisposeAsync();
    }

    protected async Task SeedUsuariosAsync(int total)
    {
        foreach (var command in UsuariosFakeDataFactory.CreateCommands(total))
        {
            var response = await Client.PostAsJsonAsync("/api/usuarios", command);
            response.EnsureSuccessStatusCode();
        }
    }
}