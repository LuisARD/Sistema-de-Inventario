using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IExistenciaService
{
    Task<IEnumerable<ExistenciaDto>> GetAllExistenciasAsync();
    Task<IEnumerable<ExistenciaDto>> GetExistenciasByProductoAsync(int productoId);
    Task<IEnumerable<ExistenciaDto>> GetExistenciasByAlmacenAsync(int almacenId);
    Task<ExistenciaDto?> GetExistenciaByProductoYAlmacenAsync(int productoId, int almacenId);
    Task<IEnumerable<ExistenciaDto>> GetProductosConStockBajoAsync();
}
