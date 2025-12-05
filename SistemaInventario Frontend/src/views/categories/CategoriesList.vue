<script setup>
import { onMounted, ref, computed } from "vue";
import { useCategoríasStore } from "../../store/categorias";
import { useRouter } from "vue-router";
import { useCrudForm } from "../../composable/useCrudForm";

const categoriasStore = useCategoríasStore();
const searchQuery = ref("");
const categoriaBase = {
  nombre: "",
  descripcion: "",
};
const formRef = ref(null);
const router = useRouter();

const { handleRemove, handleEdit } = useCrudForm(categoriasStore, categoriaBase, formRef);

// Cargar categorías al montar
onMounted(() => {
  categoriasStore.fetchCategorías();
});

// Filtrado simple: busca en nombre y descripción
const filteredCategorias = computed(() => {
  const q = searchQuery.value?.trim().toLowerCase();
  if (!q) return categoriasStore.categorias;
  return categoriasStore.categorias.filter((c) =>
    Object.values(c).some((v) =>
      String(v ?? "").toLowerCase().includes(q)
    )
  );
});

const btnCrearCategoria = () => {  
  categoriasStore.categoriaActual = null;
  router.push('/categories/create');
}

const editarCategoria = (categoria) => {
  categoriasStore.categoriaActual = categoria;
  router.push('/categories/create');
}
</script>

<template>
  <main class="min-h-screen p-4 sm:p-6">

    <!-- HEADER -->
    <header class="max-w-6xl mx-auto mb-6">
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 p-4 sm:p-5 bg-base-100 shadow rounded-xl">

        <div class="flex items-center gap-4">
          <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
              viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
              <path d="M3 7v10a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2h-6l-2-2H5a2 2 0 0 0-2 2z"/>
            </svg>
          </div>

          <h1 class="text-xl sm:text-2xl lg:text-3xl font-medium text-neutral">
            Gestión de Categorías
          </h1>
        </div>

      </div>
    </header>

    <!-- CONTENIDO -->
    <section class="max-w-6xl mx-auto bg-base-100 p-4 sm:p-6 lg:p-8 rounded-xl shadow">

      <!-- CONTROLES -->
      <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">

        <div class="w-full sm:w-56">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="🔍 Buscar"
            class="input input-bordered w-full"
          >
        </div>

        <button
          class="btn btn-info rounded-full px-6 sm:px-8 w-full sm:w-auto"
          @click="btnCrearCategoria"
        >
          Crear Categoría
        </button>
      </div>

      <!-- TABLA -->
      <div class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full min-w-[500px]">

          <thead class="bg-base-200">
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Descripción</th>
              <th class="text-center">Acción</th>
            </tr>
          </thead>

          <tbody>
            <tr
              v-for="categoria in filteredCategorias"
              :key="categoria.CategoriaId"
            >
              <td>{{ categoria.CategoriaId }}</td>
              <td class="font-medium">{{ categoria.Nombre }}</td>
              <td>{{ categoria.Descripcion ?? "-" }}</td>

              <td class="flex justify-center gap-2">
                <button
                  class="btn btn-sm btn-ghost"
                  @click="editarCategoria(categoria)"
                  title="Editar"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                    fill="none" viewBox="0 0 24 24" stroke="currentColor"
                    stroke-width="2">
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                  </svg>
                </button>

                <button
                  class="btn btn-sm btn-error text-white"
                  @click="handleRemove(categoria.CategoriaId)"
                  title="Eliminar"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                    fill="none" viewBox="0 0 24 24" stroke="currentColor"
                    stroke-width="2">
                    <polyline points="3 6 5 6 21 6"/>
                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                  </svg>
                </button>
              </td>
            </tr>

            <tr v-if="filteredCategorias.length === 0">
              <td colspan="4" class="text-center py-4 text-sm opacity-70">
                No hay categorías
              </td>
            </tr>
          </tbody>
        </table>
      </div>

    </section>

  </main>
</template>
