using SistemaDeInventario.Application.DTOs.Request.CreateDto;
using SistemaDeInventario.Application.DTOs.Request.UpdateDto;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class ProveedorService : IProveedorService
{
    private readonly IProveedorRepository _proveedorRepository;

    public ProveedorService(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<IEnumerable<ProveedorDto>> GetAllProveedoresAsync()
    {
        var proveedores = await _proveedorRepository.GetProveedoresConProductosAsync();
        return proveedores.Select(MapToDto);
    }

    public async Task<ProveedorDto?> GetProveedorByIdAsync(int id)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(id);
        return proveedor != null ? MapToDto(proveedor) : null;
    }

    public async Task<ProveedorDto> CreateProveedorAsync(CreateProveedorDto dto)
    {
        var existente = await _proveedorRepository.GetByNombreEmpresaAsync(dto.NombreEmpresa);
        if (existente != null)
            throw new InvalidOperationException($"Ya existe un proveedor con el nombre: {dto.NombreEmpresa}");

        var proveedor = new Proveedor
        {
            NombreEmpresa = dto.NombreEmpresa,
            NombreContacto = dto.NombreContacto,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion
        };

        var creado = await _proveedorRepository.AddAsync(proveedor);
        return MapToDto(creado);
    }

    public async Task<ProveedorDto> UpdateProveedorAsync(UpdateProveedorDto dto)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(dto.ProveedorId);
        if (proveedor == null)
            throw new InvalidOperationException($"Proveedor con ID {dto.ProveedorId} no encontrado");

        proveedor.NombreEmpresa = dto.NombreEmpresa;
        proveedor.NombreContacto = dto.NombreContacto;
        proveedor.Telefono = dto.Telefono;
        proveedor.Email = dto.Email;
        proveedor.Direccion = dto.Direccion;

        await _proveedorRepository.UpdateAsync(proveedor);
        return MapToDto(proveedor);
    }

    public async Task<bool> DeleteProveedorAsync(int id)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(id);
        if (proveedor == null)
            return false;

        // Validar que no tenga productos asociados
        if (proveedor.Productos.Any())
            throw new InvalidOperationException("No se puede eliminar un proveedor con productos asociados");

        await _proveedorRepository.DeleteAsync(id);
        return true;
    }

    private ProveedorDto MapToDto(Proveedor proveedor)
    {
        return new ProveedorDto
        {
            ProveedorId = proveedor.ProveedorId,
            NombreEmpresa = proveedor.NombreEmpresa,
            NombreContacto = proveedor.NombreContacto,
            Telefono = proveedor.Telefono,
            Email = proveedor.Email,
            Direccion = proveedor.Direccion,
            TotalProductos = proveedor.Productos?.Count ?? 0
        };
    }
}
