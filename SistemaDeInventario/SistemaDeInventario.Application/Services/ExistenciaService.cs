using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class ExistenciaService : IExistenciaService
{
    private readonly IExistenciaRepository _existenciaRepository;

    public ExistenciaService(IExistenciaRepository existenciaRepository)
    {
        _existenciaRepository = existenciaRepository;
    }

    public async Task<IEnumerable<ExistenciaDto>> GetAllExistenciasAsync()
    {
        var existencias = await _existenciaRepository.GetAllAsync();
        return existencias.Select(MapToDto);
    }

    public async Task<IEnumerable<ExistenciaDto>> GetExistenciasByProductoAsync(int productoId)
    {
        var existencias = await _existenciaRepository.GetExistenciasByProductoAsync(productoId);
        return existencias.Select(MapToDto);
    }

    public async Task<IEnumerable<ExistenciaDto>> GetExistenciasByAlmacenAsync(int almacenId)
    {
        var existencias = await _existenciaRepository.GetExistenciasByAlmacenAsync(almacenId);
        return existencias.Select(MapToDto);
    }

    public async Task<ExistenciaDto?> GetExistenciaByProductoYAlmacenAsync(int productoId, int almacenId)
    {
        var existencia = await _existenciaRepository.GetByProductoYAlmacenAsync(productoId, almacenId);
        return existencia != null ? MapToDto(existencia) : null;
    }

    public async Task<IEnumerable<ExistenciaDto>> GetProductosConStockBajoAsync()
    {
        var existencias = await _existenciaRepository.GetProductosConStockBajoAsync();
        return existencias.Select(MapToDto);
    }

    private ExistenciaDto MapToDto(Existencia existencia)
    {
        var stockBajo = existencia.CantidadActual <= (existencia.Producto?.StockMinimo ?? 0);

        return new ExistenciaDto
        {
            ExistenciaId = existencia.ExistenciaId,
            ProductoId = existencia.ProductoId,
            ProductoNombre = existencia.Producto?.Nombre ?? string.Empty,
            CodigoSku = existencia.Producto?.CodigoSku ?? string.Empty,
            AlmacenId = existencia.AlmacenId,
            AlmacenNombre = existencia.Almacen?.Nombre ?? string.Empty,
            CantidadActual = existencia.CantidadActual,
            StockMinimo = existencia.Producto?.StockMinimo ?? 0,
            UbicacionPasillo = existencia.UbicacionPasillo,
            UltimaActualizacion = existencia.UltimaActualizacion,
            StockBajo = stockBajo
        };
    }
}
