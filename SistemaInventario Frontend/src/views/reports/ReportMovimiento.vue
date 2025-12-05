<script setup>
import { ref, computed, onMounted } from "vue";
import { useMovimientoStore } from "../../store/movimiento";
import { formatDate, formatDateOnly } from "../../utils/formatDate";

const movimientoStore = useMovimientoStore();

// Filtros
const buscarTexto = ref("");
const filtroTipoMovimiento = ref("");
const filtroFecha = ref("");

// Cargar movimientos al montar
onMounted(async () => {
  await movimientoStore.fetchMovimiento();
});

// Computed para filtrar movimientos
const movimientosFiltrados = computed(() => {
  let resultado = [...movimientoStore.movimientos];

  // Filtrar por búsqueda de texto (producto, responsable, etc.)
  if (buscarTexto.value && buscarTexto.value.trim() !== "") {
    const textoBusqueda = buscarTexto.value.toLowerCase().trim();
    resultado = resultado.filter((m) => {
      // Buscar en los nombres de productos dentro de Detalles
      const nombresProductos = m.Detalles && m.Detalles.length > 0
        ? m.Detalles.map(d => (d.ProductoNombre || '').toLowerCase()).join(' ')
        : '';
      
      const responsable = (m.UsuarioNombre || m.ResponsableNombre || '').toLowerCase();
      const tipoMovimiento = (m.TipoMovimiento || '').toLowerCase();
      
      // Sumar cantidades de todos los detalles
      const cantidadTotal = m.Detalles && m.Detalles.length > 0
        ? m.Detalles.reduce((sum, d) => sum + (d.Cantidad || 0), 0).toString()
        : '';
      
      return nombresProductos.includes(textoBusqueda) ||
             responsable.includes(textoBusqueda) ||
             tipoMovimiento.includes(textoBusqueda) ||
             cantidadTotal.includes(textoBusqueda);
    });
  }

  // Filtrar por tipo de movimiento
  if (filtroTipoMovimiento.value && filtroTipoMovimiento.value !== "") {
    resultado = resultado.filter(
      (m) => m.TipoMovimiento?.toUpperCase() === filtroTipoMovimiento.value.toUpperCase()
    );
  }

  // Filtrar por fecha
  if (filtroFecha.value && filtroFecha.value !== "") {
    resultado = resultado.filter((m) => {
      const fechaMovimiento = new Date(m.FechaMovimiento).toISOString().split('T')[0];
      return fechaMovimiento === filtroFecha.value;
    });
  }

  return resultado;
});

// Limpiar filtros
const limpiarFiltros = () => {
  buscarTexto.value = "";
  filtroTipoMovimiento.value = "";
  filtroFecha.value = "";
};

// Obtener clase de badge según tipo de movimiento
const getBadgeClass = (tipo) => {
  return tipo?.toUpperCase() === 'ENTRADA' 
    ? 'badge badge-success' 
    : 'badge badge-error';
};
</script>

<template>
  <div class="min-h-screen bg-white p-5">
    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
          </svg>
        </div>
        <h1 class="text-3xl font-medium text-neutral">Reporte de Movimientos</h1>
      </div>
    </div>

    <!-- CONTAINER -->
    <div class="max-w-7xl mx-auto bg-base-100 p-8 rounded-xl shadow">
      
      <!-- FILTROS -->
      <div class="mb-6">
        <h2 class="text-xl font-semibold mb-4">Filtros de búsqueda</h2>
        
        <!-- Búsqueda general -->
        <div class="mb-4">
          <label class="label">
            <span class="label-text font-medium">Buscar en todos los campos</span>
          </label>
          <input 
            v-model="buscarTexto" 
            type="text" 
            placeholder="Buscar por producto, responsable, tipo o cantidad..." 
            class="input input-bordered w-full"
          />
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-4 items-end">
          
          <!-- Filtro Tipo Movimiento -->
          <div>
            <label class="label">
              <span class="label-text font-medium">Tipo de Movimiento</span>
            </label>
            <select 
              v-model="filtroTipoMovimiento" 
              class="select select-bordered w-full"
            >
              <option value="">Todos</option>
              <option value="ENTRADA">Entrada</option>
              <option value="SALIDA">Salida</option>
            </select>
          </div>

          <!-- Filtro Fecha -->
          <div>
            <label class="label">
              <span class="label-text font-medium">Fecha específica</span>
            </label>
            <input 
              v-model="filtroFecha" 
              type="date" 
              class="input input-bordered w-full"
            />
          </div>

          <!-- Botón Limpiar -->
          <div>
            <button 
              @click="limpiarFiltros" 
              class="btn btn-outline w-full"
            >
              <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
              Limpiar Filtros
            </button>
          </div>
        </div>
      </div>

      <!-- ESTADÍSTICAS -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Total Movimientos</div>
          <div class="stat-value text-primary">{{ movimientosFiltrados.length }}</div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Entradas</div>
          <div class="stat-value text-success">
            {{ movimientosFiltrados.filter(m => m.TipoMovimiento?.toUpperCase() === 'ENTRADA').length }}
          </div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Salidas</div>
          <div class="stat-value text-error">
            {{ movimientosFiltrados.filter(m => m.TipoMovimiento?.toUpperCase() === 'SALIDA').length }}
          </div>
        </div>
      </div>

      <!-- TABLA -->
      <div class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">
          <thead class="bg-base-200">
            <tr>
              <th>Producto</th>
              <th>Tipo de Movimiento</th>
              <th>Cantidad</th>
              <th>Fecha</th>
              <th>Responsable</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="movimientoStore.loading">
              <td colspan="5" class="text-center py-8">
                <span class="loading loading-spinner loading-lg"></span>
                <p class="mt-2">Cargando movimientos...</p>
              </td>
            </tr>
            
            <tr v-else-if="movimientosFiltrados.length === 0">
              <td colspan="5" class="text-center py-8 text-gray-500">
                No se encontraron movimientos
              </td>
            </tr>
            
            <tr v-else v-for="m in movimientosFiltrados" :key="m.MovimientoId">
              <td class="font-medium">
                <!-- Mostrar todos los productos del movimiento -->
                <div v-if="m.Detalles && m.Detalles.length > 0">
                  <div v-for="(detalle, index) in m.Detalles" :key="detalle.DetalleId">
                    {{ detalle.ProductoNombre }}
                    <span v-if="index < m.Detalles.length - 1" class="text-gray-400">, </span>
                  </div>
                </div>
                <span v-else class="text-gray-400">Sin productos</span>
              </td>
              <td>
                <span :class="getBadgeClass(m.TipoMovimiento)">
                  {{ m.TipoMovimiento || 'N/A' }}
                </span>
              </td>
              <td>
                <!-- Mostrar la suma total de cantidades -->
                <span class="font-bold"
                  :class="{
                    'text-success': m.TipoMovimiento?.toUpperCase() === 'ENTRADA',
                    'text-error': m.TipoMovimiento?.toUpperCase() === 'SALIDA'
                  }">
                  {{ m.TipoMovimiento?.toUpperCase() === 'ENTRADA' ? '+' : '-' }}{{ 
                    m.Detalles && m.Detalles.length > 0 
                      ? m.Detalles.reduce((sum, d) => sum + (d.Cantidad || 0), 0) 
                      : 0 
                  }}
                </span>
              </td>
              <td>{{ formatDate(m.FechaMovimiento) }}</td>
              <td>{{ m.UsuarioNombre || m.ResponsableNombre || 'N/A' }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>
.stat {
  padding: 1.5rem;
}
</style>