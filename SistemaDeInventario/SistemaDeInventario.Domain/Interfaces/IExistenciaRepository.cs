using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IExistenciaRepository : IRepository<Existencia>
{
    Task<Existencia?> GetByProductoYAlmacenAsync(int productoId, int almacenId);
    Task<IEnumerable<Existencia>> GetExistenciasByProductoAsync(int productoId);
    Task<IEnumerable<Existencia>> GetExistenciasByAlmacenAsync(int almacenId);
    Task<IEnumerable<Existencia>> GetProductosConStockBajoAsync();
    Task ActualizarStockAsync(int productoId, int almacenId, int cantidad);
}
