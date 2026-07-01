namespace Api.Domain.Usuarios;

public class Usuario
{
    public Guid Id { get; private set; }

    public string Nombre { get; private set; }

    public string Apellido { get; private set; }

    public string Email { get; private set; }

    public Usuario(Guid id, string nombre, string apellido, string email)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("El identificador del usuario es obligatorio.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(nombre));
        }

        if (string.IsNullOrWhiteSpace(apellido))
        {
            throw new ArgumentException("El apellido del usuario es obligatorio.", nameof(apellido));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("El email del usuario es obligatorio.", nameof(email));
        }

        Id = id;
        Nombre = nombre.Trim();
        Apellido = apellido.Trim();
        Email = email.Trim();
    }

    private Usuario()
    {
    }
}