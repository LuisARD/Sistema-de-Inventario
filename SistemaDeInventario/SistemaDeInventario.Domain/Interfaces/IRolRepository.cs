using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IRolRepository : IRepository<Rol>
{
    Task<Rol?> GetByNombreAsync(string nombre);
}
