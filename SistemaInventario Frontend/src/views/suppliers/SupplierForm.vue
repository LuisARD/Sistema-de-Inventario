<template>
  <div class="min-h-screen bg-white p-5">
    <!-- Header -->
    <div class="flex justify-between items-center mb-10">
      <div class="flex items-center gap-4">
        <div class="w-10 h-10 bg-orange-500 rounded-lg flex items-center justify-center text-white text-2xl">
          +
        </div>
        <h1 class="text-3xl text-gray-800 font-light">Proveedores de productos</h1>
      </div>
      <div class="flex items-center gap-4">
        <span class="text-sm text-gray-600">¡Hola, Janet!</span>
        <div class="w-8 h-8 border-2 border-gray-600 rounded-full flex items-center justify-center">
          👤
        </div>
        <button class="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-full text-sm transition">
          Cerrar sesión
        </button>
      </div>
    </div>

    <!-- Formulario -->
    <form @submit.prevent="crearProveedor" class="max-w-4xl">
      <div class="grid grid-cols-2 gap-6 mb-6">
        <!-- Nombre de la empresa -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Nombre de la empresa</label>
          <input
            v-model="proveedor.nombreEmpresa"
            type="text"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Nombre del contacto -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Nombre del contacto</label>
          <input
            v-model="proveedor.nombreContacto"
            type="text"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Teléfono -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Teléfono</label>
          <input
            v-model="proveedor.telefono"
            type="tel"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Email -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Email</label>
          <input
            v-model="proveedor.email"
            type="email"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Dirección (ocupa toda la fila) -->
        <div class="flex flex-col col-span-2">
          <label class="text-sm text-gray-700 mb-2">Dirección</label>
          <input
            v-model="proveedor.direccion"
            type="text"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
          />
        </div>
      </div>

      <!-- Botón -->
      <div class="flex justify-end mt-12">
        <button
          type="submit"
          class="bg-orange-500 hover:bg-orange-600 text-white px-8 py-3 rounded-full text-base transition"
        >
          Crear Proveedores
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
  name: 'SuppliersForm',
  data() {
    return {
      proveedor: {
        nombreEmpresa: '',
        nombreContacto: '',
        telefono: '',
        email: '',
        direccion: ''
      }
    }
  },
  methods: {
    crearProveedor() {
      console.log('Proveedor creado:', this.proveedor)
      // Aquí puedes hacer la llamada a tu API
      this.limpiarFormulario()
    },
    limpiarFormulario() {
      this.proveedor = {
        nombreEmpresa: '',
        nombreContacto: '',
        telefono: '',
        email: '',
        direccion: ''
      }
    }
  }
}
</script>

<style scoped>
/* Estilos adicionales si los necesitas */
</style>