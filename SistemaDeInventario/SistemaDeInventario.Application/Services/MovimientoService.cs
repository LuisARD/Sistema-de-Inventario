using SistemaDeInventario.Application.DTOs.Request;
using SistemaDeInventario.Application.DTOs.Response;
using SistemaDeInventario.Application.Services.Interfaces;
using SistemaDeInventario.Domain.Entities;
using SistemaDeInventario.Domain.Interfaces;

namespace SistemaDeInventario.Application.Services;

public class MovimientoService : IMovimientoService
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly IDetalleMovimientoRepository _detalleMovimientoRepository;
    private readonly IExistenciaRepository _existenciaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAlmacenRepository _almacenRepository;

    public MovimientoService(
        IMovimientoRepository movimientoRepository,
        IDetalleMovimientoRepository detalleMovimientoRepository,
        IExistenciaRepository existenciaRepository,
        IProductoRepository productoRepository,
        IUsuarioRepository usuarioRepository,
        IAlmacenRepository almacenRepository)
    {
        _movimientoRepository = movimientoRepository;
        _detalleMovimientoRepository = detalleMovimientoRepository;
        _existenciaRepository = existenciaRepository;
        _productoRepository = productoRepository;
        _usuarioRepository = usuarioRepository;
        _almacenRepository = almacenRepository;
    }

    public async Task<MovimientoDto> RegistrarEntradaAsync(CreateMovimientoDto dto)
    {
        dto.TipoMovimiento = "ENTRADA";
        return await RegistrarMovimientoAsync(dto);
    }

    public async Task<MovimientoDto> RegistrarSalidaAsync(CreateMovimientoDto dto)
    {
        dto.TipoMovimiento = "SALIDA";
        
        // Validar que haya stock suficiente para salidas
        foreach (var detalle in dto.Detalles)
        {
            var existencia = await _existenciaRepository.GetByProductoYAlmacenAsync(
                detalle.ProductoId, dto.AlmacenOrigenId);
            
            if (existencia == null || existencia.CantidadActual < detalle.Cantidad)
            {
                var producto = await _productoRepository.GetByIdAsync(detalle.ProductoId);
                throw new InvalidOperationException(
                    $"Stock insuficiente para el producto '{producto?.Nombre}'. " +
                    $"Disponible: {existencia?.CantidadActual ?? 0}, Solicitado: {detalle.Cantidad}");
            }
        }

        return await RegistrarMovimientoAsync(dto);
    }

    private async Task<MovimientoDto> RegistrarMovimientoAsync(CreateMovimientoDto dto)
    {
        // Validar usuario y almacen
        var usuario = await _usuarioRepository.GetByIdAsync(dto.UsuarioId);
        if (usuario == null)
            throw new InvalidOperationException($"Usuario con ID {dto.UsuarioId} no encontrado");

        var almacenOrigen = await _almacenRepository.GetByIdAsync(dto.AlmacenOrigenId);
        if (almacenOrigen == null)
            throw new InvalidOperationException($"Almacen origen con ID {dto.AlmacenOrigenId} no encontrado");

        if (dto.AlmacenDestinoId.HasValue)
        {
            var almacenDestino = await _almacenRepository.GetByIdAsync(dto.AlmacenDestinoId.Value);
            if (almacenDestino == null)
                throw new InvalidOperationException($"Almacen destino con ID {dto.AlmacenDestinoId} no encontrado");
        }

        // Crear movimiento
        var movimiento = new Movimiento
        {
            FechaMovimiento = DateTime.UtcNow,
            TipoMovimiento = dto.TipoMovimiento,
            Motivo = dto.Motivo,
            ReferenciaDocumento = dto.ReferenciaDocumento,
            UsuarioId = dto.UsuarioId,
            AlmacenOrigenId = dto.AlmacenOrigenId,
            AlmacenDestinoId = dto.AlmacenDestinoId
        };

        var movimientoCreado = await _movimientoRepository.AddAsync(movimiento);

        // Crear detalles y actualizar existencias
        foreach (var detalleDto in dto.Detalles)
        {
            // Validar producto
            var producto = await _productoRepository.GetByIdAsync(detalleDto.ProductoId);
            if (producto == null)
                throw new InvalidOperationException($"Producto con ID {detalleDto.ProductoId} no encontrado");

            // Crear detalle
            var detalle = new DetalleMovimiento
            {
                MovimientoId = movimientoCreado.MovimientoId,
                ProductoId = detalleDto.ProductoId,
                Cantidad = detalleDto.Cantidad,
                CostoUnitarioHistorico = detalleDto.CostoUnitarioHistorico
            };

            await _detalleMovimientoRepository.AddAsync(detalle);

            // **ACTUALIZAR EXISTENCIAS AUTOMÁTICAMENTE**
            await ActualizarExistenciasAsync(
                detalleDto.ProductoId,
                dto.AlmacenOrigenId,
                dto.TipoMovimiento,
                detalleDto.Cantidad);
        }

        // Retornar el movimiento completo
        var movimientoCompleto = await _movimientoRepository.GetMovimientoConDetallesAsync(movimientoCreado.MovimientoId);
        return MapToDto(movimientoCompleto!);
    }

    private async Task ActualizarExistenciasAsync(int productoId, int almacenId, string tipoMovimiento, int cantidad)
    {
        var existencia = await _existenciaRepository.GetByProductoYAlmacenAsync(productoId, almacenId);

        if (existencia == null)
        {
            // Crear nueva existencia si no existe
            existencia = new Existencia
            {
                ProductoId = productoId,
                AlmacenId = almacenId,
                CantidadActual = 0,
                UltimaActualizacion = DateTime.UtcNow
            };
            await _existenciaRepository.AddAsync(existencia);
        }

        // Actualizar cantidad segun tipo de movimiento
        if (tipoMovimiento == "ENTRADA")
        {
            existencia.CantidadActual += cantidad;
        }
        else if (tipoMovimiento == "SALIDA")
        {
            existencia.CantidadActual -= cantidad;
            
            if (existencia.CantidadActual < 0)
            {
                var producto = await _productoRepository.GetByIdAsync(productoId);
                throw new InvalidOperationException(
                    $"La salida genera stock negativo para el producto '{producto?.Nombre}'");
            }
        }

        existencia.UltimaActualizacion = DateTime.UtcNow;
        await _existenciaRepository.UpdateAsync(existencia);
    }

    public async Task<MovimientoDto?> GetMovimientoByIdAsync(int id)
    {
        var movimiento = await _movimientoRepository.GetMovimientoConDetallesAsync(id);
        return movimiento != null ? MapToDto(movimiento) : null;
    }

    public async Task<IEnumerable<MovimientoDto>> GetAllMovimientosAsync()
    {
        var movimientos = await _movimientoRepository.GetAllAsync();
        return movimientos.Select(MapToDto);
    }

    public async Task<IEnumerable<MovimientoDto>> GetMovimientosByTipoAsync(string tipo)
    {
        var movimientos = await _movimientoRepository.GetMovimientosByTipoAsync(tipo);
        return movimientos.Select(MapToDto);
    }

    public async Task<IEnumerable<MovimientoDto>> GetMovimientosByUsuarioAsync(int usuarioId)
    {
        var movimientos = await _movimientoRepository.GetMovimientosByUsuarioAsync(usuarioId);
        return movimientos.Select(MapToDto);
    }

    public async Task<IEnumerable<MovimientoDto>> GetMovimientosByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
    {
        var movimientos = await _movimientoRepository.GetMovimientosByFechaAsync(fechaInicio, fechaFin);
        return movimientos.Select(MapToDto);
    }

    public async Task<IEnumerable<MovimientoDto>> GetMovimientosByAlmacenAsync(int almacenId)
    {
        var movimientos = await _movimientoRepository.GetMovimientosByAlmacenAsync(almacenId);
        return movimientos.Select(MapToDto);
    }

    private MovimientoDto MapToDto(Movimiento movimiento)
    {
        return new MovimientoDto
        {
            MovimientoId = movimiento.MovimientoId,
            FechaMovimiento = movimiento.FechaMovimiento,
            TipoMovimiento = movimiento.TipoMovimiento,
            Motivo = movimiento.Motivo,
            ReferenciaDocumento = movimiento.ReferenciaDocumento,
            UsuarioId = movimiento.UsuarioId,
            UsuarioNombre = movimiento.Usuario?.NombreCompleto ?? string.Empty,
            AlmacenOrigenId = movimiento.AlmacenOrigenId,
            AlmacenOrigenNombre = movimiento.AlmacenOrigen?.Nombre ?? string.Empty,
            AlmacenDestinoId = movimiento.AlmacenDestinoId,
            AlmacenDestinoNombre = movimiento.AlmacenDestino?.Nombre,
            Detalles = movimiento.DetalleMovimientos?.Select(d => new DetalleMovimientoDto
            {
                DetalleId = d.DetalleId,
                MovimientoId = d.MovimientoId,
                ProductoId = d.ProductoId,
                ProductoNombre = d.Producto?.Nombre ?? string.Empty,
                CodigoSku = d.Producto?.CodigoSku ?? string.Empty,
                Cantidad = d.Cantidad,
                CostoUnitarioHistorico = d.CostoUnitarioHistorico,
                CostoTotal = d.Cantidad * d.CostoUnitarioHistorico
            }).ToList() ?? new List<DetalleMovimientoDto>()
        };
    }
}
