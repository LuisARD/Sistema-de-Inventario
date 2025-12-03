using SistemaDeInventario.Application.DTOs.Response.UsuariosDto;

namespace SistemaDeInventario.Application.DTOs.Response;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDto Usuario { get; set; } = null!;
    public DateTime Expiracion { get; set; }
}
