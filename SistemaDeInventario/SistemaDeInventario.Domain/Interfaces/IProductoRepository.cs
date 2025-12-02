using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IProductoRepository : IRepository<Producto>
{
    Task<Producto?> GetByCodigoSkuAsync(string codigoSku);
    Task<IEnumerable<Producto>> GetProductosActivosAsync();
    Task<IEnumerable<Producto>> GetProductosByCategoriaAsync(int categoriaId);
    Task<IEnumerable<Producto>> GetProductosByProveedorAsync(int proveedorId);
    Task<Producto?> GetProductoConDetallesAsync(int productoId);
}
