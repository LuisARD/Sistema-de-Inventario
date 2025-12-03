using Microsoft.AspNetCore.Mvc;
using SistemaDeInventario.Application.DTOs.Request.CreateDto;
using SistemaDeInventario.Application.DTOs.Request.UpdateDto;
using SistemaDeInventario.Application.Services.Interfaces;

namespace SistemaDeInventarioWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var productos = await _productoService.GetAllProductosAsync();
            return Ok(productos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener productos", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener producto", error = ex.Message });
        }
    }

    [HttpGet("sku/{sku}")]
    public async Task<IActionResult> GetBySku(string sku)
    {
        try
        {
            var producto = await _productoService.GetProductoBySkuAsync(sku);
            if (producto == null)
                return NotFound(new { message = $"Producto con SKU '{sku}' no encontrado" });

            return Ok(producto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener producto", error = ex.Message });
        }
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> GetByCategoria(int categoriaId)
    {
        try
        {
            var productos = await _productoService.GetProductosByCategoriaAsync(categoriaId);
            return Ok(productos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener productos por categoría", error = ex.Message });
        }
    }

    [HttpGet("proveedor/{proveedorId}")]
    public async Task<IActionResult> GetByProveedor(int proveedorId)
    {
        try
        {
            var productos = await _productoService.GetProductosByProveedorAsync(proveedorId);
            return Ok(productos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener productos por proveedor", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductoDto dto)
    {
        try
        {
            var producto = await _productoService.CreateProductoAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = producto.ProductoId }, producto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al crear producto", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductoDto dto)
    {
        try
        {
            if (id != dto.ProductoId)
                return BadRequest(new { message = "El ID del producto no coincide" });

            var producto = await _productoService.UpdateProductoAsync(dto);
            return Ok(producto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al actualizar producto", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _productoService.DeleteProductoAsync(id);
            if (!result)
                return NotFound(new { message = $"Producto con ID {id} no encontrado" });

            return Ok(new { message = "Producto eliminado correctamente" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al eliminar producto", error = ex.Message });
        }
    }
}
