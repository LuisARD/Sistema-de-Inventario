using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IAlmacenRepository : IRepository<Almacen>
{
    Task<IEnumerable<Almacen>> GetAlmacenesActivosAsync();
    Task<Almacen?> GetAlmacenConExistenciasAsync(int almacenId);
}
