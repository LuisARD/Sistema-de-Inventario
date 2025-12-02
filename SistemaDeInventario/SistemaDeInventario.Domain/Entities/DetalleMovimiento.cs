namespace SistemaDeInventario.Domain.Entities;

public class DetalleMovimiento
{
    public int DetalleId { get; set; }
    public int MovimientoId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal CostoUnitarioHistorico { get; set; }

    // Propiedades del navegador
    public virtual Movimiento Movimiento { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
