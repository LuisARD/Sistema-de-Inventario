namespace SistemaDeInventario.Application.DTOs.Response.UsuariosDto;

public class RolDto
{
    public int RolId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}
