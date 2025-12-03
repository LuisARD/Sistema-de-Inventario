using SistemaDeInventario.Application.DTOs.Request.CreateDto;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IMovimientoService
{
    Task<MovimientoDto> RegistrarEntradaAsync(CreateMovimientoDto dto);
    Task<MovimientoDto> RegistrarSalidaAsync(CreateMovimientoDto dto);
    Task<MovimientoDto?> GetMovimientoByIdAsync(int id);
    Task<IEnumerable<MovimientoDto>> GetAllMovimientosAsync();
    Task<IEnumerable<MovimientoDto>> GetMovimientosByTipoAsync(string tipo);
    Task<IEnumerable<MovimientoDto>> GetMovimientosByUsuarioAsync(int usuarioId);
    Task<IEnumerable<MovimientoDto>> GetMovimientosByFechaAsync(DateTime fechaInicio, DateTime fechaFin);
    Task<IEnumerable<MovimientoDto>> GetMovimientosByAlmacenAsync(int almacenId);
}
