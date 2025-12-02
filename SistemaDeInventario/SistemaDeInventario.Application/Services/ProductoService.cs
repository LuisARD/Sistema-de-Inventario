using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IProveedorRepository _proveedorRepository;

    public ProductoService(
        IProductoRepository productoRepository,
        ICategoriaRepository categoriaRepository,
        IProveedorRepository proveedorRepository)
    {
        _productoRepository = productoRepository;
        _categoriaRepository = categoriaRepository;
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<ProductoDto>> GetAllProductosAsync()
    {
        var productos = await _productoRepository.GetAllAsync();
        return productos.Select(MapToDto);
    }

    public async Task<ProductoDto?> GetProductoByIdAsync(int id)
    {
        var producto = await _productoRepository.GetProductoConDetallesAsync(id);
        return producto != null ? MapToDto(producto) : null;
    }

    public async Task<ProductoDto?> GetProductoBySkuAsync(string codigoSku)
    {
        var producto = await _productoRepository.GetByCodigoSkuAsync(codigoSku);
        return producto != null ? MapToDto(producto) : null;
    }

    public async Task<ProductoDto> CreateProductoAsync(CreateProductoDto dto)
    {
        // Validar que no exista el SKU
        var existente = await _productoRepository.GetByCodigoSkuAsync(dto.CodigoSku);
        if (existente != null)
            throw new InvalidOperationException($"Ya existe un producto con el SKU: {dto.CodigoSku}");

        // Validar categoría y proveedor
        var categoria = await _categoriaRepository.GetByIdAsync(dto.CategoriaId);
        if (categoria == null)
            throw new InvalidOperationException($"Categoría con ID {dto.CategoriaId} no encontrada");

        var proveedor = await _proveedorRepository.GetByIdAsync(dto.ProveedorId);
        if (proveedor == null)
            throw new InvalidOperationException($"Proveedor con ID {dto.ProveedorId} no encontrado");

        var producto = new Producto
        {
            CodigoSku = dto.CodigoSku,
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            CategoriaId = dto.CategoriaId,
            ProveedorId = dto.ProveedorId,
            PrecioCompra = dto.PrecioCompra,
            PrecioVenta = dto.PrecioVenta,
            UnidadMedida = dto.UnidadMedida,
            StockMinimo = dto.StockMinimo,
            Activo = true,
            FechaCreacion = DateTime.UtcNow
        };

        var creado = await _productoRepository.AddAsync(producto);
        return MapToDto(creado);
    }

    public async Task<ProductoDto> UpdateProductoAsync(UpdateProductoDto dto)
    {
        var producto = await _productoRepository.GetByIdAsync(dto.ProductoId);
        if (producto == null)
            throw new InvalidOperationException($"Producto con ID {dto.ProductoId} no encontrado");

        // Validar categoría y proveedor
        var categoria = await _categoriaRepository.GetByIdAsync(dto.CategoriaId);
        if (categoria == null)
            throw new InvalidOperationException($"Categoría con ID {dto.CategoriaId} no encontrada");

        var proveedor = await _proveedorRepository.GetByIdAsync(dto.ProveedorId);
        if (proveedor == null)
            throw new InvalidOperationException($"Proveedor con ID {dto.ProveedorId} no encontrado");

        producto.Nombre = dto.Nombre;
        producto.Descripcion = dto.Descripcion;
        producto.CategoriaId = dto.CategoriaId;
        producto.ProveedorId = dto.ProveedorId;
        producto.PrecioCompra = dto.PrecioCompra;
        producto.PrecioVenta = dto.PrecioVenta;
        producto.UnidadMedida = dto.UnidadMedida;
        producto.StockMinimo = dto.StockMinimo;
        producto.Activo = dto.Activo;

        await _productoRepository.UpdateAsync(producto);
        return MapToDto(producto);
    }

    public async Task<bool> DeleteProductoAsync(int id)
    {
        var producto = await _productoRepository.GetByIdAsync(id);
        if (producto == null)
            return false;

        producto.Activo = false;
        await _productoRepository.UpdateAsync(producto);
        return true;
    }

    public async Task<IEnumerable<ProductoDto>> GetProductosByCategoriaAsync(int categoriaId)
    {
        var productos = await _productoRepository.GetProductosByCategoriaAsync(categoriaId);
        return productos.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductoDto>> GetProductosByProveedorAsync(int proveedorId)
    {
        var productos = await _productoRepository.GetProductosByProveedorAsync(proveedorId);
        return productos.Select(MapToDto);
    }

    private ProductoDto MapToDto(Producto producto)
    {
        return new ProductoDto
        {
            ProductoId = producto.ProductoId,
            CodigoSku = producto.CodigoSku,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            CategoriaId = producto.CategoriaId,
            CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty,
            ProveedorId = producto.ProveedorId,
            ProveedorNombre = producto.Proveedor?.NombreEmpresa ?? string.Empty,
            PrecioCompra = producto.PrecioCompra,
            PrecioVenta = producto.PrecioVenta,
            UnidadMedida = producto.UnidadMedida,
            StockMinimo = producto.StockMinimo,
            Activo = producto.Activo,
            FechaCreacion = producto.FechaCreacion
        };
    }
}
