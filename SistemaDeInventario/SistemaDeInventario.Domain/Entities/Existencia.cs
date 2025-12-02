namespace SistemaDeInventario.Domain.Entities;

public class Existencia
{
    public int ExistenciaId { get; set; }
    public int ProductoId { get; set; }
    public int AlmacenId { get; set; }
    public int CantidadActual { get; set; } = 0;
    public string? UbicacionPasillo { get; set; }
    public DateTime UltimaActualizacion { get; set; } = DateTime.UtcNow;

    // propiedades del navegador
    public virtual Producto Producto { get; set; } = null!;
    public virtual Almacen Almacen { get; set; } = null!;
}
