<script setup></script>


<template>

  <div class="">
    <header class="navbar bg-neutral  z-10 text-neutral-content">
      <div class="flex-1">
        <router-link to="/" class="btn btn-ghost text-xl md:text-2xl">Inventario</router-link>
      </div>
      <div class="flex  items-center">
        <h3 class="mr-4 ">Hola, user  </h3>
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
    <div class="tooltip mt-10  ">
    <div
      class="tooltip-content flex items-center justify-center space-x-2 flex-row-reverse mt-10"
    >
      <div class="animate-bounce text-base-100 font-black">
        Fuera de servicio!!!
      </div>
    </div>

    <figcaption class="font-bold flex items-center justify-center space-x-2 text-xl">
      ¿Necesita asistencia?
      <img class="w-14 p-2" src="/Group.png" />
    </figcaption>
  </div>
  </footer>
</template>
