using SistemaDeInventario.Application.DTOs.Request.CreateDto;
using SistemaDeInventario.Application.DTOs.Request.UpdateDto;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> GetAllProductosAsync();
    Task<ProductoDto?> GetProductoByIdAsync(int id);
    Task<ProductoDto?> GetProductoBySkuAsync(string codigoSku);
    Task<ProductoDto> CreateProductoAsync(CreateProductoDto dto);
    Task<ProductoDto> UpdateProductoAsync(UpdateProductoDto dto);
    Task<bool> DeleteProductoAsync(int id);
    Task<IEnumerable<ProductoDto>> GetProductosByCategoriaAsync(int categoriaId);
    Task<IEnumerable<ProductoDto>> GetProductosByProveedorAsync(int proveedorId);
}
