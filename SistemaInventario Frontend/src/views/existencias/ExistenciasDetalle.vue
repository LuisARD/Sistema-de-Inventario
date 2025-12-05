<script setup>
import { onMounted, ref, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useExistenciasStore } from "../../store/existencias";
import { useProductosStore } from "../../store/producto";

const route = useRoute();
const router = useRouter();
const existenciasStore = useExistenciasStore();
const productosStore = useProductosStore();

const productoId = ref(route.params.productoId);
const producto = ref(null);

onMounted(async () => {
  await existenciasStore.fetchExistenciasPorProducto(productoId.value);
  await productosStore.fetchProductos();
  
  producto.value = productosStore.productos.find(
    p => p.ProductoId == productoId.value
  );
});

const totalStock = computed(() => {
  return existenciasStore.existenciasPorProducto.reduce(
    (sum, e) => sum + (e.CantidadActual || 0), 0
  );
});

const volver = () => {
  router.push('/existencias');
};
</script>

<template>
  <div class="min-h-screen bg-white p-5">

    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <button @click="volver" class="btn btn-ghost btn-circle">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path d="M15 18l-6-6 6-6"/>
          </svg>
        </button>

        <h1 class="text-3xl font-medium text-neutral">
          Existencias: {{ producto?.Nombre || 'Producto' }}
        </h1>
      </div>
    </div>

    <!-- CONTAINER -->
    <div class="max-w-6xl mx-auto bg-base-100 p-8 rounded-xl shadow">

      <!-- PRODUCTO INFO -->
      <div v-if="producto" class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-8">
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">SKU</div>
          <div class="stat-value text-2xl">{{ producto.CodigoSku }}</div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Stock Total</div>
          <div class="stat-value text-primary">{{ totalStock }}</div>
          <div class="stat-desc">{{ producto.UnidadMedida }}</div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Stock Mínimo</div>
          <div class="stat-value text-2xl">{{ producto.StockMinimo }}</div>
        </div>
      </div>

      <!-- TABLE -->
      <h2 class="text-xl font-semibold mb-4">Existencias por Almacén</h2>
      <div class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">

          <thead class="bg-base-200">
            <tr>
              <th>Almacén</th>
              <th>Ubicación</th>
              <th>Stock Actual</th>
              <th>Última Actualización</th>
              <th>Estado</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="existencia in existenciasStore.existenciasPorProducto" 
                :key="existencia.ExistenciaId">
              <td>{{ existencia.AlmacenNombre || "Principal" }}</td>
              <td>{{ existencia.UbicacionPasillo || "-" }}</td>
              <td class="font-semibold">{{ existencia.CantidadActual || 0 }}</td>
              <td>{{ existencia.UltimaActualizacion ? new Date(existencia.UltimaActualizacion).toLocaleDateString() : "-" }}</td>
              <td>
                <span 
                  v-if="existencia.CantidadActual === 0"
                  class="badge badge-error"
                >
                  Sin Stock
                </span>
                <span 
                  v-else-if="existencia.StockBajo"
                  class="badge badge-warning"
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
            <tr v-if="existenciasStore.existenciasPorProducto.length === 0">
              <td colspan="5" class="text-center py-4">
                No hay existencias para este producto
              </td>
            </tr>
          </tbody>
        </table>
      </div>

    </div>
  </div>
</template>
