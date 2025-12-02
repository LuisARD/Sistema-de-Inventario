using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IRolRepository _rolRepository;
    private readonly IAuthService _authService;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IRolRepository rolRepository,
        IAuthService authService)
    {
        _usuarioRepository = usuarioRepository;
        _rolRepository = rolRepository;
        _authService = authService;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        return usuarios.Select(MapToDto);
    }

    public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        return usuario != null ? MapToDto(usuario) : null;
    }

    public async Task<UsuarioDto?> GetUsuarioByEmailAsync(string email)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(email);
        return usuario != null ? MapToDto(usuario) : null;
    }

    public async Task<UsuarioDto> CreateUsuarioAsync(CreateUsuarioDto dto)
    {
        var existente = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (existente != null)
            throw new InvalidOperationException($"Ya existe un usuario con el email: {dto.Email}");

        var rol = await _rolRepository.GetByIdAsync(dto.RolId);
        if (rol == null)
            throw new InvalidOperationException($"Rol con ID {dto.RolId} no encontrado");

        var usuario = new Usuario
        {
            NombreCompleto = dto.NombreCompleto,
            Email = dto.Email,
            PasswordHash = _authService.HashPassword(dto.Password),
            RolId = dto.RolId,
            FechaCreacion = DateTime.UtcNow,
            Activo = true
        };

        var creado = await _usuarioRepository.AddAsync(usuario);
        return MapToDto(creado);
    }

    public async Task<UsuarioDto> UpdateUsuarioAsync(UpdateUsuarioDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new InvalidOperationException($"Usuario con ID {dto.UsuarioId} no encontrado");

        var rol = await _rolRepository.GetByIdAsync(dto.RolId);
        if (rol == null)
            throw new InvalidOperationException($"Rol con ID {dto.RolId} no encontrado");

        // Verificar si el email cambió y si ya está en uso
        if (usuario.Email != dto.Email)
        {
            var existente = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (existente != null)
                throw new InvalidOperationException($"Ya existe un usuario con el email: {dto.Email}");
        }

        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.Email = dto.Email;
        usuario.RolId = dto.RolId;
        usuario.Activo = dto.Activo;

        await _usuarioRepository.UpdateAsync(usuario);
        return MapToDto(usuario);
    }

    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null)
            return false;

        usuario.Activo = false;
        await _usuarioRepository.UpdateAsync(usuario);
        return true;
    }

    public async Task<IEnumerable<UsuarioDto>> GetUsuariosActivosAsync()
    {
        var usuarios = await _usuarioRepository.GetUsuariosActivosAsync();
        return usuarios.Select(MapToDto);
    }

    private UsuarioDto MapToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            NombreCompleto = usuario.NombreCompleto,
            Email = usuario.Email,
            RolId = usuario.RolId,
            RolNombre = usuario.Rol?.Nombre ?? string.Empty,
            FechaCreacion = usuario.FechaCreacion,
            Activo = usuario.Activo
        };
    }
}
