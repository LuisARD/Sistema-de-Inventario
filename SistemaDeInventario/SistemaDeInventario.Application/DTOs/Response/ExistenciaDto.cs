namespace SistemaDeInventario.Application.DTOs.Response;

public class ExistenciaDto
{
    public int ExistenciaId { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public string CodigoSku { get; set; } = string.Empty;
    public int AlmacenId { get; set; }
    public string AlmacenNombre { get; set; } = string.Empty;
    public int CantidadActual { get; set; }
    public int StockMinimo { get; set; }
    public string? UbicacionPasillo { get; set; }
    public DateTime UltimaActualizacion { get; set; }
    public bool StockBajo { get; set; }
}
