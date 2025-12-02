namespace SistemaDeInventario.Application.DTOs.Request;

public class CreateAlmacenDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
}
