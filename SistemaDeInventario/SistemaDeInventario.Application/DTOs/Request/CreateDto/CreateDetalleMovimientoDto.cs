namespace SistemaDeInventario.Application.DTOs.Request.CreateDto;

public class CreateDetalleMovimientoDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal CostoUnitarioHistorico { get; set; }
}
