namespace SistemaDeInventario.Application.DTOs.Request;

public class CreateDetalleMovimientoDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal CostoUnitarioHistorico { get; set; }
}
