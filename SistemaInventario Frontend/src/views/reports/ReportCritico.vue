<script setup>
import { ref, computed, onMounted } from "vue";
import { useExistenciasStore } from "../../store/existencias";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";

const existenciasStore = useExistenciasStore();

// Cargar existencias al montar
onMounted(async () => {
  await existenciasStore.fetchExistencias();
});

// Filtrar productos con estado CRÍTICO y BAJO (stock actual <= stock mínimo)
const productosCriticos = computed(() => {
  return existenciasStore.existencias.filter((e) => {
    const stockActual = e.CantidadActual || 0;
    const stockMinimo = e.StockMinimo || 0;
    
    // Mostrar productos críticos (< 70% del mínimo) y bajos (<= stock mínimo)
    return stockActual <= stockMinimo;
  });
});

// Función para determinar el estado del producto
const getEstado = (existencia) => {
  const stockActual = existencia.CantidadActual || 0;
  const stockMinimo = existencia.StockMinimo || 0;
  const umbralCritico = stockMinimo * 0.7; // 70% del mínimo
  
  if (stockActual === 0) return "Agotado";
  if (stockActual < umbralCritico) return "Crítico"; // Menos del 70%
  if (stockActual <= stockMinimo) return "Bajo"; // Entre 70% y 100%
  return "Normal";
};

// Función para obtener clase de badge según estado
const getBadgeClass = (producto) => {
  const estado = getEstado(producto);
  if (estado === "Agotado") return "badge badge-error";
  if (estado === "Crítico") return "badge badge-warning";
  if (estado === "Bajo") return "badge badge-info";
  return "badge badge-success";
};

const cantidad = computed(() => productosCriticos.value.length);

// Computed para contar críticos y bajos
const totalCriticos = computed(() => {
  return productosCriticos.value.filter(e => {
    const estado = getEstado(e);
    return estado === "Crítico" || estado === "Agotado";
  }).length;
});

const totalBajos = computed(() => {
  return productosCriticos.value.filter(e => {
    const estado = getEstado(e);
    return estado === "Bajo";
  }).length;
});

// Función para exportar a PDF
const exportarPDF = () => {
  if (!productosCriticos.value || productosCriticos.value.length === 0) {
    alert("No hay datos para exportar");
    return;
  }

  const doc = new jsPDF();

  // Título
  doc.setFontSize(16);
  doc.setFont(undefined, 'bold');
  doc.text("Reporte de Stock Crítico", 14, 15);

  // Información del reporte
  doc.setFontSize(10);
  doc.setFont(undefined, 'normal');
  doc.text(`Fecha: ${new Date().toLocaleDateString('es-ES')}`, 14, 22);
  doc.text(`Total productos: ${cantidad.value}`, 14, 28);
  doc.text(`Críticos/Agotados: ${totalCriticos.value}`, 14, 34);
  doc.text(`Bajos: ${totalBajos.value}`, 14, 40);

  // Tabla
  autoTable(doc, {
    startY: 48,
    head: [["SKU", "Producto", "Stock", "Mínimo", "Estado", "Diferencia", "Almacén"]],
    body: productosCriticos.value.map((e) => [
      e.CodigoSku,
      e.ProductoNombre,
      e.CantidadActual,
      e.StockMinimo,
      getEstado(e),
      `-${e.StockMinimo - e.CantidadActual}`,
      e.AlmacenNombre,
    ]),
    styles: {
      fontSize: 9,
      halign: "center",
    },
    headStyles: {
      fillColor: [30, 30, 30],
      textColor: [255, 255, 255],
      fontStyle: 'bold',
    },
    alternateRowStyles: {
      fillColor: [245, 245, 245],
    },
    // Colorear filas según estado
    didParseCell: function(data) {
      if (data.column.index === 4 && data.section === 'body') {
        const estado = data.cell.text[0];
        if (estado === 'Agotado' || estado === 'Crítico') {
          data.cell.styles.textColor = [220, 38, 38]; // Rojo
          data.cell.styles.fontStyle = 'bold';
        } else if (estado === 'Bajo') {
          data.cell.styles.textColor = [234, 179, 8]; // Amarillo
          data.cell.styles.fontStyle = 'bold';
        }
      }
    }
  });

  doc.save(`reporte-stock-critico-${new Date().toISOString().split('T')[0]}.pdf`);
};
</script>

