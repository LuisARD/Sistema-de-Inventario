namespace SistemaDeInventario.Application.DTOs.Request;

public class UpdateCategoriaDto
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
