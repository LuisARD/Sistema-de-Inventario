<script setup>


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



const productos = [
  {
    id: 1,
    nombre: "Café Molido",
    categoria: "Alimentos",
    stock: 30,
    minimo: 10,
   get estado() {
    return calcularEstado(this.stock, this.minimo);
  }
 },
{
    id: 2,
    nombre: "Azúcar Blanca",
    categoria: "Alimentos",
    stock: 20,
    minimo: 15,
   get estado() {
    return calcularEstado(this.stock, this.minimo);
  }
 },
 {
    id: 3,
    nombre: "Cloro Líquido",
    categoria: "Limpieza",
    stock: 3,
    minimo: 5,
   get estado() {
    return calcularEstado(this.stock, this.minimo);
  }
 }
];

  

</script>



<template>

<header class="flex justify-between mt-5 mb-3">
    <FormKit
  type="search"
  placeholder="Search..."
  label="Buscar productos"
/>


<button class="btn btn-accent ml-2">
    <img src="/IconExport.svg" class="w-5 mr-2" />
    Exportar</button>
</header>

<section>
    <div class="overflow-x-auto rounded-box border border-base-content/5 bg-base-100">
  <table class="table">
    <!-- head -->
    <thead>
      <tr>
        <th>ID</th>
        <th>Nombre</th>
        <th>Categorías</th>
        <th>Stock</th>
         <th>Minimo</th>
          <th>Estado</th>
        
      </tr>
    </thead>
    <tbody>
   <tr v-for="p in productos" :key="p.id" :class="styleEstado[p.estado]">
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