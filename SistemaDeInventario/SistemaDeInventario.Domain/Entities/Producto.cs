namespace SistemaDeInventario.Domain.Entities;

public class Producto
{
    public int ProductoId { get; set; }
    public string CodigoSku { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CategoriaId { get; set; }
    public int ProveedorId { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public string UnidadMedida { get; set; } = "Unidad";
    public int StockMinimo { get; set; } = 5;
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Categoria Categoria { get; set; } = null!;
    public virtual Proveedor Proveedor { get; set; } = null!;
    public virtual ICollection<Existencia> Existencias { get; set; } = new List<Existencia>();
    public virtual ICollection<DetalleMovimiento> DetalleMovimientos { get; set; } = new List<DetalleMovimiento>();
}
