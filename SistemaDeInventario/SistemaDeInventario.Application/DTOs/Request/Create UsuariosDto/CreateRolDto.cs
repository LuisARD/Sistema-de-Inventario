namespace SistemaDeInventario.Application.DTOs.Request;

public class CreateRolDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
