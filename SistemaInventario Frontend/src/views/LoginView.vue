<template class="">
  <div class="absolute inset-0 bg-neutral"></div>
  <header class="navbar flex justify-end">
  <div class="tooltip mt-10  ">
    <div
      class="tooltip-content flex items-center justify-center space-x-2 flex-row-reverse mt-10"
    >
      <div class="animate-bounce text-base-100 font-black">
        Fuera de servicio!!!
      </div>
    </div>

    <figcaption class="text-white flex items-center justify-center space-x-2">
      ¿Necesita asistencia?
      <img class="w-14 p-2" src="/Group.png" />
    </figcaption>
  </div>
</header>


  <section
    class="bg-neutral flex flex-col justify-center items-center relative z-10 h-full"
  >
    <fieldset
      class="fieldset bg-base-200 border border-base-300 rounded-xl w-full max-w-md p-6 shadow text-neutral py-8"
    >

      <FormKit
        type="form"
        :actions="false"
        @submit="handleLogin"
        form-class="flex flex-col gap-4"
        incomplete-message="Por favor completa los campos."
      >
        <!-- Nombre -->
               <h3 class="fieldset-legend text-lg font-bold text-center  ">Iniciar Sesión</h3>

        <FormKit
          type="text"
          name="nombre"
          label="Nombre"
          v-model="nombre"
          validation="required"
          placeholder="Ingresa tu nombre"
          outerClass="flex flex-col gap-1"
          labelClass="font-semibold "
          inputClass="w-full px-3 py-2 rounded-md bg-base-100 shadow-sm focus:outline-none focus:ring-2 focus:ring-warning transition-all"
        />

  

        <!-- Contraseña -->
        <FormKit
          type="password"
          name="password"
          label="Contraseña"
          v-model="password"
          validation="required"
          placeholder="•••••••••"
          outerClass="flex flex-col gap-1"
          labelClass="font-semibold "
          inputClass="w-full px-3 py-2 rounded-md bg-base-100 shadow-sm focus:outline-none focus:ring-2 focus:ring-warning transition-all"
        />

        <!-- Botón -->
        <button
           
          type="submit"
          class="btn btn-primary w-full mt-4 flex justify-center"
          :disabled="loading"
        >
          <span v-if="loading" class="loading loading-spinner"></span>
          <span  v-else>Acceder</span>
        </button>

          <p v-if="error" class="text-error text-center mt-2">{{ error }}</p>
        </FormKit>
      </fieldset>

      <router-link to="/" class="text-white mt-4 underline">Volver</router-link>

    </section>

  </div>
</template>

<script>
export default {
  data() {
    return {
      nombre: "",
      password: "",
      loading: false,
      error: ""
    };
  },

  methods: {
    handleLogin() {
      this.loading = true;
      this.error = "";

      setTimeout(() => {
        this.loading = false;

        if (this.nombre === "" || this.password === "") {
          this.error = "Credenciales inválidas.";
          return;
        }

        this.$router.push("/");
      }, 1200);
    }
  }
};
</script>
