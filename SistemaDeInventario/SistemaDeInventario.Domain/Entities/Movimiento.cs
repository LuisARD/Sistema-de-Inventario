namespace SistemaDeInventario.Domain.Entities;

public class Movimiento
{
    public int MovimientoId { get; set; }
    public DateTime FechaMovimiento { get; set; } = DateTime.UtcNow;
    public string TipoMovimiento { get; set; } = string.Empty; // ENTRADA, SALIDA
    public string? Motivo { get; set; }
    public string? ReferenciaDocumento { get; set; }
    public int UsuarioId { get; set; }
    public int AlmacenOrigenId { get; set; }
    public int? AlmacenDestinoId { get; set; }

    // Pro
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Almacen AlmacenOrigen { get; set; } = null!;
    public virtual Almacen? AlmacenDestino { get; set; }
    public virtual ICollection<DetalleMovimiento> DetalleMovimientos { get; set; } = new List<DetalleMovimiento>();
}
