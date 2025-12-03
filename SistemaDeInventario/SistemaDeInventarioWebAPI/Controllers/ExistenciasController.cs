using Microsoft.AspNetCore.Mvc;
using SistemaDeInventario.Application.Services.Interfaces;

namespace SistemaDeInventarioWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExistenciasController : ControllerBase
{
    private readonly IExistenciaService _existenciaService;

    public ExistenciasController(IExistenciaService existenciaService)
    {
        _existenciaService = existenciaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var existencias = await _existenciaService.GetAllExistenciasAsync();
            return Ok(existencias);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener existencias", error = ex.Message });
        }
    }

    [HttpGet("producto/{productoId}")]
    public async Task<IActionResult> GetByProducto(int productoId)
    {
        try
        {
            var existencias = await _existenciaService.GetExistenciasByProductoAsync(productoId);
            return Ok(existencias);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener existencias por producto", error = ex.Message });
        }
    }

    [HttpGet("almacen/{almacenId}")]
    public async Task<IActionResult> GetByAlmacen(int almacenId)
    {
        try
        {
            var existencias = await _existenciaService.GetExistenciasByAlmacenAsync(almacenId);
            return Ok(existencias);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener existencias por almacén", error = ex.Message });
        }
    }

    [HttpGet("stock-bajo")]
    public async Task<IActionResult> GetStockBajo()
    {
        try
        {
            var existencias = await _existenciaService.GetProductosConStockBajoAsync();
            return Ok(existencias);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener productos con stock bajo", error = ex.Message });
        }
    }

    [HttpGet("producto/{productoId}/almacen/{almacenId}")]
    public async Task<IActionResult> GetByProductoYAlmacen(int productoId, int almacenId)
    {
        try
        {
            var existencia = await _existenciaService.GetExistenciaByProductoYAlmacenAsync(productoId, almacenId);
            if (existencia == null)
                return NotFound(new { message = $"No se encontró existencia para el producto {productoId} en el almacén {almacenId}" });

            return Ok(existencia);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener existencia", error = ex.Message });
        }
    }
}
