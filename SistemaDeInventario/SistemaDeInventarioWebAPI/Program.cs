using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using SistemaDeInventario.Infrastructure;
using SistemaDeInventario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace SistemaDeInventarioWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar JSON options
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
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
                try
                {
                    Console.WriteLine("?? Intentando conectar a PostgreSQL...");
                    
                    // Intentar conectar a la base de datos
                    var canConnect = await dbContext.Database.CanConnectAsync();

                    if (canConnect)
                    {
                        Console.WriteLine("? Conexión exitosa!");
                        
                        // Obtener información de la base de datos
                        var rolesCount = await dbContext.Roles.CountAsync();
                        var categoriasCount = await dbContext.Categorias.CountAsync();
                        var almacenesCount = await dbContext.Almacenes.CountAsync();
                        var productosCount = await dbContext.Productos.CountAsync();
                        var usuariosCount = await dbContext.Usuarios.CountAsync();
                        var proveedoresCount = await dbContext.Proveedores.CountAsync();
                        var existenciasCount = await dbContext.Existencias.CountAsync();
                        var movimientosCount = await dbContext.Movimientos.CountAsync();

                        return Results.Ok(new
                        {
                            success = true,
                            message = "? Conexión exitosa a PostgreSQL",
                            database = "sistemainventariodb",
                            server = "dpg-d4mta6u3jp1c73a76lpg-a.oregon-postgres.render.com",
                            statistics = new
                            {
                                roles = rolesCount,
                                categorias = categoriasCount,
                                almacenes = almacenesCount,
                                productos = productosCount,
                                usuarios = usuariosCount,
                                proveedores = proveedoresCount,
                                existencias = existenciasCount,
                                movimientos = movimientosCount
                            }
                        });
                    }
                    else
                    {
                        Console.WriteLine("? No se pudo conectar");
                        return Results.Problem(
                            detail: "No se pudo conectar a la base de datos",
                            statusCode: 500
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"? Error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"   Inner: {ex.InnerException.Message}");
                    }
                    
                    return Results.Problem(
                        detail: $"? Error al conectar: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}",
                        statusCode: 500
                    );
                }
            })
            .WithName("TestDatabaseConnection")
            .WithTags("Database")
            .WithDescription("Prueba la conexión a la base de datos PostgreSQL");

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

    [JsonSerializable(typeof(Todo[]))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {

    }
}