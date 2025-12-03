using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.DTOs.Response.UsuariosDto;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Application.Services.Interfaces.UsuariosService;

namespace SistemaDeInventario.Application.Services.UsuariosService;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
        
        if (usuario == null || !usuario.Activo)
            return null;

        if (!VerifyPassword(dto.Password, usuario.PasswordHash))
            return null;

        // Generar token JWT
        var token = GenerateJwtToken(usuario);
        var expiracion = DateTime.UtcNow.AddHours(24); 

        return new LoginResponseDto
        {
            Token = token,
            Expiracion = expiracion,
            Usuario = new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre ?? string.Empty,
                FechaCreacion = usuario.FechaCreacion,
                Activo = usuario.Activo
            }
        };
    }

    public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(email);
        
        if (usuario == null || !usuario.Activo)
            return false;

        return VerifyPassword(password, usuario.PasswordHash);
    }

    public string HashPassword(string password)
    {
        // Usar BCrypt
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch
        {
            return false;
        }
    }

    private string GenerateJwtToken(Domain.Entities.Usuario usuario)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.NombreCompleto),
            new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Usuario"),
            new Claim("RolId", usuario.RolId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
