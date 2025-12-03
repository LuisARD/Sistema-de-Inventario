using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<Usuario>> GetUsuariosActivosAsync()
    {
        return await _dbSet
            .Include(u => u.Rol)
            .Where(u => u.Activo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetUsuariosByRolAsync(int rolId)
    {
        return await _dbSet
            .Include(u => u.Rol)
            .Where(u => u.RolId == rolId)
            .ToListAsync();
    }
}
