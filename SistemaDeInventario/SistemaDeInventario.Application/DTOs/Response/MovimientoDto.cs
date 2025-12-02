namespace SistemaDeInventario.Application.DTOs.Response;

public class MovimientoDto
{
    public int MovimientoId { get; set; }
    public DateTime FechaMovimiento { get; set; }
    public string TipoMovimiento { get; set; } = string.Empty;
    public string? Motivo { get; set; }
    public string? ReferenciaDocumento { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public int AlmacenOrigenId { get; set; }
    public string AlmacenOrigenNombre { get; set; } = string.Empty;
    public int? AlmacenDestinoId { get; set; }
    public string? AlmacenDestinoNombre { get; set; }
    public List<DetalleMovimientoDto> Detalles { get; set; } = new();
}
