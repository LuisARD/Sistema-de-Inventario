namespace SistemaDeInventario.Domain.Entities;

public class Usuario
{
    public int UsuarioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RolId { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public bool Activo { get; set; } = true;

    // Navigation properties
    public virtual Rol Rol { get; set; } = null!;
    public virtual ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}
