<script setup>

const movimientos = [
  {
    fecha: "2025-01-20",
    movimiento: "Entrada",
    cantidad: 15,
    usuario: "Juan Pérez",
    detalle: "Ingreso de mercancía desde proveedor",
  },
  {
    fecha: "2025-01-18",
    movimiento: "Salida",
    cantidad: 4,
    usuario: "María Gómez",
    detalle: "Producto vendido a cliente",
  },
  {
    fecha: "2025-01-16",
    movimiento: "Entrada",
    cantidad: 20,
    usuario: "Carlos Ruiz",
    detalle: "Reposición de inventario",
  },
];

</script>



<template>
   <header class="flex justify-between items-center mt-5 mb-9">
 <FormKit
  type="select"
  name="movimiento"
  label="Tipo de movimiento"
  placeholder="Selecciona una opción"
  :options="['Entrada', 'Salida']"
  input-class="select select-bordered w-full"
  outer-class="mb-4 flex flex-col gap-1"
/>

<FormKit
  type="date"
  name="fecha"
  label="Fecha del movimiento"
  input-class="input input-bordered w-full"
  outer-class="mb-4 flex flex-col gap-1"
/>


<button class="btn btn-accent ml-2">
    <img src="/Search.svg" class="w-5 mr-2" />
    Buscar</button>
</header>


  <table class="table w-full bg-base-300 rounded-lg shadow-lg ">
    <thead>
      <tr class="text-neutral">
        <th>Producto</th>
        <th>Tipo de Movimiento</th>
        <th>Cantidad</th>
        <th>Fecha</th>
        <th>Responsable</th>
      </tr>
    </thead>
    <tbody>
     <tr v-for="m in movimientos" :key="m.fecha + m.usuario">
  <td>{{ m.fecha }}</td>
  <td>{{ m.movimiento }}</td>
  <td>{{ m.cantidad }}</td>
  <td>{{ m.usuario }}</td>
  <td>{{ m.detalle }}</td>
</tr>
    </tbody>
  </table>


</template>