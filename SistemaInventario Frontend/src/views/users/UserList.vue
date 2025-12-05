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

// filtro
const usuariosFiltrados = computed(() => {
  return usuarioStore.usuarios.filter((u) =>
    u.NombreCompleto.toLowerCase().includes(buscar.value.toLowerCase())
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
  <div class="p-6">

    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold">Gestión de usuarios</h1>

      <button class="btn btn-primary" @click="crearUsuario">
        Crear usuario
      </button>
    </div>

    <input
      v-model="buscar"
      type="text"
      placeholder="Buscar usuario..."
      class="input input-bordered w-full max-w-xs mb-4"
    />

    <table class="table w-full">
      <thead class="bg-base-200 text-sm">
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
      </tbody>

    </table>
  </div>
</template>
