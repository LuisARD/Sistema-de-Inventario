using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IProveedorService
{
    Task<IEnumerable<ProveedorDto>> GetAllProveedoresAsync();
    Task<ProveedorDto?> GetProveedorByIdAsync(int id);
    Task<ProveedorDto> CreateProveedorAsync(CreateProveedorDto dto);
    Task<ProveedorDto> UpdateProveedorAsync(UpdateProveedorDto dto);
    Task<bool> DeleteProveedorAsync(int id);
}
