<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useCrudForm } from "../../composable/useCrudForm";
import { useUsuarioStore } from "../../store/usuario";

const router = useRouter();
const buscar = ref("");

const usuarioStore = useUsuarioStore();
const usuarioBase = {
  nombre: "",
  email: "",
  password: "",
  rol: "",
};

// reutilizamos la lógica de productos ❤️
const { handleRemove, handleEdit } = useCrudForm(
  usuarioStore,
  usuarioBase,
  null
);

onMounted(() => {
  usuarioStore.fetchUsuario();
});

// filtro mejorado - busca en todos los campos
const usuariosFiltrados = computed(() => {
  const q = buscar.value?.trim().toLowerCase();
  if (!q) return usuarioStore.usuarios;
  
  return usuarioStore.usuarios.filter((u) =>
    Object.values(u).some((v) =>
      String(v ?? "").toLowerCase().includes(q)
    )
  );
});

const crearUsuario = () => {
  usuarioStore.usuarioActual = null;
  router.push("/usuario/create");
};

const editarUsuario = (u) => {
  usuarioStore.usuarioActual = u;
  router.push("/usuario/create");
};
</script>

<template>
  <div class="min-h-screen bg-white p-5">

    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" 
                  d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"/>
          </svg>
        </div>
        <h1 class="text-3xl font-medium text-neutral">Gestión de Usuarios</h1>
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
            v-model="buscar"
            placeholder="🔍 Buscar"
            class="input input-bordered w-full"
          >
        </div>

        <!-- BUTTON -->
        <button 
          class="btn btn-primary rounded-full px-8"
          @click="crearUsuario"
        >
          Crear Usuario
        </button>
      </div>

      <!-- TABLE -->
      <div class="overflow-x-auto rounded-lg border border-base-300">

        <table class="table table-zebra w-full">
          <thead class="bg-base-200">
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Email</th>
          <th>Rol</th>
          <th>Activo</th>
          <th class="text-center">Acciones</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="u in usuariosFiltrados" :key="u.UsuarioId">
          <td>{{ u.UsuarioId }}</td>
          <td>{{ u.NombreCompleto }}</td>
          <td>{{ u.Email }}</td>
          <td>{{ u.RolNombre }}</td>

          <td>
            <span
              :class="{
                'badge bg-green-500': u.Activo,
                'badge bg-red-500': !u.Activo
              }"
              class="h-4 w-4 rounded-full inline-block"
            ></span>
          </td>

          <td class="flex gap-2 justify-center">
            <button class="btn btn-sm" @click="editarUsuario(u)">
              Editar
            </button>

            <button class="btn btn-sm btn-error" @click="handleRemove(u.UsuarioId)">
              Eliminar
            </button>
          </td>
        </tr>
        
        <!-- Sin usuarios -->
        <tr v-if="usuariosFiltrados.length === 0">
          <td colspan="6" class="text-center py-4">No hay usuarios</td>
        </tr>
      </tbody>

    </table>
      </div>
    </div>
  </div>
</template>
