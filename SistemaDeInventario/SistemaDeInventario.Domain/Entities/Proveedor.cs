namespace SistemaDeInventario.Domain.Entities;

public class Proveedor
{
    public int ProveedorId { get; set; }
    public string NombreEmpresa { get; set; } = string.Empty;
    public string? NombreContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }

    // Navigation properties
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
