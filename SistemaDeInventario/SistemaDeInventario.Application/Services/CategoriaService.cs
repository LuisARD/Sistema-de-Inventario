using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<CategoriaDto>> GetAllCategoriasAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasConProductosAsync();
        return categorias.Select(MapToDto);
    }

    public async Task<CategoriaDto?> GetCategoriaByIdAsync(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        return categoria != null ? MapToDto(categoria) : null;
    }

    public async Task<CategoriaDto> CreateCategoriaAsync(CreateCategoriaDto dto)
    {
        var existente = await _categoriaRepository.GetByNombreAsync(dto.Nombre);
        if (existente != null)
            throw new InvalidOperationException($"Ya existe una categoría con el nombre: {dto.Nombre}");

        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        var creada = await _categoriaRepository.AddAsync(categoria);
        return MapToDto(creada);
    }

    public async Task<CategoriaDto> UpdateCategoriaAsync(UpdateCategoriaDto dto)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(dto.CategoriaId);
        if (categoria == null)
            throw new InvalidOperationException($"Categoría con ID {dto.CategoriaId} no encontrada");

        categoria.Nombre = dto.Nombre;
        categoria.Descripcion = dto.Descripcion;

        await _categoriaRepository.UpdateAsync(categoria);
        return MapToDto(categoria);
    }

    public async Task<bool> DeleteCategoriaAsync(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);
        if (categoria == null)
            return false;

        // Validar que no tenga productos asociados
        if (categoria.Productos.Any())
            throw new InvalidOperationException("No se puede eliminar una categoría con productos asociados");

        await _categoriaRepository.DeleteAsync(id);
        return true;
    }

    private CategoriaDto MapToDto(Categoria categoria)
    {
        return new CategoriaDto
        {
            CategoriaId = categoria.CategoriaId,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            TotalProductos = categoria.Productos?.Count ?? 0
        };
    }
}
