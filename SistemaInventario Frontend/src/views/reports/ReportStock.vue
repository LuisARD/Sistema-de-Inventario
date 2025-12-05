<script setup>
import { ref, computed, onMounted } from "vue";
import { useExistenciasStore } from "../../store/existencias";
import { storeToRefs } from "pinia";

const store = useExistenciasStore();
const { existencias, loading } = storeToRefs(store);
const { fetchExistencias } = store;

const buscar = ref("");


// ---------------------------
// Lógica para estado del stock
// ---------------------------
function calcularEstado(stock, minimo) {
  if (stock < minimo) return "Crítico";
  if (stock < minimo * 2) return "Bajo";
  return "Normal";
}

const styleEstado = {
  Crítico: "bg-error text-error-content font-bold",
  Bajo: "bg-warning text-warning-content font-bold",
  Normal: "bg-success text-success-content font-bold",
};

// ---------------------------
// Obtener datos al montar
// ---------------------------
onMounted(() => {
  fetchExistencias();
}); 

console.log(existencias)

// ---------------------------
// Crear lista procesada + estado + filtro
// ---------------------------
const productos = computed(() => {
  if (!existencias.value) return [];

  return existencias.value
    .map((item) => ({
      id: item.ExistenciaId,
      codigoSku: item.CodigoSku,
      nombre: item.ProductoNombre,
      almacen: item.AlmacenNombre,
      stock: item.CantidadActual,
      minimo: item.StockMinimo,
      estado: calcularEstado(item.CantidadActual, item.StockMinimo),
    }))
    .filter((p) =>
      p.nombre.toLowerCase().includes(buscar.value.toLowerCase())
    );
});


console.log(productos)
</script>

<template>
  <header class="flex justify-between mt-5 mb-3">
  <FormKit
  type="search"
  v-model="buscar"
  placeholder="Buscar por nombre..."
  :classes="{
    outer: 'w-full max-w-sm',
    label: 'text-sm font-semibold text-gray-600 mb-1',
    input:
      'input input-bordered w-full rounded-xl pl-10 bg-base-100 focus:outline-none focus:ring-2 focus:ring-primary',
  }"
>
  <
</FormKit>

    <button class="btn btn-accent ml-2">
      <img src="/IconExport.svg" class="w-5 mr-2" />
      Exportar
    </button>
  </header>

  <section>
    <div class="overflow-x-auto rounded-box border border-base-content/5 bg-base-100">
      <table class="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Almacen</th>
            <th>Stock</th>
            <th>Mínimo</th>
            <th>Estado</th>
          </tr>
        </thead>

       <tbody>
  <tr
    v-for="p in productos"
    :key="p.id"
    :class="styleEstado[p.estado]"
  >
    <th>{{ p.codigoSku }}</th>
    <td>{{ p.nombre }}</td>
    <td>{{ p.almacen }}</td>
    <td>{{ p.stock }}</td>
    <td>{{ p.minimo }}</td>
    <td>{{ p.estado }}</td>
  </tr>
</tbody>
      </table>
    </div>
  </section>
</template>
