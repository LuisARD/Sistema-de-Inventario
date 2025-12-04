using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using SistemaDeInventario.Infrastructure;
using System.Security.Claims;
using System.Text;

namespace SistemaDeInventarioWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null;
                options.SerializerOptions.WriteIndented = true;
            });

            builder.Services.AddInfrastructure(builder.Configuration);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey ?? string.Empty)),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("AdminOrSupervisor", policy => policy.RequireRole("Admin", "Supervisor"));
                options.AddPolicy("UsuariosRead", policy => policy.RequireRole("Admin", "Supervisor"));
                options.AddPolicy("CategoriasRead", policy => policy.RequireRole("Admin", "Usuario", "Supervisor"));
                options.AddPolicy("CategoriasWrite", policy => policy.RequireRole("Admin", "Supervisor"));
                options.AddPolicy("MovimientosRead", policy => policy.RequireRole("Admin", "Usuario", "Supervisor"));
                options.AddPolicy("MovimientosWrite", policy => policy.RequireRole("Admin", "Supervisor"));
                options.AddPolicy("ProductosRead", policy => policy.RequireRole("Admin", "Usuario", "Supervisor"));
                options.AddPolicy("ProductosWrite", policy => policy.RequireRole("Admin", "Usuario", "Supervisor"));
                options.AddPolicy("AlmacenesAccess", policy => policy.RequireRole("Admin", "Supervisor"));
                options.AddPolicy("ProveedoresAccess", policy => policy.RequireRole("Admin", "Supervisor"));
                options.AddPolicy("ExistenciasAccess", policy => policy.RequireRole("Admin", "Supervisor"));
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // NSwag: documento OpenAPI y seguridad JWT
            builder.Services.AddOpenApiDocument(config =>
            {
                config.Title = "Sistema de Inventario API";
                config.Version = "v1";

                config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Description = "Introduce: Bearer tu_token"
                });

                config.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            var app = builder.Build();

            // NSwag middlewares
            app.UseOpenApi(); // /swagger/v1/swagger.json
            app.UseSwaggerUi(settings =>
            {
                settings.DocumentPath = "/swagger/v1/swagger.json";
                settings.Path = ""; // UI en raíz
            });

            app.UseHttpsRedirection();

            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}