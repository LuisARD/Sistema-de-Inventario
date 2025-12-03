using SistemaDeInventario.Application.DTOs.Request.CreateDto;
using SistemaDeInventario.Application.DTOs.Request.UpdateDto;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IAlmacenService
{
    Task<IEnumerable<AlmacenDto>> GetAllAlmacenesAsync();
    Task<IEnumerable<AlmacenDto>> GetAlmacenesActivosAsync();
    Task<AlmacenDto?> GetAlmacenByIdAsync(int id);
    Task<AlmacenDto> CreateAlmacenAsync(CreateAlmacenDto dto);
    Task<AlmacenDto> UpdateAlmacenAsync(UpdateAlmacenDto dto);
    Task<bool> DeleteAlmacenAsync(int id);
}