<template>
  <main class="min-h-screen p-5">

    <!-- HEADER -->
    <header class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <figure
          class="w-12 h-12 bg-warning text-white rounded-xl flex items-center justify-center"
          aria-hidden="true"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" 
              d="M12 9v3m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
            />
          </svg>
        </figure>

        <h1 class="text-3xl font-medium text-neutral">
          Reporte de Stock Crítico
        </h1>
      </div>
    </header>

    <!-- CONTENIDO PRINCIPAL -->
    <section class="max-w-7xl mx-auto bg-base-100 p-8 rounded-xl shadow">

      <!-- ESTADÍSTICAS -->
      <section class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
        
        <article class="stat bg-info rounded-lg">
          <h2 class="stat-title">Total Productos</h2>
          <p class="stat-value">{{ cantidad }}</p>
          <p class="stat-desc">Requieren atención</p>
        </article>

        <article class="stat bg-info rounded-lg">
          <h2 class="stat-title">Críticos/Agotados</h2>
          <p class="stat-value">{{ totalCriticos }}</p>
          <p class="stat-desc">Menos del 70% del mínimo</p>
        </article>

        <article class="stat bg-info rounded-lg">
          <h2 class="stat-title">Bajos</h2>
          <p class="stat-value">{{ totalBajos }}</p>
          <p class="stat-desc">Entre 70% y 100% del mínimo</p>
        </article>

      </section>

      <!-- ACCIONES -->
      <nav class="mb-4" aria-label="Acciones del reporte">
        <button @click="exportarPDF" class="btn btn-accent">
          <img src="/IconExport.svg" class="w-5 mr-2" alt="Exportar PDF" />
          Exportar PDF
        </button>
      </nav>

      <!-- TABLA -->
      <section class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">
          <thead class="bg-base-200">
            <tr>
              <th scope="col">Código/SKU</th>
              <th scope="col">Nombre</th>
              <th scope="col">Stock Actual</th>
              <th scope="col">Stock Mínimo</th>
              <th scope="col">Estado</th>
              <th scope="col">Diferencia</th>
              <th scope="col">Almacén</th>
            </tr>
          </thead>

          <tbody>
            <!-- LOADING -->
            <tr v-if="existenciasStore.loading">
              <td colspan="7" class="text-center py-8">
                <span class="loading loading-spinner loading-lg"></span>
                <p class="mt-2">Cargando existencias...</p>
              </td>
            </tr>

            <!-- SIN PRODUCTOS CRÍTICOS -->
            <tr v-else-if="productosCriticos.length === 0">
              <td colspan="7" class="text-center py-8">
                <div class="flex flex-col items-center gap-2">
                  <svg xmlns="http://www.w3.org/2000/svg"
                    class="w-12 h-12 text-success"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path stroke-linecap="round" stroke-linejoin="round"
                      stroke-width="2"
                      d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"
                    />
                  </svg>
                  <p class="text-lg font-semibold text-success">¡Excelente!</p>
                  <p class="text-gray-500">
                    No hay productos con stock crítico
                  </p>
                </div>
              </td>
            </tr>

            <!-- FILAS -->
            <tr
              v-else
              v-for="e in productosCriticos"
              :key="e.ExistenciaId"
            >
              <td class="font-mono">{{ e.CodigoSku }}</td>
              <td class="font-medium">{{ e.ProductoNombre }}</td>

              <td>
                <strong :class="{
                  'text-error': e.CantidadActual === 0,
                  'text-warning': e.CantidadActual > 0
                }">
                  {{ e.CantidadActual }}
                </strong>
              </td>

              <td>{{ e.StockMinimo }}</td>

              <td>
                <span :class="getBadgeClass(e)">
                  {{ getEstado(e) }}
                </span>
              </td>

              <td>
                <span class="text-error font-semibold">
                  -{{ e.StockMinimo - e.CantidadActual }}
                </span>
              </td>

              <td class="text-gray-600">
                {{ e.AlmacenNombre }}
              </td>
            </tr>
          </tbody>
        </table>
      </section>

    </section>
  </main>
</template>

<style scoped>
.stat {
  padding: 1.5rem;
}
</style>
