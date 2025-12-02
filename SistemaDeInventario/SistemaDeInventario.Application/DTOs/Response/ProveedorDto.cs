namespace SistemaDeInventario.Application.DTOs.Response;

public class ProveedorDto
{
    public int ProveedorId { get; set; }
    public string NombreEmpresa { get; set; } = string.Empty;
    public string? NombreContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public int TotalProductos { get; set; }
}
