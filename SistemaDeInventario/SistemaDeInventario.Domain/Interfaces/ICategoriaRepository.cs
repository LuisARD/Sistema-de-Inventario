using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<Categoria?> GetByNombreAsync(string nombre);
    Task<IEnumerable<Categoria>> GetCategoriasConProductosAsync();
}
