using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IRolService
{
    Task<IEnumerable<RolDto>> GetAllRolesAsync();
    Task<RolDto?> GetRolByIdAsync(int id);
    Task<RolDto> CreateRolAsync(CreateRolDto dto);
}
