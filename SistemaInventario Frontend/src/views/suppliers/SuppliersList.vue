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
  <div class="min-h-screen bg-white p-5">

    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <!-- ICON -->
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/>
            <circle cx="9" cy="7" r="4"/>
            <path d="M23 21v-2a4 4 0 0 0-3-3.87"/>
            <path d="M16 3.13a4 4 0 0 1 0 7.75"/>
          </svg>
        </div>

        <h1 class="text-3xl font-medium text-neutral">Gestión de Proveedores</h1>
      </div>
    </div>

    <!-- CONTAINER -->
    <div class="max-w-6xl mx-auto bg-base-100 p-8 rounded-xl shadow">

      <!-- CONTROLS -->
      <div class="flex items-center justify-between mb-6">

        <!-- SEARCH -->
        <div class="w-40">
          <input
            type="text"
            v-model="searchQuery"
            placeholder="🔍 Buscar"
            class="input input-bordered w-full"
          >
        </div>

        <!-- BUTTON -->
        <button 
          class="btn btn-primary rounded-full px-8"
          @click="btnCrearProveedor"
        >
          Crear Proveedor
        </button>
      </div>

      <!-- TABLE -->
      <div class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">

          <thead class="bg-base-200">
            <tr>
              <th>ID</th>
              <th>Empresa</th>
              <th>Contacto</th>
              <th>Teléfono</th>
              <th>Email</th>
              <th>Dirección</th>
              <th>Acción</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="proveedor in filteredProveedores" :key="proveedor.ProveedorId">
              <td>{{ proveedor.ProveedorId }}</td>
              <td>{{ proveedor.NombreEmpresa }}</td>
              <td>{{ proveedor.NombreContacto ?? "-" }}</td>
              <td>{{ proveedor.Telefono ?? "-" }}</td>
              <td>{{ proveedor.Email ?? "-" }}</td>
              <td>{{ proveedor.Direccion ?? "-" }}</td>

              <!-- ACTIONS -->
              <td class="flex gap-2">

                <!-- EDIT BUTTON -->
                <button 
                  class="btn btn-sm btn-ghost"
                  @click="editarProveedor(proveedor)"
                  title="Editar"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                       fill="none" viewBox="0 0 24 24" stroke="currentColor"
                       stroke-width="2">
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                  </svg>
                </button>

                <!-- DELETE BUTTON -->
                <button 
                  class="btn btn-sm btn-error text-white"
                  @click="handleRemove(proveedor.ProveedorId)"
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
            <tr v-if="filteredProveedores.length === 0">
              <td colspan="7" class="text-center py-4">No hay proveedores</td>
            </tr>
          </tbody>
        </table>
      </div>

    </div>
  </div>
</template>
