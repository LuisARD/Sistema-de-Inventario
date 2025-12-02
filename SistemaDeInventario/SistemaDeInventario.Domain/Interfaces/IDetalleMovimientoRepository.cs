using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IDetalleMovimientoRepository : IRepository<DetalleMovimiento>
{
    Task<IEnumerable<DetalleMovimiento>> GetDetallesByMovimientoAsync(int movimientoId);
    Task<IEnumerable<DetalleMovimiento>> GetDetallesByProductoAsync(int productoId);
}
