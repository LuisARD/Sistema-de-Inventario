<script setup>
import { onMounted, ref, computed } from "vue";
import { useExistenciasStore } from "../../store/existencias";
import { useProductosStore } from "../../store/producto";

const existenciasStore = useExistenciasStore();
const productosStore = useProductosStore();
const searchQuery = ref("");
const selectedTab = ref("todas"); // todas, stock-bajo

// Cargar datos al montar
onMounted(async () => {
  await existenciasStore.fetchExistencias();
  await productosStore.fetchProductos();
});

// Filtrado de existencias
const filteredExistencias = computed(() => {
  let data = existenciasStore.existencias;

  // Si el tab es stock-bajo, filtrar solo los que tienen StockBajo = true
  if (selectedTab.value === "stock-bajo") {
    data = data.filter(e => e.StockBajo === true || e.CantidadActual <= e.StockMinimo);
  }

  const q = searchQuery.value?.trim().toLowerCase();
  if (!q) return data;

  return data.filter((e) =>
    Object.values(e).some((v) =>
      String(v ?? "").toLowerCase().includes(q)
    )
  );
});

const cargarStockBajo = () => {
  selectedTab.value = "stock-bajo";
};

const cargarTodas = () => {
  selectedTab.value = "todas";
};

// Función para obtener clase de alerta según el stock
const getStockClass = (stock, stockMinimo) => {
  if (stock === 0) return "text-error font-bold";
  if (stock <= stockMinimo) return "text-error font-semibold";
  return "";
};
</script>

<template>
  <div class="min-h-screen bg-white p-5">

    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <!-- ICON -->
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path d="M20 7h-9"/>
            <path d="M14 17H5"/>
            <circle cx="17" cy="17" r="3"/>
            <circle cx="7" cy="7" r="3"/>
          </svg>
        </div>

        <h1 class="text-3xl font-medium text-neutral">Gestión de Existencias</h1>
      </div>
    </div>

    <!-- CONTAINER -->
    <div class="max-w-7xl mx-auto bg-base-100 p-8 rounded-xl shadow">

      <!-- TABS -->
      <div class="tabs tabs-boxed mb-6">
        <a 
          :class="['tab', { 'tab-active': selectedTab === 'todas' }]"
          @click="cargarTodas"
        >
          Todas las Existencias
        </a>
        <a 
          :class="['tab', { 'tab-active': selectedTab === 'stock-bajo' }]"
          @click="cargarStockBajo"
        >
          Stock Bajo
        </a>
      </div>

      <!-- CONTROLS -->
      <div class="flex items-center justify-between mb-6">

        <!-- SEARCH -->
        <div class="w-64">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="🔍 Buscar por producto o almacén"
            class="input input-bordered w-full"
          >
        </div>

        <!-- INFO -->
        <div class="stats shadow">
          <div class="stat">
            <div class="stat-title">Total Productos</div>
            <div class="stat-value text-primary">{{ filteredExistencias.length }}</div>
          </div>
        </div>
      </div>

      <!-- TABLE -->
      <div class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">

          <thead class="bg-base-200">
            <tr>
              <th>Producto</th>
              <th>SKU</th>
              <th>Almacén</th>
              <th>Stock Actual</th>
              <th>Stock Mínimo</th>
              <th>Ubicación</th>
              <th>Estado</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="existencia in filteredExistencias" :key="existencia.ExistenciaId">
              <td>{{ existencia.ProductoNombre }}</td>
              <td>{{ existencia.CodigoSku || "-" }}</td>
              <td>{{ existencia.AlmacenNombre || "Principal" }}</td>
              <td :class="getStockClass(existencia.CantidadActual, existencia.StockMinimo)">
                {{ existencia.CantidadActual || 0 }}
              </td>
              <td>{{ existencia.StockMinimo || 0 }}</td>
              <td>{{ existencia.UbicacionPasillo || "-" }}</td>
              <td>
                <span 
                  v-if="existencia.CantidadActual === 0"
                  class="badge badge-error"
                >
                  Sin Stock
                </span>
                <span 
                  v-else-if="existencia.StockBajo"
                  class="badge badge-error"
                >
                  Stock Bajo
                </span>
                <span 
                  v-else
                  class="badge badge-success"
                >
                  Normal
                </span>
              </td>
            </tr>
            <tr v-if="filteredExistencias.length === 0">
              <td colspan="7" class="text-center py-4">
                {{ existenciasStore.loading ? "Cargando..." : "No hay existencias" }}
              </td>
            </tr>
          </tbody>
        </table>
      </div>

    </div>
  </div>
</template>
