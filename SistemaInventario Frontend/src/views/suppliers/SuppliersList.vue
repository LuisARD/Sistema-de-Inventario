<script setup>
import { onMounted, ref, computed } from "vue";
import { useProveedoresStore } from "../../store/proveedores";
import { useRouter } from "vue-router";
import { useCrudForm } from "../../composable/useCrudForm";

const proveedoresStore = useProveedoresStore();
const searchQuery = ref("");
const proveedorBase = {
  nombre_empresa: "",
  nombre_contacto: "",
  telefono: "",
  email: "",
  direccion: "",
};
const formRef = ref(null);
const router = useRouter();

const { handleRemove, handleEdit } = useCrudForm(proveedoresStore, proveedorBase, formRef);

// Cargar proveedores al montar
onMounted(() => {
  proveedoresStore.fetchProveedores();
});

// Filtrado simple: busca en todos los campos
const filteredProveedores = computed(() => {
  const q = searchQuery.value?.trim().toLowerCase();
  if (!q) return proveedoresStore.proveedores;
  return proveedoresStore.proveedores.filter((p) =>
    Object.values(p).some((v) =>
      String(v ?? "").toLowerCase().includes(q)
    )
  );
});

const btnCrearProveedor = () => {  
  proveedoresStore.proveedorActual = null;
  router.push('/suppliers/create');
}

const editarProveedor = (proveedor) => {
  proveedoresStore.proveedorActual = proveedor;
  router.push('/suppliers/create');
}
</script>

<template>
  <main class="min-h-screen p-5">

    <!-- HEADER SEMÁNTICO -->
    <header class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">

        <figure class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"
               aria-hidden="true">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
          </svg>
        </figure>

        <h1 class="text-3xl font-medium text-neutral">
          Gestión de Proveedores
        </h1>
      </div>
    </header>

    <!-- CONTENIDO PRINCIPAL -->
    <section class="max-w-6xl mx-auto bg-base-100 p-8 rounded-xl shadow">

      <!-- CONTROLES -->
      <nav class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">

        <!-- BUSCADOR -->
        <form class="w-full sm:w-40" role="search">
          <input
            type="search"
            v-model="searchQuery"
            placeholder="🔍 Buscar"
            class="input input-bordered w-full"
            aria-label="Buscar proveedor"
          >
        </form>

        <!-- BOTÓN -->
        <button 
          class="btn btn-info rounded-full px-8 w-full sm:w-auto"
          @click="btnCrearProveedor"
        >
          Crear Proveedor
        </button>
      </nav>

      <!-- TABLA -->
      <section class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full" aria-label="Listado de proveedores">

          <thead class="bg-base-200">
            <tr>
              <th scope="col">ID</th>
              <th scope="col">Empresa</th>
              <th scope="col">Contacto</th>
              <th scope="col">Teléfono</th>
              <th scope="col">Email</th>
              <th scope="col">Dirección</th>
              <th scope="col">Acción</th>
            </tr>
          </thead>

          <tbody>
            <tr 
              v-for="proveedor in filteredProveedores"
              :key="proveedor.ProveedorId"
            >
              <td>{{ proveedor.ProveedorId }}</td>
              <td>{{ proveedor.NombreEmpresa }}</td>
              <td>{{ proveedor.NombreContacto ?? "-" }}</td>
              <td>{{ proveedor.Telefono ?? "-" }}</td>
              <td>{{ proveedor.Email ?? "-" }}</td>
              <td>{{ proveedor.Direccion ?? "-" }}</td>

              <!-- ACCIONES -->
              <td class="flex gap-2">

                <!-- EDITAR -->
                <button 
                  class="btn btn-sm btn-ghost"
                  @click="editarProveedor(proveedor)"
                  aria-label="Editar proveedor"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                       fill="none" viewBox="0 0 24 24" stroke="currentColor"
                       stroke-width="2" aria-hidden="true">
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                  </svg>
                </button>

                <!-- ELIMINAR -->
                <button 
                  class="btn btn-sm btn-error text-white"
                  @click="handleRemove(proveedor.ProveedorId)"
                  aria-label="Eliminar proveedor"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                       fill="none" viewBox="0 0 24 24" stroke="currentColor"
                       stroke-width="2" aria-hidden="true">
                    <polyline points="3 6 5 6 21 6"/>
                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                  </svg>
                </button>

              </td>
            </tr>

            <!-- SIN DATOS -->
            <tr v-if="filteredProveedores.length === 0">
              <td colspan="7" class="text-center py-4">
                No hay proveedores
              </td>
            </tr>

          </tbody>
        </table>
      </section>

    </section>
  </main>
</template>
