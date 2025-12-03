using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response.UsuariosDto;

namespace SistemaDeInventario.Application.Services.Interfaces.UsuariosService;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();
    Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
    Task<UsuarioDto?> GetUsuarioByEmailAsync(string email);
    Task<UsuarioDto> CreateUsuarioAsync(CreateUsuarioDto dto);
    Task<UsuarioDto> UpdateUsuarioAsync(UpdateUsuarioDto dto);
    Task<bool> DeleteUsuarioAsync(int id);
    Task<IEnumerable<UsuarioDto>> GetUsuariosActivosAsync();
}
