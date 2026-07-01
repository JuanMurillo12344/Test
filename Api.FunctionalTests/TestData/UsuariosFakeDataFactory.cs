using Api.Application.Usuarios.Create;
using Bogus;

namespace Api.FunctionalTests.TestData;

public sealed class UsuariosFakeDataFactory
{
    private readonly Faker faker = new("es");

    public CreateUsuarioCommand CreateCommand()
    {
        return new CreateUsuarioCommand(
            faker.Name.FirstName(),
            faker.Name.LastName(),
            faker.Internet.Email());
    }

    public IReadOnlyList<CreateUsuarioCommand> CreateCommands(int total)
    {
        return Enumerable.Range(1, total)
            .Select(_ => CreateCommand())
            .ToArray();
    }
}