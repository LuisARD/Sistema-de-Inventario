using Microsoft.AspNetCore.Mvc;
using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.Services.Interfaces;

namespace SistemaDeInventarioWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolesController(IRolService rolService)
    {
        _rolService = rolService;
    }

    /// <summary>
    /// Obtener todos los roles
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var roles = await _rolService.GetAllRolesAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener roles", error = ex.Message });
        }
    }

    /// <summary>
    /// Obtener rol por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var rol = await _rolService.GetRolByIdAsync(id);
            if (rol == null)
                return NotFound(new { message = $"Rol con ID {id} no encontrado" });

            return Ok(rol);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener rol", error = ex.Message });
        }
    }

    /// <summary>
    /// Crear nuevo rol
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRolDto dto)
    {
        try
        {
            var rol = await _rolService.CreateRolAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = rol.RolId }, rol);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear rol", error = ex.Message });
        }
    }
}
