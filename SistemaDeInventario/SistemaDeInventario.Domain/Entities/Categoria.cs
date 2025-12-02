namespace SistemaDeInventario.Domain.Entities;

public class Categoria
{
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    // propiedades de navegación
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
