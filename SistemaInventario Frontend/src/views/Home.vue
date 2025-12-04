<script setup>
const buttones = [
  { to: "/products", label: "Gestión de Productos", icon: "/camion.svg" },
  { to: "/suppliers", label: "Gestión de Proveedores", icon: "/btn1.svg" },
  { to: "/products", label: "Control de Inventario", icon: "/btn1.svg" },
  { to: "/movements", label: "Movimientos de Inventario", icon: "/camion.svg" },
  { to: "/categories", label: "Gestión de Categorías", icon: "/btn1.svg" },
  { to: "/reports", label: "Reportes", icon: "/iconReporte.svg" },
];
</script>

<template>
  <section
    class="flex flex-col md:flex-row md:items-center md:justify-between md:w-[90%] md:mx-auto lg:justify-center lg:items-center md:gap-x-20 md:mt-20"
  >
   
    <article class="w-full h-full md:w-[50%] lg:w-[70%] lg:max-w-[900px] mt-6 md:mt-0">
      <div
        class="grid grid-cols-1 gap-4 md:grid-cols-1 md:gap-6 md:place-items-center lg:grid-cols-3"
      >
        <router-link
          v-for="btn in buttones"
          :key="btn.to"
          :to="btn.to"
          
          class="w-full md:w-[280px] rounded-xl py-6 shadow-lg hover:opacity-90 transition flex justify-center items-center border-2 border-transparent hover:border-primary hover:border-2 duration-300 delay-75 lg:gap-x-4"
        >
          <div
            class="flex justify-between items-center gap-x-6 w-50 text-center"
          >
            <img :src="btn.icon" class="w-10 h-10 mb-2" />
            <span class="text-xl font-semibold w-50">{{ btn.label }}</span>
          </div>
        </router-link>
        <!-- v-if="auth.user?.rol === 'admin'" -->
        <router-link
          to="/login"
          class="w-full md:w-[280px] rounded-xl py-6 shadow-lg hover:opacity-90 transition flex justify-center items-center border-2 border-transparent hover:border-primary  hover:border-2 duration-300 delay-75 lg:gap-x-4"
        >
          <div
            class="flex justify-between items-center gap-x-6 w-50 text-center "
          >
            <img src="/iconControl.png" class="w-12  h-10 mb-2" />
            <span class="text-xl font-semibold w-50">Control de usuario</span>
          </div>
        </router-link>
      </div>
    </article>
  </section>
</template>
