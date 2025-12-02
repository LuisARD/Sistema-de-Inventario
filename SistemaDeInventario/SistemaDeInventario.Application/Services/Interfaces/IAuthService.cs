using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;

namespace SistemaDeInventario.Application.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    Task<bool> ValidateUserCredentialsAsync(string email, string password);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
