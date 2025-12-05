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
</script>

<template>
  <header class="flex justify-between mt-5 mb-3 gap-3">
    <FormKit
      type="search"
      v-model="buscar"
      placeholder="Buscar por nombre..."
      :classes="{
        outer: 'w-full max-w-sm',
        input:
          'input input-bordered w-full rounded-xl pl-10 bg-base-100 focus:outline-none focus:ring-2 focus:ring-primary',
      }"
    />

    <button @click="exportarPDF" class="btn btn-accent ml-2">
      <img src="/IconExport.svg" class="w-5 mr-2" />
      Exportar PDF
    </button>
  </header>

  <section>
    <div
      ref="pdfContent"
      class="overflow-x-auto rounded-box border border-base-content/5 bg-base-100"
    >
      <table class="table">
        <thead>
          <tr>
            <th>SKU</th>
            <th>Nombre</th>
            <th>Almacén</th>
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
