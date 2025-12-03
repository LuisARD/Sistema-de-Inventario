using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class AlmacenRepository : Repository<Almacen>, IAlmacenRepository
{
    public AlmacenRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Almacen>> GetAlmacenesActivosAsync()
    {
        return await _dbSet
            .Where(a => a.Activo)
            .ToListAsync();
    }

    public async Task<Almacen?> GetAlmacenConExistenciasAsync(int almacenId)
    {
        return await _dbSet
            .Include(a => a.Existencias)
            .ThenInclude(e => e.Producto)
            .FirstOrDefaultAsync(a => a.AlmacenId == almacenId);
    }
}
