using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories.UsuariosRepository;

public class RolRepository : Repository<Rol>, IRolRepository
{
    public RolRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Rol?> GetByNombreAsync(string nombre)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Nombre == nombre);
    }
}
