using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;

    public RolService(IRolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
    {
        var roles = await _rolRepository.GetAllAsync();
        return roles.Select(MapToDto);
    }

    public async Task<RolDto?> GetRolByIdAsync(int id)
    {
        var rol = await _rolRepository.GetByIdAsync(id);
        return rol != null ? MapToDto(rol) : null;
    }

    public async Task<RolDto> CreateRolAsync(CreateRolDto dto)
    {
        var existente = await _rolRepository.GetByNombreAsync(dto.Nombre);
        if (existente != null)
            throw new InvalidOperationException($"Ya existe un rol con el nombre: {dto.Nombre}");

        var rol = new Rol
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        var creado = await _rolRepository.AddAsync(rol);
        return MapToDto(creado);
    }

    private RolDto MapToDto(Rol rol)
    {
        return new RolDto
        {
            RolId = rol.RolId,
            Nombre = rol.Nombre,
            Descripcion = rol.Descripcion
        };
    }
}
