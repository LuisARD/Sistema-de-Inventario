using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class AlmacenService : IAlmacenService
{
    private readonly IAlmacenRepository _almacenRepository;

    public AlmacenService(IAlmacenRepository almacenRepository)
    {
        _almacenRepository = almacenRepository;
    }

    public async Task<IEnumerable<AlmacenDto>> GetAllAlmacenesAsync()
    {
        var almacenes = await _almacenRepository.GetAllAsync();
        return almacenes.Select(MapToDto);
    }

    public async Task<IEnumerable<AlmacenDto>> GetAlmacenesActivosAsync()
    {
        var almacenes = await _almacenRepository.GetAlmacenesActivosAsync();
        return almacenes.Select(MapToDto);
    }

    public async Task<AlmacenDto?> GetAlmacenByIdAsync(int id)
    {
        var almacen = await _almacenRepository.GetByIdAsync(id);
        return almacen != null ? MapToDto(almacen) : null;
    }

    public async Task<AlmacenDto> CreateAlmacenAsync(CreateAlmacenDto dto)
    {
        var almacen = new Almacen
        {
            Nombre = dto.Nombre,
            Ubicacion = dto.Ubicacion,
            Activo = true
        };

        var creado = await _almacenRepository.AddAsync(almacen);
        return MapToDto(creado);
    }

    public async Task<AlmacenDto> UpdateAlmacenAsync(UpdateAlmacenDto dto)
    {
        var almacen = await _almacenRepository.GetByIdAsync(dto.AlmacenId);
        if (almacen == null)
            throw new InvalidOperationException($"Almacen con ID {dto.AlmacenId} no encontrado");

        almacen.Nombre = dto.Nombre;
        almacen.Ubicacion = dto.Ubicacion;
        almacen.Activo = dto.Activo;

        await _almacenRepository.UpdateAsync(almacen);
        return MapToDto(almacen);
    }

    public async Task<bool> DeleteAlmacenAsync(int id)
    {
        var almacen = await _almacenRepository.GetByIdAsync(id);
        if (almacen == null)
            return false;

        almacen.Activo = false;
        await _almacenRepository.UpdateAsync(almacen);
        return true;
    }

    private AlmacenDto MapToDto(Almacen almacen)
    {
        return new AlmacenDto
        {
            AlmacenId = almacen.AlmacenId,
            Nombre = almacen.Nombre,
            Ubicacion = almacen.Ubicacion,
            Activo = almacen.Activo
        };
    }
}
