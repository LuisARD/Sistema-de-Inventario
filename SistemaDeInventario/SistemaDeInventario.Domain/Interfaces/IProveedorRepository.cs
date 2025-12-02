using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Domain.Interfaces;

public interface IProveedorRepository : IRepository<Proveedor>
{
    Task<Proveedor?> GetByNombreEmpresaAsync(string nombreEmpresa);
    Task<IEnumerable<Proveedor>> GetProveedoresConProductosAsync();
}
