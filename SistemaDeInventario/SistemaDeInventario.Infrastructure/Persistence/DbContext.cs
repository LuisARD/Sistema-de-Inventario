using Microsoft.EntityFrameworkCore;
using SistemaDeInventario.Domain.Entities;

namespace SistemaDeInventario.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Almacen> Almacenes { get; set; }
    public DbSet<Existencia> Existencias { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
    public DbSet<DetalleMovimiento> DetalleMovimientos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        // Configuración de Proveedor
        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.ProveedorId);
            entity.Property(e => e.NombreEmpresa).IsRequired().HasMaxLength(200);
            entity.Property(e => e.NombreContacto).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Direccion).HasMaxLength(300);
            entity.HasIndex(e => e.NombreEmpresa).IsUnique();
        });

        // Configuración de Almacen
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.AlmacenId);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Ubicacion).HasMaxLength(300);
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);
        });

        // Configuración de Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId);
            entity.Property(e => e.CodigoSku).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.PrecioCompra).HasPrecision(18, 2);
            entity.Property(e => e.PrecioVenta).HasPrecision(18, 2);
            entity.Property(e => e.UnidadMedida).HasMaxLength(50).HasDefaultValue("Unidad");
            entity.Property(e => e.StockMinimo).HasDefaultValue(5);
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasIndex(e => e.CodigoSku).IsUnique();

            // Relación con Categoria
            entity.HasOne(e => e.Categoria)
                  .WithMany(c => c.Productos)
                  .HasForeignKey(e => e.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con Proveedor
            entity.HasOne(e => e.Proveedor)
                  .WithMany(p => p.Productos)
                  .HasForeignKey(e => e.ProveedorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Existencia
        modelBuilder.Entity<Existencia>(entity =>
        {
            entity.HasKey(e => e.ExistenciaId);
            entity.Property(e => e.CantidadActual).IsRequired().HasDefaultValue(0);
            entity.Property(e => e.UbicacionPasillo).HasMaxLength(50);
            entity.Property(e => e.UltimaActualizacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Índice único para ProductoId + AlmacenId (no puede haber duplicados)
            entity.HasIndex(e => new { e.ProductoId, e.AlmacenId }).IsUnique();

            // Relación con Producto
            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.Existencias)
                  .HasForeignKey(e => e.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con Almacen
            entity.HasOne(e => e.Almacen)
                  .WithMany(a => a.Existencias)
                  .HasForeignKey(e => e.AlmacenId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId);
            entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Activo).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.Email).IsUnique();

            // Relación con Rol
            entity.HasOne(e => e.Rol)
                  .WithMany(r => r.Usuarios)
                  .HasForeignKey(e => e.RolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Movimiento
        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.MovimientoId);
            entity.Property(e => e.FechaMovimiento).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TipoMovimiento).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Motivo).HasMaxLength(500);
            entity.Property(e => e.ReferenciaDocumento).HasMaxLength(100);

            // Relación con Usuario
            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Movimientos)
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con Almacen Origen
            entity.HasOne(e => e.AlmacenOrigen)
                  .WithMany(a => a.MovimientosOrigen)
                  .HasForeignKey(e => e.AlmacenOrigenId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Relación con Almacen Destino (Opcional)
            entity.HasOne(e => e.AlmacenDestino)
                  .WithMany(a => a.MovimientosDestino)
                  .HasForeignKey(e => e.AlmacenDestinoId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        // Configuración de DetalleMovimiento
        modelBuilder.Entity<DetalleMovimiento>(entity =>
        {
            entity.HasKey(e => e.DetalleId);
            entity.Property(e => e.Cantidad).IsRequired();
            entity.Property(e => e.CostoUnitarioHistorico).HasPrecision(18, 2);

            // Relación con Movimiento
            entity.HasOne(e => e.Movimiento)
                  .WithMany(m => m.DetalleMovimientos)
                  .HasForeignKey(e => e.MovimientoId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Relación con Producto
            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.DetalleMovimientos)
                  .HasForeignKey(e => e.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Datos iniciales (Seed Data)
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Roles
        modelBuilder.Entity<Rol>().HasData(
            new Rol { RolId = 1, Nombre = "Administrador", Descripcion = "Acceso completo al sistema" },
            new Rol { RolId = 2, Nombre = "Gerente", Descripcion = "Gestión de inventario y reportes" },
            new Rol { RolId = 3, Nombre = "Operador", Descripcion = "Registro de movimientos" },
            new Rol { RolId = 4, Nombre = "Consultor", Descripcion = "Solo consulta de información" }
        );

        // Seed Categorias
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { CategoriaId = 1, Nombre = "Electrónica", Descripcion = "Productos electrónicos" },
            new Categoria { CategoriaId = 2, Nombre = "Hogar", Descripcion = "Artículos para el hogar" },
            new Categoria { CategoriaId = 3, Nombre = "Oficina", Descripcion = "Suministros de oficina" }
        );

        // Seed Almacenes
        modelBuilder.Entity<Almacen>().HasData(
            new Almacen { AlmacenId = 1, Nombre = "Almacén Central", Ubicacion = "Bodega Principal - Piso 1", Activo = true },
            new Almacen { AlmacenId = 2, Nombre = "Almacén Secundario", Ubicacion = "Bodega 2 - Piso 2", Activo = true }
        );
    }

    // DbSets
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Almacen> Almacenes { get; set; }
    public DbSet<Existencia> Existencias { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
    public DbSet<DetalleMovimiento> DetalleMovimientos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.RolId);
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
            entity.HasIndex(e => e.Nombre).IsUnique();
        });

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuarios");
            entity.HasKey(e => e.UsuarioId);
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.NombreCompleto).HasColumnName("nombre_completo").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);

            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.Rol)
                  .WithMany(r => r.Usuarios)
                  .HasForeignKey(e => e.RolId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Categoria
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(e => e.CategoriaId);
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
        });

        // Configuración de Proveedor
        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("proveedores");
            entity.HasKey(e => e.ProveedorId);
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.NombreEmpresa).HasColumnName("nombre_empresa").IsRequired().HasMaxLength(100);
            entity.Property(e => e.NombreContacto).HasColumnName("nombre_contacto").HasMaxLength(100);
            entity.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(20);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
            entity.Property(e => e.Direccion).HasColumnName("direccion").HasColumnType("text");
        });

        // Configuración de Almacen
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.ToTable("almacenes");
            entity.HasKey(e => e.AlmacenId);
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id");
            entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Ubicacion).HasColumnName("ubicacion").HasMaxLength(255);
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
        });

        // Configuración de Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("productos");
            entity.HasKey(e => e.ProductoId);
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.CodigoSku).HasColumnName("codigo_sku").IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasColumnType("text");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.PrecioCompra).HasColumnName("precio_compra").HasPrecision(12, 2);
            entity.Property(e => e.PrecioVenta).HasColumnName("precio_venta").HasPrecision(12, 2);
            entity.Property(e => e.UnidadMedida).HasColumnName("unidad_medida").HasMaxLength(20).HasDefaultValue("Unidad");
            entity.Property(e => e.StockMinimo).HasColumnName("stock_minimo").HasDefaultValue(5);
            entity.Property(e => e.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasIndex(e => e.CodigoSku).IsUnique();

            entity.HasOne(e => e.Categoria)
                  .WithMany(c => c.Productos)
                  .HasForeignKey(e => e.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Proveedor)
                  .WithMany(p => p.Productos)
                  .HasForeignKey(e => e.ProveedorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Existencia
        modelBuilder.Entity<Existencia>(entity =>
        {
            entity.ToTable("existencias");
            entity.HasKey(e => e.ExistenciaId);
            entity.Property(e => e.ExistenciaId).HasColumnName("existencia_id");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.AlmacenId).HasColumnName("almacen_id");
            entity.Property(e => e.CantidadActual).HasColumnName("cantidad_actual").IsRequired().HasDefaultValue(0);
            entity.Property(e => e.UbicacionPasillo).HasColumnName("ubicacion_pasillo").HasMaxLength(50);
            entity.Property(e => e.UltimaActualizacion).HasColumnName("ultima_actualizacion").HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => new { e.ProductoId, e.AlmacenId }).IsUnique().HasDatabaseName("uk_producto_almacen");

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.Existencias)
                  .HasForeignKey(e => e.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Almacen)
                  .WithMany(a => a.Existencias)
                  .HasForeignKey(e => e.AlmacenId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Movimiento
        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.ToTable("movimientos");
            entity.HasKey(e => e.MovimientoId);
            entity.Property(e => e.MovimientoId).HasColumnName("movimiento_id");
            entity.Property(e => e.FechaMovimiento).HasColumnName("fecha_movimiento").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TipoMovimiento).HasColumnName("tipo_movimiento").IsRequired().HasMaxLength(20);
            entity.Property(e => e.Motivo).HasColumnName("motivo").HasMaxLength(255);
            entity.Property(e => e.ReferenciaDocumento).HasColumnName("referencia_documento").HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.AlmacenOrigenId).HasColumnName("almacen_origen_id");
            entity.Property(e => e.AlmacenDestinoId).HasColumnName("almacen_destino_id");

            entity.HasOne(e => e.Usuario)
                  .WithMany(u => u.Movimientos)
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AlmacenOrigen)
                  .WithMany(a => a.MovimientosOrigen)
                  .HasForeignKey(e => e.AlmacenOrigenId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AlmacenDestino)
                  .WithMany(a => a.MovimientosDestino)
                  .HasForeignKey(e => e.AlmacenDestinoId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });

        // Configuración de DetalleMovimiento
        modelBuilder.Entity<DetalleMovimiento>(entity =>
        {
            entity.ToTable("detalle_movimientos");
            entity.HasKey(e => e.DetalleId);
            entity.Property(e => e.DetalleId).HasColumnName("detalle_id");
            entity.Property(e => e.MovimientoId).HasColumnName("movimiento_id");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad").IsRequired();
            entity.Property(e => e.CostoUnitarioHistorico).HasColumnName("costo_unitario_historico").HasPrecision(12, 2);

            entity.HasOne(e => e.Movimiento)
                  .WithMany(m => m.DetalleMovimientos)
                  .HasForeignKey(e => e.MovimientoId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.DetalleMovimientos)
                  .HasForeignKey(e => e.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
