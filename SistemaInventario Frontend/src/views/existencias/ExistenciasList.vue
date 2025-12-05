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
  <main class="min-h-screen p-4 sm:p-5">

    <!-- HEADER -->
    <header class="flex items-center justify-between mb-6 sm:mb-8 p-4 sm:p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <figure class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"
               aria-hidden="true">
            <path d="M20 7h-9"/>
            <path d="M14 17H5"/>
            <circle cx="17" cy="17" r="3"/>
            <circle cx="7" cy="7" r="3"/>
          </svg>
        </figure>

        <h1 class="text-2xl sm:text-3xl font-medium text-neutral">
          Gestión de Existencias
        </h1>
      </div>
    </header>

    <!-- CONTENIDO -->
    <section class="max-w-7xl mx-auto bg-base-100 p-4 sm:p-8 rounded-xl shadow">

      <!-- TABS -->
      <nav class="tabs tabs-boxed mb-6 flex flex-col sm:flex-row gap-2">
        <button
          type="button"
          class="tab"
          :class="{ 'tab-active': selectedTab === 'todas' }"
          @click="cargarTodas"
        >
          Todas las Existencias
        </button>

        <!-- <button
          type="button"
          class="tab"
          :class="{ 'tab-active': selectedTab === 'stock-bajo' }"
          @click="cargarStockBajo"
        >
          Stock Bajo
        </button> -->
      </nav>

      <!-- CONTROLES -->
      <section class="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-6">

        <!-- BUSCADOR -->
        <form class="w-full md:w-64" role="search">
          <input
            type="search"
            v-model="searchQuery"
            placeholder="🔍 Buscar por producto o almacén"
            class="input input-bordered w-full"
            aria-label="Buscar existencias"
          >
        </form>

        <!-- ESTADÍSTICA -->
        <aside class="stats shadow w-full md:w-auto text-center">
          <div class="stat">
            <div class="stat-title">Total Productos</div>
            <div class="stat-value text-primary">
              {{ filteredExistencias.length }}
            </div>
          </div>
        </aside>

      </section>

      <!-- TABLA RESPONSIVE -->
      <section class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full" aria-label="Listado de existencias">

          <thead class="bg-base-200">
            <tr>
              <th scope="col">Producto</th>
              <th scope="col">SKU</th>
              <th scope="col">Almacén</th>
              <th scope="col">Stock Actual</th>
              <th scope="col">Stock Mínimo</th>
              <th scope="col">Ubicación</th>
              <th scope="col">Estado</th>
            </tr>
          </thead>

          <tbody>
            <tr 
              v-for="existencia in filteredExistencias"
              :key="existencia.ExistenciaId"
            >
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

            <!-- ESTADO VACÍO -->
            <tr v-if="filteredExistencias.length === 0">
              <td colspan="7" class="text-center py-6 text-gray-500">
                {{ existenciasStore.loading ? "Cargando..." : "No hay existencias" }}
              </td>
            </tr>

          </tbody>
        </table>
      </section>

    </section>
  </main>
</template>
