namespace SistemaDeInventario.Application.DTOs.Response;

public class CategoriaDto
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int TotalProductos { get; set; }
}
