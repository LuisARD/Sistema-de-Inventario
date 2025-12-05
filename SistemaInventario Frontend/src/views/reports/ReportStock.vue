<script setup>
import { ref, computed, onMounted } from "vue";
import { useExistenciasStore } from "../../store/existencias";
import { storeToRefs } from "pinia";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";

const store = useExistenciasStore();
const { existencias, loading } = storeToRefs(store);
const { fetchExistencias } = store;

const buscar = ref("");
const pdfContent = ref(null);

const exportarPDF = () => {
  if (!productos.value || productos.value.length === 0) {
    alert("No hay datos para exportar");
    return;
  }

  const doc = new jsPDF();

  doc.setFontSize(14);
  doc.text("Reporte de Existencias", 14, 15);

  doc.setFontSize(10);
  doc.text(`Fecha: ${new Date().toLocaleDateString()}`, 14, 22);

  autoTable(doc, {
    startY: 28,
    head: [["SKU", "Producto", "Almacén", "Stock", "Mínimo", "Estado"]],
    body: productos.value.map((p) => [
      p.codigoSku,
      p.nombre,
      p.almacen,
      p.stock,
      p.minimo,
      p.estado,
    ]),
    styles: {
      fontSize: 9,
      halign: "center",
    },
    headStyles: {
      fillColor: [30, 30, 30],
      textColor: [255, 255, 255],
    },
    alternateRowStyles: {
      fillColor: [245, 245, 245],
    },
  });

  doc.save("existencias.pdf");
};

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

onMounted(() => {
  fetchExistencias();
});

const productos = computed(() => {
  if (!existencias.value || existencias.value.length === 0) return [];

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

// Estadísticas
const totalProductos = computed(() => productos.value.length);
const totalCriticos = computed(() => productos.value.filter(p => p.estado === "Crítico").length);
const totalBajos = computed(() => productos.value.filter(p => p.estado === "Bajo").length);
const totalNormales = computed(() => productos.value.filter(p => p.estado === "Normal").length);
</script>

<template>
  <div class="min-h-screen bg-white p-5">
    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" 
                  d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"/>
          </svg>
        </div>
        <h1 class="text-3xl font-medium text-neutral">Reporte de Stock General</h1>
      </div>
    </div>

    <!-- CONTAINER -->
    <div class="max-w-7xl mx-auto bg-base-100 p-8 rounded-xl shadow">
      
      <!-- ESTADÍSTICAS -->
      <div class="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Total Productos</div>
          <div class="stat-value text-primary">{{ totalProductos }}</div>
          <div class="stat-desc">En inventario</div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Normal</div>
          <div class="stat-value text-success">{{ totalNormales }}</div>
          <div class="stat-desc">Stock adecuado</div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Bajos</div>
          <div class="stat-value text-warning">{{ totalBajos }}</div>
          <div class="stat-desc">Requieren atención</div>
        </div>
        <div class="stat bg-base-200 rounded-lg">
          <div class="stat-title">Críticos</div>
          <div class="stat-value text-error">{{ totalCriticos }}</div>
          <div class="stat-desc">Urgente</div>
        </div>
      </div>

      <!-- CONTROLES -->
      <div class="flex items-center justify-between mb-6">
        <!-- BÚSQUEDA -->
        <div class="w-full max-w-md">
          <FormKit
            type="search"
            v-model="buscar"
            placeholder="🔍 Buscar por nombre..."
            :classes="{
              outer: 'w-full',
              input: 'input input-bordered w-full',
            }"
          />
        </div>

        <!-- BOTÓN EXPORTAR -->
        <button @click="exportarPDF" class="btn btn-accent">
          <img src="/IconExport.svg" class="w-5 mr-2" />
          Exportar PDF
        </button>
      </div>

      <!-- TABLA -->
      <div ref="pdfContent" class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">
          <thead class="bg-base-200">
            <tr>
              <th>SKU</th>
              <th>Nombre</th>
              <th>Almacén</th>
              <th>Stock Actual</th>
              <th>Stock Mínimo</th>
              <th>Estado</th>
            </tr>
          </thead>

          <tbody>
            <!-- Loading -->
            <tr v-if="loading">
              <td colspan="6" class="text-center py-8">
                <span class="loading loading-spinner loading-lg"></span>
                <p class="mt-2">Cargando existencias...</p>
              </td>
            </tr>

            <!-- Sin resultados -->
            <tr v-else-if="productos.length === 0">
              <td colspan="6" class="text-center py-8">
                <p class="text-gray-500">No se encontraron productos</p>
              </td>
            </tr>

            <!-- Productos -->
            <tr v-else v-for="p in productos" :key="p.id">
              <td class="font-mono">{{ p.codigoSku }}</td>
              <td class="font-medium">{{ p.nombre }}</td>
              <td class="text-gray-600">{{ p.almacen }}</td>
              <td>
                <span class="font-bold" :class="{
                  'text-error': p.estado === 'Crítico',
                  'text-warning': p.estado === 'Bajo',
                  'text-success': p.estado === 'Normal'
                }">
                  {{ p.stock }}
                </span>
              </td>
              <td>{{ p.minimo }}</td>
              <td>
                <span class="badge" :class="{
                  'badge-error': p.estado === 'Crítico',
                  'badge-warning': p.estado === 'Bajo',
                  'badge-success': p.estado === 'Normal'
                }">
                  {{ p.estado }}
                </span>
              </td>
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
