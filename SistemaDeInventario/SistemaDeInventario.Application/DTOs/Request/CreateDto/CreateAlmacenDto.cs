namespace SistemaDeInventario.Application.DTOs.Request.CreateDto;

public class CreateAlmacenDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
}
