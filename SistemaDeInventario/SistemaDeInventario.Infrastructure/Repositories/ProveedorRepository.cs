using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class ProveedorRepository : Repository<Proveedor>, IProveedorRepository
{
    public ProveedorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Proveedor?> GetByNombreEmpresaAsync(string nombreEmpresa)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.NombreEmpresa == nombreEmpresa);
    }

    public async Task<IEnumerable<Proveedor>> GetProveedoresConProductosAsync()
    {
        return await _dbSet
            .Include(p => p.Productos)
            .ToListAsync();
    }
}
