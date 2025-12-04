<script setup>
const productosCriticos = [
  {
    codigo: "P-001",
    nombre: "Azúcar Blanca 1kg",
    stock: 12,
    minimo: 20,
    estado: "Bajo",
  },
  {
    codigo: "P-003",
    nombre: "Cloro Desinfectante",
    stock: 3,
    minimo: 5,
    estado: "Crítico",
  },
  {
    codigo: "P-006",
    nombre: "Detergente Líquido",
    stock: 0,
    minimo: 8,
    estado: "Agotado",
  },
  {
    codigo: "P-006",
    nombre: "Detergente Líquido",
    stock: 0,
    minimo: 8,
    estado: "Agotado",
  },
];

const cantidad = productosCriticos.length;
</script>

<template>
  <section class="w-full max-w-5xl mx-auto mt-10 space-y-6">
    <!-- ALERTA -->
    <div class="alert bg-yellow-100 border border-yellow-300 rounded-lg">
      <svg
        xmlns="http://www.w3.org/2000/svg"
        class="stroke-current w-6 h-6"
        fill="none"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M12 9v3m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 
          1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 
          0L3.34 16c-.77 1.333.192 3 1.732 3z"
        />
      </svg>

      <div class="">
        <h3 class="font-bold">Atención Requerida</h3>
        <div class="text-sm">
          Hay {{ cantidad }} producto(s) que requieren reabastecimiento
        </div>
      </div>
    </div>

    <!-- BOTÓN EXPORTAR -->
  <button class="btn btn-accent ml-2">
    <img src="/IconExport.svg" class="w-5 mr-2" />
    Exportar
  </button>

    <table class="table w-full rounded-lg shadow-lg">
      <thead class="">
        <tr>
          <th>Código</th>
          <th>Nombre</th>
          <th>Stock Actual</th>
          <th>Mínimo</th>
          <th>Estado</th>
        </tr>
      </thead>

      <tbody>
        <tr
          v-for="p in productosCriticos"
          :key="p.codigo"
          class=""
        >
          <td>{{ p.codigo }}</td>
          <td>{{ p.nombre }}</td>
          <td>{{ p.stock }}</td>
          <td>{{ p.minimo }}</td>
          <td>{{ p.estado }}</td>
        </tr>
      </tbody>
    </table>
  </section>
</template>
