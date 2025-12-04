<script setup >
import ReportStock from "./ReportStock.vue";
import ReportMovimiento from "./ReportMovimiento.vue";
import ReportCritico from "./ReportCritico.vue";
import { ref } from "vue";

const activeTab = ref("stock");

const tabs = [
  { id: "stock", label: "Generar Reporte de Stock", desc: "Estado actual del inventario", component: ReportStock },
  { id: "mov", label: "Reporte de Movimientos", desc: "Historial de transacciones", component: ReportMovimiento },
  { id: "crit", label: "Stock crítico", desc: "Producto bajo del minimo", component: ReportCritico },
];


</script>

<template>
  <section class="flex justify-center items-center flex-col">
    <div class="flex flex-wrap justify-center items-center mt-10 mb-5">
      <h1 class="text-2xl font-bold flex items-center">
        <img src="/iconPageReport.svg" class="w-8 mr-2" />
        Reportes y Consultas
      </h1>
    </div>

    <!-- TABS -->
    <article class="w-full max-w-5xl p-4 rounded-lg flex gap-6  justify-center">
      <button
        v-for="t in tabs"
        :key="t.id"
        @click="activeTab = t.id"
        class="btn py-10 btn-base-300 flex gap-4 items-center w-80 text-left transition-colors duration-300"
        :class="{
          'btn-info text-white shadow-lg': activeTab === t.id,
          'hover:bg-base-300': activeTab !== t.id,
        }"
      >
        <div class="size-12 flex justify-center items-center">
          <img src="/document.svg" class="size-8" />
        </div>

        <p class="flex flex-col gap-y-1">
          <span>{{ t.label }}</span>
          <span class="text-xs opacity-70">{{ t.desc }}</span>
        </p>
      </button>
    </article>

    <!-- CONTENIDO DEL TAB -->
    <section class="w-full max-w-5xl mt-6 p-4">
      <component :is="tabs.find(t => t.id === activeTab).component" />
    </section>
  </section>
</template>

