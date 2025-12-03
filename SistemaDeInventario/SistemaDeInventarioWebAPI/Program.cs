

namespace SistemaDeInventarioWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = null; // Mantener PascalCase
                options.SerializerOptions.WriteIndented = true; // JSON formateado
            });

            // Agregar Infrastructure (DbContext con PostgreSQL)
            builder.Services.AddInfrastructure(builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Agregar Infrastructure (DbContext + Repositorios + Servicios)
            builder.Services.AddInfrastructure(builder.Configuration);

            // Agregar controladores con configuración JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // Configurar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Habilitar Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Inventario API v1");
                options.RoutePrefix = string.Empty;
            });

            // ========== ENDPOINT DE PRUEBA DE CONEXI�N A BD ==========
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
                        diagnosticInfo.Add("? No se pudo establecer conexi�n con la base de datos");
                        var errorResponse = new
                        {
                            success = false,
                            error = "No se pudo establecer conexi�n con la base de datos",
                            diagnosticLog = diagnosticInfo
                        };
                        return Results.Json(errorResponse, statusCode: 500);
                    }

                    diagnosticInfo.Add("? Conexi�n exitosa!");
                    diagnosticInfo.Add("?? Paso 3: Obteniendo estad�sticas de tablas...");
                    
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
                        message = "? Conexi�n exitosa a PostgreSQL",
                        database = "sistemainventariodb",
                        server = "dpg-d4mta6u3jp1c73a76lpg-a.oregon-postgres.render.com",
                        statistics = stats,
                        diagnosticLog = diagnosticInfo
                    };

                    return Results.Ok(successResponse);
                }
                catch (Exception ex)
                {
                    diagnosticInfo.Add($"? Error cr�tico: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        diagnosticInfo.Add($"   Inner Exception: {ex.InnerException.Message}");
                    }

                    // Imprimir diagn�stico en consola
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
            .WithDescription("Prueba detallada de la conexi�n a PostgreSQL con diagn�stico completo");

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

            app.UseHttpsRedirection();
            
            // CORS (opcional)
            app.UseCors(policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.MapControllers();

            app.Run();
        }
    }
}