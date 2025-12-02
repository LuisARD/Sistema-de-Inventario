namespace SistemaDeInventario.Domain.Entities;

public class Almacen
{
    public int AlmacenId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public bool Activo { get; set; } = true;

    // propiedades del navegador
    public virtual ICollection<Existencia> Existencias { get; set; } = new List<Existencia>();
    public virtual ICollection<Movimiento> MovimientosOrigen { get; set; } = new List<Movimiento>();
    public virtual ICollection<Movimiento> MovimientosDestino { get; set; } = new List<Movimiento>();
}
