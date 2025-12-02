namespace SistemaDeInventario.Domain.Entities;

public enum TipoMovimiento
{
    Entrada = 1,
    Salida = 2
}

public class MovimientoInventario
{
    public int Id { get; set; }

    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }

    public int Cantidad { get; set; }
    public TipoMovimiento TipoMovimiento { get; set; }
    public string Motivo { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public string? DocumentoReferencia { get; set; }
}
