namespace SistemaDeInventario.Application.DTOs.Request;

public class UpdateUsuarioDto
{
    public int UsuarioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int RolId { get; set; }
    public bool Activo { get; set; }
}
