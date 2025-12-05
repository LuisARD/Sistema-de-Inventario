<script setup>

import { useAuthStore } from '../store/auth';

const auth = useAuthStore();




const buttones = [
  { to: "/products", label: "Gestión de Productos", icon: "/camion.svg" },
  { to: "/suppliers", label: "Gestión de Proveedores", icon: "/camion.svg" },
  { to: "/products", label: "Control de Inventario", icon: "/btn1.svg" },
  { to: "/existencias", label: "Control de Existencias", icon: "/btn1.svg" },
  { to: "/movements", label: "Movimientos de Inventario", icon: "/camion.svg" },
  { to: "/categories", label: "Gestión de Categorías", icon: "/btn1.svg" },
  { to: "/reports", label: "Reportes", icon: "/iconReporte.svg" },
];


</script>

<template>
  <section
    class="flex flex-col items-center w-full px-4 sm:px-6 md:px-8 md:mt-20"
  >
   
    <article class="w-full max-w-6xl mt-6 md:mt-0">
      <div
        class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6 place-items-center"
      >
        <!-- BOTONES DINÁMICOS -->
        <router-link
          v-for="btn in buttones"
          :key="btn.to"
          :to="btn.to"
          class="
            w-full
            max-w-xs
            sm:max-w-none
            h-[90px]
            sm:h-[100px]
            rounded-xl
            py-4
            px-4
            shadow-lg
            transition
            flex
            justify-center
            items-center
            border-2
            border-transparent
            hover:border-primary
            hover:opacity-90
            duration-300
          "
        >
          <div class="flex items-center gap-3 sm:gap-4 text-center">
            <img
              :src="btn.icon"
              class="w-8 h-8 sm:w-10 sm:h-10 object-contain brightness-0 saturate-100"
              style="filter: invert(27%) sepia(51%) saturate(999%) hue-rotate(200deg) brightness(104%) contrast(10%);"
            />
            <span class="text-sm sm:text-base md:text-lg font-semibold">
              {{ btn.label }}
            </span>
          </div>
        </router-link>
       
        <!-- BOTÓN CONTROL DE USUARIO -->
        <router-link
          v-if="['Admin', 'Supervisor'].includes(auth.user?.RolNombre)"
          to="usuario"
          class="
            w-full
            max-w-xs
            sm:max-w-none
            h-[90px]
            sm:h-[100px]
            rounded-xl
            py-4
            px-4
            shadow-lg
            transition
            flex
            justify-center
            items-center
            border-2
            border-transparent
            hover:border-primary
            hover:opacity-90
            duration-300
          "
        >
          <div class="flex items-center gap-3 sm:gap-4 text-center">
            <img
              src="/iconControl.png"
              class="w-8 h-8 sm:w-10 sm:h-10 object-contain brightness-0 saturate-100"
              style="filter: invert(27%) sepia(51%) saturate(2878%) hue-rotate(200deg) brightness(104%) contrast(97%);"
            />
            <span class="text-sm sm:text-base md:text-lg font-semibold">
              Control de usuario
            </span>
          </div>
        </router-link>

      </div>
    </article>
  </section>
</template>
