<script setup>
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
  <section class="flex flex-col items-center px-4 md:px-0">

    <!-- HEADER -->
    <div class="flex justify-center items-center mt-10 mb-6 text-center">
      <h1 class="text-xl sm:text-2xl font-bold flex items-center gap-2">
        <img src="/iconPageReport.svg" class="w-7 sm:w-8" />
        Reportes y Consultas
      </h1>
    </div>

    <!-- TABS -->
    <article
      class="w-full max-w-5xl p-4 rounded-lg grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4"
    >
      <button
        v-for="t in tabs"
        :key="t.id"
        @click="activeTab = t.id"
        class="btn btn-base-300 flex gap-4 items-center text-left transition-all duration-300 w-full py-6 sm:py-8"
        :class="{
          'btn-info text-white shadow-lg scale-[1.02]': activeTab === t.id,
          'hover:bg-base-300': activeTab !== t.id,
        }"
      >
        <div class="size-10 sm:size-12 flex justify-center items-center">
          <img src="/document.svg" class="size-6 sm:size-8" />
        </div>

        <p class="flex flex-col gap-y-1">
          <span class="text-sm sm:text-base font-semibold">
            {{ t.label }}
          </span>
          <span class="text-xs opacity-70">
            {{ t.desc }}
          </span>
        </p>
      </button>
    </article>

    <!-- CONTENIDO -->
    <section class="w-full max-w-5xl mt-6 p-2 sm:p-4">
      <component :is="tabs.find(t => t.id === activeTab).component" />
    </section>
  </section>
</template>
