using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaDeInventario.Domain.Interfaces;
using SistemaDeInventario.Infrastructure.Persistence;
using SistemaDeInventario.Infrastructure.Repositories;
using SistemaDeInventario.Application.Services;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Application.Services.Interfaces.UsuariosService;
using SistemaDeInventario.Application.Services.UsuariosService;
using SistemaDeInventario.Infrastructure.Repositories.UsuariosRepository;

namespace SistemaDeInventario.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar PostgreSQL DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsqlOptions => npgsqlOptions
                    .EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null)
            )
        );

        // Registrar Repositorios
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        services.AddScoped<IAlmacenRepository, AlmacenRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<IExistenciaRepository, ExistenciaRepository>();
        services.AddScoped<IMovimientoRepository, MovimientoRepository>();
        services.AddScoped<IDetalleMovimientoRepository, DetalleMovimientoRepository>();

        // Registrar Servicios de Aplicación
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<IProveedorService, ProveedorService>();
        services.AddScoped<IAlmacenService, AlmacenService>();
        services.AddScoped<IProductoService, ProductoService>();
        services.AddScoped<IExistenciaService, ExistenciaService>();
        services.AddScoped<IMovimientoService, MovimientoService>();

        return services;
    }
}
