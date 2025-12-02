using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IMovimientoRepository : IRepository<Movimiento>
{
    Task<Movimiento?> GetMovimientoConDetallesAsync(int movimientoId);
    Task<IEnumerable<Movimiento>> GetMovimientosByTipoAsync(string tipo);
    Task<IEnumerable<Movimiento>> GetMovimientosByUsuarioAsync(int usuarioId);
    Task<IEnumerable<Movimiento>> GetMovimientosByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<Movimiento>> GetMovimientosByAlmacenAsync(int almacenId);
}
