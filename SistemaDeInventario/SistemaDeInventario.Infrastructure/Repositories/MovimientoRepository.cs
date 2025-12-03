using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventario.Infrastructure.Repositories;

public class MovimientoRepository : Repository<Movimiento>, IMovimientoRepository
{
    public MovimientoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Movimiento?> GetMovimientoConDetallesAsync(int movimientoId)
    {
        return await _dbSet
            .Include(m => m.Usuario)
            .ThenInclude(u => u.Rol)
            .Include(m => m.AlmacenOrigen)
            .Include(m => m.AlmacenDestino)
            .Include(m => m.DetalleMovimientos)
            .ThenInclude(d => d.Producto)
            .FirstOrDefaultAsync(m => m.MovimientoId == movimientoId);
    }

    public async Task<IEnumerable<Movimiento>> GetMovimientosByTipoAsync(string tipo)
    {
        return await _dbSet
            .Include(m => m.Usuario)
            .Include(m => m.AlmacenOrigen)
            .Include(m => m.AlmacenDestino)
            .Where(m => m.TipoMovimiento == tipo)
            .OrderByDescending(m => m.FechaMovimiento)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetMovimientosByUsuarioAsync(int usuarioId)
    {
        return await _dbSet
            .Include(m => m.Usuario)
            .Include(m => m.AlmacenOrigen)
            .Include(m => m.AlmacenDestino)
            .Where(m => m.UsuarioId == usuarioId)
            .OrderByDescending(m => m.FechaMovimiento)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetMovimientosByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        return await _dbSet
            .Include(m => m.Usuario)
            .Include(m => m.AlmacenOrigen)
            .Include(m => m.AlmacenDestino)
            .Where(m => m.FechaMovimiento >= fechaInicio && m.FechaMovimiento <= fechaFin)
            .OrderByDescending(m => m.FechaMovimiento)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movimiento>> GetMovimientosByAlmacenAsync(int almacenId)
    {
        return await _dbSet
            .Include(m => m.Usuario)
            .Include(m => m.AlmacenOrigen)
            .Include(m => m.AlmacenDestino)
            .Where(m => m.AlmacenOrigenId == almacenId || m.AlmacenDestinoId == almacenId)
            .OrderByDescending(m => m.FechaMovimiento)
            .ToListAsync();
    }
}
