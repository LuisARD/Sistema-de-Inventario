<script setup>
import { ref, computed, onMounted } from "vue";
import { useExistenciasStore } from "../../store/existencias";

const buscar = ref("");

const store = useExistenciasStore();
const { existencias, fetchExistencias, loading } = store;

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

// ---------------------------
// Crear lista procesada + estado + filtro
// ---------------------------
const productos = computed(() => {
  if (!existencias.value) return [];

  return existencias.value
    .map((item) => ({
      id: item.id,
      nombre: item.nombreProducto,
      categoria: item.categoriaNombre,
      stock: item.stockActual,
      minimo: item.stockMinimo,
      estado: calcularEstado(item.stockActual, item.stockMinimo),
    }))
    .filter((p) =>
      p.nombre.toLowerCase().includes(buscar.value.toLowerCase())
    );
});
</script>

<template>
  <header class="flex justify-between mt-5 mb-3">
    <FormKit
      type="search"
      placeholder="Search..."
      label="Buscar productos"
      v-model="buscar"
    />

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
            <th>Categorías</th>
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
            <th>{{ p.id }}</th>
            <td>{{ p.nombre }}</td>
            <td>{{ p.categoria }}</td>
            <td>{{ p.stock }}</td>
            <td>{{ p.minimo }}</td>
            <td>{{ p.estado }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </section>
</template>
