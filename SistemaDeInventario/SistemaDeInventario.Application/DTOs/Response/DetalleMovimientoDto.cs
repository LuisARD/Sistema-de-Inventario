namespace SistemaDeInventario.Application.DTOs.Response;

public class DetalleMovimientoDto
{
    public int DetalleId { get; set; }
    public int MovimientoId { get; set; }
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; } = string.Empty;
    public string CodigoSku { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal CostoUnitarioHistorico { get; set; }
    public decimal CostoTotal { get; set; }
}
