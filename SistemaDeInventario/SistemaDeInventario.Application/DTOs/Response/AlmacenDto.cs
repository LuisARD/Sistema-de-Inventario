namespace SistemaDeInventario.Application.DTOs.Response;

public class AlmacenDto
{
    public int AlmacenId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public bool Activo { get; set; }
}
