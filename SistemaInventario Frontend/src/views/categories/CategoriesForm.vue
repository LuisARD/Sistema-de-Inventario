<template>
  <div class="min-h-screen bg-white p-5">
    <!-- Header -->
    <div class="flex justify-between items-center mb-10">
      <div class="flex items-center gap-4">
        <div class="w-10 h-10 bg-orange-500 rounded-lg flex items-center justify-center text-white text-2xl">
          +
        </div>
        <h1 class="text-3xl text-gray-800 font-light">Categorías de productos</h1>
      </div>
      <div class="flex items-center gap-4">
        <span class="text-sm text-gray-600">¡Hola, Bienvenido!</span>
        <div class="w-8 h-8 border-2 border-gray-600 rounded-full flex items-center justify-center">
          👤
        </div>
        <button class="bg-orange-500 hover:bg-orange-600 text-white px-6 py-2 rounded-full text-sm transition">
          Cerrar sesión
        </button>
      </div>
    </div>

    <!-- Formulario -->
    <form @submit.prevent="crearCategoria" class="max-w-4xl">
      <div class="grid grid-cols-2 gap-6 mb-8">
        <!-- Nombre de la categoría -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Nombre de la categoría</label>
          <input
            v-model="categoria.nombre"
            type="text"
            class="border border-gray-300 rounded-lg px-4 py-3 focus:outline-none focus:border-orange-500 transition"
            required
          />
        </div>

        <!-- Descripción -->
        <div class="flex flex-col">
          <label class="text-sm text-gray-700 mb-2">Descripción</label>
          <input
            v-model="categoria.descripcion"
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
          Crear Categorias
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
  name: 'CategoriesForm',
  data() {
    return {
      categoria: {
        nombre: '',
        descripcion: ''
      }
    }
  },
  methods: {
    crearCategoria() {
      console.log('Categoría creada:', this.categoria)
      // Aquí puedes hacer la llamada a tu API
      this.limpiarFormulario()
    },
    limpiarFormulario() {
      this.categoria = {
        nombre: '',
        descripcion: ''
      }
    }
  }
}
</script>

<style scoped>
/* Estilos adicionales si los necesitas */
</style>