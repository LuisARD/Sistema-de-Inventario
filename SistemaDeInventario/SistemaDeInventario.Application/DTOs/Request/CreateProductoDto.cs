namespace SistemaDeInventario.Application.DTOs.Request;

public class CreateProductoDto
{
    public string CodigoSku { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CategoriaId { get; set; }
    public int ProveedorId { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string UnidadMedida { get; set; } = "Unidad";
    public int StockMinimo { get; set; } = 5;
}
