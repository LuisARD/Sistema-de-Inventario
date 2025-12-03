using SistemaDeInventario.Application.DTOs.Request.CreateDto;
using SistemaDeInventario.Application.DTOs.Request.UpdateDto;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaDto>> GetAllCategoriasAsync();
    Task<CategoriaDto?> GetCategoriaByIdAsync(int id);
    Task<CategoriaDto> CreateCategoriaAsync(CreateCategoriaDto dto);
    Task<CategoriaDto> UpdateCategoriaAsync(UpdateCategoriaDto dto);
    Task<bool> DeleteCategoriaAsync(int id);
}
