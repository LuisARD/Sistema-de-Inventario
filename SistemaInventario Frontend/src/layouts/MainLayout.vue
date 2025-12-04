<script setup>
import BtnAsistencia from '../components/btnAsistencia.vue';

import { useAuthStore } from "../store/auth";
const auth = useAuthStore();
console.log(auth.user);




const handleLogout = () => {
  auth.logout();
};

</script>


<template>

  <div class="">
    <header class="navbar bg-neutral  z-10 text-neutral-content">
      <div class="flex-1">
        <router-link to="/" class="btn btn-ghost text-xl md:text-2xl">Inventario</router-link>
      </div>
      <div class="flex  items-center">
        <h3 class="mr-4 ">Hola, {{ auth.user?.NombreCompleto  }} </h3>
        <img
          src="/iconUser.svg"
          alt="Avatar"
          class="w-10 h-10 rounded-full mr-4"
        />
        <button @click="handleLogout" class="btn    ">Cerrar sesión</button>
      </div>
    </header>
    </div>

  <main class="main-layout">
    <router-view />
  </main>

 <footer
    class="footer sm:footer-horizontal items-center p-4 flex justify-end"
  >
    <BtnAsistencia />
  </footer>
</template>
