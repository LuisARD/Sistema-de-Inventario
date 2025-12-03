using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class ExistenciaRepository : Repository<Existencia>, IExistenciaRepository
{
    public ExistenciaRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Existencia>> GetAllAsync()
    {
        return await _dbSet
            .Include(e => e.Producto)
            .Include(e => e.Almacen)
            .ToListAsync();
    }

    public async Task<Existencia?> GetByProductoYAlmacenAsync(int productoId, int almacenId)
    {
        return await _dbSet
            .Include(e => e.Producto)
            .Include(e => e.Almacen)
            .FirstOrDefaultAsync(e => e.ProductoId == productoId && e.AlmacenId == almacenId);
    }

    public async Task<IEnumerable<Existencia>> GetExistenciasByProductoAsync(int productoId)
    {
        return await _dbSet
            .Include(e => e.Producto)
            .Include(e => e.Almacen)
            .Where(e => e.ProductoId == productoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Existencia>> GetExistenciasByAlmacenAsync(int almacenId)
    {
        return await _dbSet
            .Include(e => e.Producto)
            .ThenInclude(p => p.Categoria)
            .Include(e => e.Almacen)
            .Where(e => e.AlmacenId == almacenId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Existencia>> GetProductosConStockBajoAsync()
    {
        return await _dbSet
            .Include(e => e.Producto)
            .ThenInclude(p => p.Categoria)
            .Include(e => e.Almacen)
            .Where(e => e.CantidadActual <= e.Producto.StockMinimo)
            .ToListAsync();
    }

    public async Task ActualizarStockAsync(int productoId, int almacenId, int cantidad)
    {
        var existencia = await GetByProductoYAlmacenAsync(productoId, almacenId);
        
        if (existencia != null)
        {
            existencia.CantidadActual += cantidad;
            existencia.UltimaActualizacion = DateTime.UtcNow;
            await UpdateAsync(existencia);
        }
    }
}
