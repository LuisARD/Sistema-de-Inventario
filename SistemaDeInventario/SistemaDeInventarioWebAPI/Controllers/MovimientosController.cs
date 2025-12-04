using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.Services.Interfaces;

namespace SistemaDeInventarioWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoService _movimientoService;

    public MovimientosController(IMovimientoService movimientoService)
    {
        _movimientoService = movimientoService;
    }

    /// <summary>
    /// Obtener todos los movimientos (Auditoría)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var movimientos = await _movimientoService.GetAllMovimientosAsync();
            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener movimientos", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener movimiento por ID (Auditoría)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var movimiento = await _movimientoService.GetMovimientoByIdAsync(id);
            if (movimiento == null)
                return NotFound(new { message = $"Movimiento con ID {id} no encontrado" });

            return Ok(movimiento);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener movimiento", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener movimientos por tipo (Auditoría)
    /// </summary>
    [HttpGet("tipo/{tipo}")]
    public async Task<IActionResult> GetByTipo(string tipo)
    {
        try
        {
            var movimientos = await _movimientoService.GetMovimientosByTipoAsync(tipo.ToUpper());
            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener movimientos por tipo", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener movimientos por usuario (Auditoría)
    /// </summary>
    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> GetByUsuario(int usuarioId)
    {
        try
        {
            var movimientos = await _movimientoService.GetMovimientosByUsuarioAsync(usuarioId);
            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener movimientos por usuario", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener movimientos por almacén (Auditoría)
    /// </summary>
    [HttpGet("almacen/{almacenId}")]
    public async Task<IActionResult> GetByAlmacen(int almacenId)
    {
        try
        {
            var movimientos = await _movimientoService.GetMovimientosByAlmacenAsync(almacenId);
            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener movimientos por almacén", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener movimientos por rango de fechas (Auditoría)
    /// </summary>
    [HttpGet("fecha")]
    public async Task<IActionResult> GetByFecha([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
    {
        try
        {
            var movimientos = await _movimientoService.GetMovimientosByFechaAsync(fechaInicio, fechaFin);
            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener movimientos por fecha", error = ex.Message });
        }
    }

    /// <summary>
    /// Registrar entrada de productos (Operarios de almacén)
    /// </summary>
    [HttpPost("entrada")]
    public async Task<IActionResult> RegistrarEntrada([FromBody] CreateMovimientoDto dto)
    {
        try
        {
            var movimiento = await _movimientoService.RegistrarEntradaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = movimiento.MovimientoId }, movimiento);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al registrar entrada", error = ex.Message });
        }
    }

    /// <summary>
    /// Registrar salida de productos (Operarios de almacén)
    /// </summary>
    [HttpPost("salida")]
    public async Task<IActionResult> RegistrarSalida([FromBody] CreateMovimientoDto dto)
    {
        try
        {
            var movimiento = await _movimientoService.RegistrarSalidaAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = movimiento.MovimientoId }, movimiento);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al registrar salida", error = ex.Message });
        }
    }
}
