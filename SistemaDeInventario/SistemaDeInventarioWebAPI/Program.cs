using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using SistemaDeInventario.Infrastructure;
using SistemaDeInventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SistemaDeInventarioWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar JSON options SIN Source Generator
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null; // Mantener PascalCase
                options.SerializerOptions.WriteIndented = true; // JSON formateado
            });

            // Agregar Infrastructure (DbContext con PostgreSQL)
            builder.Services.AddInfrastructure(builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // ========== ENDPOINT DE PRUEBA DE CONEXIÓN A BD ==========
            app.MapGet("/api/test-connection", async (AppDbContext dbContext) =>
            {
                var diagnosticInfo = new List<string>();
                
                try
                {
                    diagnosticInfo.Add("?? Paso 1: Validando DbContext...");
                    
                    if (dbContext == null)
                    {
                        var errorResponse = new
                        {
                            success = false,
                            error = "DbContext no se pudo inyectar correctamente",
                            diagnosticLog = diagnosticInfo
                        };
                        return Results.Json(errorResponse, statusCode: 500);
                    }

                    diagnosticInfo.Add("? DbContext inyectado correctamente");
                    diagnosticInfo.Add("?? Paso 2: Intentando conectar a PostgreSQL...");
                    
                    var canConnect = await dbContext.Database.CanConnectAsync();

                    if (!canConnect)
                    {
                        diagnosticInfo.Add("? No se pudo establecer conexión con la base de datos");
                        var errorResponse = new
                        {
                            success = false,
                            error = "No se pudo establecer conexión con la base de datos",
                            diagnosticLog = diagnosticInfo
                        };
                        return Results.Json(errorResponse, statusCode: 500);
                    }

                    diagnosticInfo.Add("? Conexión exitosa!");
                    diagnosticInfo.Add("?? Paso 3: Obteniendo estadísticas de tablas...");
                    
                    // Intentar obtener conteos de cada tabla individualmente
                    var stats = new Dictionary<string, object>();

                    try
                    {
                        stats["roles"] = await dbContext.Roles.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Roles' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["roles"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Roles': {ex.Message}");
                    }

                    try
                    {
                        stats["categorias"] = await dbContext.Categorias.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Categorias' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["categorias"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Categorias': {ex.Message}");
                    }

                    try
                    {
                        stats["almacenes"] = await dbContext.Almacenes.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Almacenes' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["almacenes"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Almacenes': {ex.Message}");
                    }

                    try
                    {
                        stats["productos"] = await dbContext.Productos.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Productos' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["productos"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Productos': {ex.Message}");
                    }

                    try
                    {
                        stats["usuarios"] = await dbContext.Usuarios.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Usuarios' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["usuarios"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Usuarios': {ex.Message}");
                    }

                    try
                    {
                        stats["proveedores"] = await dbContext.Proveedores.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Proveedores' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["proveedores"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Proveedores': {ex.Message}");
                    }

                    try
                    {
                        stats["existencias"] = await dbContext.Existencias.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Existencias' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["existencias"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Existencias': {ex.Message}");
                    }

                    try
                    {
                        stats["movimientos"] = await dbContext.Movimientos.CountAsync();
                        diagnosticInfo.Add("   ? Tabla 'Movimientos' accedida correctamente");
                    }
                    catch (Exception ex)
                    {
                        stats["movimientos"] = $"Error: {ex.Message}";
                        diagnosticInfo.Add($"   ? Error en tabla 'Movimientos': {ex.Message}");
                    }

                    diagnosticInfo.Add("? Prueba completada!");

                    // Imprimir todo en la consola
                    foreach (var info in diagnosticInfo)
                    {
                        Console.WriteLine(info);
                    }

                    var successResponse = new
                    {
                        success = true,
                        message = "? Conexión exitosa a PostgreSQL",
                        database = "sistemainventariodb",
                        server = "dpg-d4mta6u3jp1c73a76lpg-a.oregon-postgres.render.com",
                        statistics = stats,
                        diagnosticLog = diagnosticInfo
                    };

                    return Results.Ok(successResponse);
                }
                catch (Exception ex)
                {
                    diagnosticInfo.Add($"? Error crítico: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        diagnosticInfo.Add($"   Inner Exception: {ex.InnerException.Message}");
                    }

                    // Imprimir diagnóstico en consola
                    foreach (var info in diagnosticInfo)
                    {
                        Console.WriteLine(info);
                    }
                    
                    var errorResponse = new
                    {
                        success = false,
                        error = ex.Message,
                        innerError = ex.InnerException?.Message,
                        diagnosticLog = diagnosticInfo
                    };

                    return Results.Json(errorResponse, statusCode: 500);
                }
            })
            .WithName("TestDatabaseConnection")
            .WithTags("Database")
            .WithDescription("Prueba detallada de la conexión a PostgreSQL con diagnóstico completo");

            // ========== ENDPOINTS DE EJEMPLO (TODO) ==========
            Todo[] sampleTodos =
    
            [
                new(1, "Walk the dog"),
                new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
                new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
                new(4, "Clean the bathroom"),
                new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
            ];

            var todosApi = app.MapGroup("/todos");
            todosApi.MapGet("/", () => sampleTodos)
                    .WithName("GetTodos");

            todosApi.MapGet("/{id}", Results<Ok<Todo>, NotFound> (int id) =>
                sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound())
                .WithName("GetTodoById");

            app.Run();
        }
    }

    public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);
}