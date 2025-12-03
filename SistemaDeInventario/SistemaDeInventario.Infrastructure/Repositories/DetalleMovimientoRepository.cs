using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class DetalleMovimientoRepository : Repository<DetalleMovimiento>, IDetalleMovimientoRepository
{
    public DetalleMovimientoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DetalleMovimiento>> GetDetallesByMovimientoAsync(int movimientoId)
    {
        return await _dbSet
            .Include(d => d.Producto)
            .Include(d => d.Movimiento)
            .Where(d => d.MovimientoId == movimientoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DetalleMovimiento>> GetDetallesByProductoAsync(int productoId)
    {
        return await _dbSet
            .Include(d => d.Producto)
            .Include(d => d.Movimiento)
            .Where(d => d.ProductoId == productoId)
            .ToListAsync();
    }
}
