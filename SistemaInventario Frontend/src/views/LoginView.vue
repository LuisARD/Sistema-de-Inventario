<script setup lang="ts">
import { useRouter } from "vue-router";
import BtnAsistencia from "../components/btnAsistencia.vue";
import { useAuthStore } from "../store/auth";
import { ref } from "vue";

const auth = useAuthStore();
const router = useRouter();

const error = ref("");
const loading = ref(false);
const email = ref("");
const password = ref("");


const handleLogin = async (data) => {
  loading.value = true;
  error.value = "";

  const ok = await auth.login(email.value, password.value);

  loading.value = false;

  console.log(data);

  if (!ok) {
    error.value = "Credenciales incorrectas";
    return;
  }

  router.push("/");
};
</script>


<template class="">
   <div class="bg-login"> 
   <div class="absolute inset-0 bg-black/30"></div>
 
     

  

  <section
    class=" flex flex-col justify-center items-center relative z-10 h-full "
  >
    <fieldset
      class="fieldset bg-base-200 border border-base-300 rounded-xl w-full h-100 max-w-md p-6 shadow text-neutral py-8"
    >
      <FormKit
        type="form"
        :actions="false"
        @submit="handleLogin"
        form-class="flex flex-col gap-4"
        incomplete-message="Por favor completa los campos."
      >
        <!-- Nombre -->
        <h3 class="fieldset-legend text-lg font-bold text-center">
          Iniciar Sesión
        </h3>

        <FormKit
          type="email"
          name="email"
          label="Correo Electrónico"
          v-model="email"
          validation="required|email"
          placeholder="correo@ejemplo.com"
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
          <span v-else>Acceder</span>
        </button>

        <p v-if="error" class="text-error text-center mt-2 text-2xl font-bold">{{ error }}</p>
      </FormKit>
    </fieldset>
    <a to="/"></a>
  </section>
</div>
</template>

<!-- <script>
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
</script> -->
