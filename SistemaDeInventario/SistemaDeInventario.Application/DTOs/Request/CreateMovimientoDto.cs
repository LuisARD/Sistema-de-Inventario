namespace SistemaDeInventario.Application.DTOs.Request;

public class CreateMovimientoDto
{
    public string TipoMovimiento { get; set; } = string.Empty; // ENTRADA, SALIDA
    public string? Motivo { get; set; }
    public string? ReferenciaDocumento { get; set; }
    public int UsuarioId { get; set; }
    public int AlmacenOrigenId { get; set; }
    public int? AlmacenDestinoId { get; set; }
    public List<CreateDetalleMovimientoDto> Detalles { get; set; } = new();
}
