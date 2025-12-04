using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Infrastructure.Persistence;

namespace SistemaDeInventarioWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController : ControllerBase
{
    private readonly AppDbContext _context;

    public TestController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Probar conexión a la base de datos (ELIMINAR EN PRODUCCIÓN)
    /// </summary>
    [HttpGet("connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                var dbName = _context.Database.GetDbConnection().Database;
                var server = _context.Database.GetDbConnection().DataSource;

                return Ok(new ConnectionTestResult
                {
                    Success = true,
                    Message = "? Conexión exitosa a PostgreSQL",
                    Database = dbName,
                    Server = server
                });
            }
            
            return StatusCode(500, new ConnectionTestResult
            {
                Success = false,
                Message = "? No se pudo conectar a la base de datos"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ConnectionTestResult
            {
                Success = false,
                Message = "? Error al conectar a la base de datos",
                Error = ex.Message,
                InnerError = ex.InnerException?.Message
            });
        }
    }

    /// <summary>
    /// Verificar tablas de la base de datos (ELIMINAR EN PRODUCCIÓN)
    /// </summary>
    [HttpGet("tables")]
    public async Task<IActionResult> TestTables()
    {
        try
        {
            var rolesCount = await _context.Roles.CountAsync();
            var usuariosCount = await _context.Usuarios.CountAsync();
            var categoriasCount = await _context.Categorias.CountAsync();
            var proveedoresCount = await _context.Proveedores.CountAsync();
            var almacenesCount = await _context.Almacenes.CountAsync();
            var productosCount = await _context.Productos.CountAsync();
            var existenciasCount = await _context.Existencias.CountAsync();
            var movimientosCount = await _context.Movimientos.CountAsync();

            return Ok(new TablesTestResult
            {
                Success = true,
                Message = "? Tablas verificadas correctamente",
                Tablas = new TableCounts
                {
                    Roles = rolesCount,
                    Usuarios = usuariosCount,
                    Categorias = categoriasCount,
                    Proveedores = proveedoresCount,
                    Almacenes = almacenesCount,
                    Productos = productosCount,
                    Existencias = existenciasCount,
                    Movimientos = movimientosCount
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new TablesTestResult
            {
                Success = false,
                Message = "? Error al consultar las tablas",
                Error = ex.Message,
                InnerError = ex.InnerException?.Message
            });
        }
    }

    /// <summary>
    /// Obtener datos de muestra (ELIMINAR EN PRODUCCIÓN)
    /// </summary>
    [HttpGet("sample-data")]
    public async Task<IActionResult> GetSampleData()
    {
        try
        {
            var roles = await _context.Roles.Take(5).ToListAsync();
            var categorias = await _context.Categorias.Take(5).ToListAsync();
            var almacenes = await _context.Almacenes.Take(5).ToListAsync();

            return Ok(new SampleDataResult
            {
                Success = true,
                Message = "? Datos de muestra obtenidos",
                Roles = roles,
                Categorias = categorias,
                Almacenes = almacenes
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new SampleDataResult
            {
                Success = false,
                Message = "? Error al obtener datos",
                Error = ex.Message,
                InnerError = ex.InnerException?.Message
            });
        }
    }
}

// DTOs para las respuestas
public class ConnectionTestResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Database { get; set; }
    public string? Server { get; set; }
    public string? Error { get; set; }
    public string? InnerError { get; set; }
}

public class TablesTestResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public TableCounts? Tablas { get; set; }
    public string? Error { get; set; }
    public string? InnerError { get; set; }
}

public class TableCounts
{
    public int Roles { get; set; }
    public int Usuarios { get; set; }
    public int Categorias { get; set; }
    public int Proveedores { get; set; }
    public int Almacenes { get; set; }
    public int Productos { get; set; }
    public int Existencias { get; set; }
    public int Movimientos { get; set; }
}

public class SampleDataResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Roles { get; set; }
    public object? Categorias { get; set; }
    public object? Almacenes { get; set; }
    public string? Error { get; set; }
    public string? InnerError { get; set; }
}
