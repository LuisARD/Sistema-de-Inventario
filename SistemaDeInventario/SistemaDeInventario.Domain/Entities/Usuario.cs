namespace SistemaDeInventario.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Rol { get; set; } = "Usuario";

    public ICollection<MovimientoInventario> Movimientos { get; set; } = new List<MovimientoInventario>();
}
