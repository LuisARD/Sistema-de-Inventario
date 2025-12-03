using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Categoria?> GetByNombreAsync(string nombre)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Nombre == nombre);
    }

    public async Task<IEnumerable<Categoria>> GetCategoriasConProductosAsync()
    {
        return await _dbSet
            .Include(c => c.Productos)
            .ToListAsync();
    }
}
