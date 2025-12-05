<script setup>

import { useAuthStore } from '../store/auth';
import { computed } from 'vue';

const auth = useAuthStore();

const userRole = computed(() => auth.user?.RolNombre);

// Función para verificar si el usuario tiene acceso a una ruta
const hasAccess = (roles) => {
  if (!roles || roles.length === 0) return true;
  return roles.includes(userRole.value);
};

// Función para obtener el label correcto según el rol
const getButtonLabel = (button) => {
  const role = userRole.value;
  
  // Si no tiene permisos de escritura, cambiar "Gestión de" por "Ver"
  const hasWriteAccess = (button) => {
    if (button.to === '/categories') {
      return ['Admin', 'Supervisor'].includes(role);
    }
    if (button.to === '/products') {
      return ['Admin', 'Usuario', 'Supervisor'].includes(role);
    }
    if (button.to === '/suppliers') {
      return ['Admin', 'Supervisor'].includes(role);
    }
    if (button.to === '/almacenes') {
      return ['Admin', 'Supervisor'].includes(role);
    }
    if (button.to === '/movements') {
      return ['Admin', 'Supervisor'].includes(role);
    }
    return true;
  };
  
  // Cambiar "Gestión de" por "Ver" si no tiene permisos de escritura
  if (!hasWriteAccess(button) && button.label.startsWith('Gestión de')) {
    return button.label.replace('Gestión de', 'Ver');
  }
  
  return button.label;
};

const buttones = [
  { 
    to: "/products", 
    label: "Gestión de Productos", 
    icon: "/camion.svg",
    roles: ["Admin", "Usuario", "Supervisor"]
  },
  { 
    to: "/suppliers", 
    label: "Gestión de Proveedores", 
    icon: "/camion.svg",
    roles: ["Admin", "Supervisor"]
  },
  { 
    to: "/existencias", 
    label: "Control de Existencias", 
    icon: "/btn1.svg",
    roles: ["Admin", "Supervisor"]
  },
  { 
    to: "/almacenes", 
    label: "Gestión de Almacenes", 
    icon: "/btn1.svg",
    roles: ["Admin", "Supervisor"]
  },
  { 
    to: "/movements", 
    label: "Movimientos de Inventario", 
    icon: "/camion.svg",
    roles: ["Admin", "Usuario", "Supervisor"]
  },
  { 
    to: "/categories", 
    label: "Gestión de Categorías", 
    icon: "/btn1.svg",
    roles: ["Admin", "Usuario", "Supervisor"]
  },
  { 
    to: "/reports", 
    label: "Reportes", 
    icon: "/iconReporte.svg",
    roles: ["Admin", "Supervisor"]
  },
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
          v-show="hasAccess(btn.roles)"
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
              {{ getButtonLabel(btn) }}
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
