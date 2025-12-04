<template>
  <div class="min-h-screen bg-white p-5">
    <!-- Header -->
    <div class="flex justify-between items-center mb-10">
      <div class="flex items-center gap-4">
        <div class="w-10 h-10 bg-orange-500 rounded-lg flex items-center justify-center text-white text-2xl">
          +
        </div>
        <h1 class="text-3xl text-gray-800 font-light">Control de usuarios</h1>
      </div>
      <div class="flex items-center gap-4">
        <span class="text-sm text-gray-600">¡Hola, Bienvenido!</span>
        <div class="w-8 h-8 border-2 border-gray-600 rounded-full flex items-center justify-center">
          👤
        </div>
        <button 
          @click="cerrarSesion"
          class="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-full text-sm transition"
        >
          Cerrar sesión
        </button>
      </div>
    </div>

    <!-- Formulario -->
    <form @submit.prevent="crearUsuario" class="max-w-4xl">
      <div class="grid grid-cols-2 gap-6 mb-8">
        <!-- Nombre completo -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Nombre completo</label>
          <input
            v-model="usuario.nombreCompleto"
            type="text"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Email -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Email</label>
          <input
            v-model="usuario.email"
            type="email"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Password -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Password</label>
          <input
            v-model="usuario.password"
            type="password"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Rol -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">rol</label>
          <select
            v-model="usuario.rol"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition appearance-none bg-white cursor-pointer"
            style="background-image: url('data:image/svg+xml,%3Csvg width=\'12\' height=\'8\' viewBox=\'0 0 12 8\' fill=\'none\' xmlns=\'http://www.w3.org/2000/svg\'%3E%3Cpath d=\'M1 1.5L6 6.5L11 1.5\' stroke=\'%23666\' stroke-width=\'2\' stroke-linecap=\'round\'/%3E%3C/svg%3E'); background-repeat: no-repeat; background-position: right 15px center; padding-right: 40px;"
            required
          >
            <option value="">Seleccione un rol</option>
            <option value="administrador">Administrador</option>
            <option value="usuario">Usuario</option>
            <option value="vendedor">Vendedor</option>
            <option value="almacenista">Almacenista</option>
          </select>
        </div>
      </div>

      <!-- Botón -->
      <div class="flex justify-end mt-12">
        <button
          type="submit"
          class="bg-orange-500 hover:bg-orange-600 text-white px-8 py-3 rounded-full text-base transition"
        >
          Crear usuarios
        </button>
      </div>
    </form>

    <!-- Footer -->
    <div class="fixed bottom-8 right-8 flex items-center gap-2 text-sm text-gray-400">
      <span>¿Necesita asistencia?</span>
      <div class="w-6 h-6 border-2 border-gray-400 rounded-full flex items-center justify-center text-xs">
        🎧
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UsersForm',
  data() {
    return {
      usuario: {
        nombreCompleto: '',
        email: '',
        password: '',
        rol: ''
      }
    }
  },
  methods: {
    crearUsuario() {
      console.log('Usuario creado:', this.usuario)
      // Aquí puedes hacer la llamada a tu API
      alert('Usuario creado exitosamente')
      this.limpiarFormulario()
    },
    limpiarFormulario() {
      this.usuario = {
        nombreCompleto: '',
        email: '',
        password: '',
        rol: ''
      }
    },
    cerrarSesion() {
      console.log('Cerrando sesión...')
      this.$router.push('/login')
    }
  }
}
</script>

<style scoped>
/* Estilos adicionales si los necesitas */
</style>