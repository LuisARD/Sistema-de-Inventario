using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<IEnumerable<Usuario>> GetUsuariosActivosAsync();
    Task<IEnumerable<Usuario>> GetUsuariosByRolAsync(int rolId);
}
