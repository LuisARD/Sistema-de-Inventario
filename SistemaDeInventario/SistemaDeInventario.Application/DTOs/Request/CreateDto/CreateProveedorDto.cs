namespace SistemaDeInventario.Application.DTOs.Request.CreateDto;

public class CreateProveedorDto
{
    public string NombreEmpresa { get; set; } = string.Empty;
    public string? NombreContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
}
