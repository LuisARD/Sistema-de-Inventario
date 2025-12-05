<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { usuarioSchema } from "../../schema/usuarioSchema";
import { useUsuarioStore } from "../../store/usuario";
import { useCrudForm } from "../../composable/useCrudForm";

const formRef = ref(null);
const router = useRouter();
const usuarioStore = useUsuarioStore();

// Objeto base
const usuarioBase = {
  nombre: "",
  email: "",
  password: "",
  rol: "",
  activo: true,
};

// useCrudForm reutilizado
const { formData, editId, handleSubmit, saveEdit } = useCrudForm(
  usuarioStore,
  usuarioBase,
  formRef
);

// Schema dinámico según modo edición
const schema = computed(() => usuarioSchema(!!editId.value));

// Cargar datos al editar
onMounted(() => {
  if (usuarioStore.usuarioActual) {
    const u = usuarioStore.usuarioActual;
    // llenar datos
    Object.assign(formData.value, {
      UsuarioId: u.UsuarioId,
      nombre: u.NombreCompleto,
      email: u.Email,
      password: "", // nunca mostrar la real
      rol:  u.RolId ?? u.rol,
      activo: u.Activo ?? true,
    });

    // setear en FormKit
    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...formData.value });
    }

    editId.value = u.UsuarioId;
  }
});

// Enviar formulario
const onSubmit = (data) => {
  // Asegurar que RolId sea un número
  const rolId = typeof data.rol === 'string' ? parseInt(data.rol, 10) : Number(data.rol);
  
  const payload = {
    NombreCompleto: data.nombre,
    Email: data.email,
    RolId: rolId,
    Activo: Boolean(data.activo),
  };

  if (editId.value) {
    // Modo edición: incluir Password solo si se proporcionó una nueva
    const payloadEdit = {
      UsuarioId: editId.value,
      ...payload
    };
    
    // Solo agregar Password si el usuario escribió algo
    if (data.password && data.password.trim() !== '') {
      payloadEdit.Password = data.password;
    }
    
    console.log('Payload edición enviado:', payloadEdit);
    saveEdit(payloadEdit);
  } else {
    // Modo creación: Password es obligatorio
    if (!data.password || data.password.trim() === '') {
      alert('La contraseña es obligatoria para crear un usuario');
      return;
    }
    payload.Password = data.password;
    console.log('Payload creación enviado:', payload);
    handleSubmit(payload);
  }

  formRef.value?.node.reset();
  router.push("/usuario");
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
                  d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"/>
          </svg>
        </div>
        <h1 class="text-3xl font-medium text-neutral">
          {{ editId ? "Editar Usuario" : "Crear Usuario" }}
        </h1>
      </div>
    </div>

    <!-- CONTAINER -->
    <div class="max-w-4xl mx-auto bg-base-100 p-8 rounded-xl shadow">
      
      <FormKit ref="formRef" type="form" :actions="false" @submit="onSubmit">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <FormKitSchema :schema="schema" />
        </div>

        <div class="flex gap-4 justify-end mt-8">
          <button 
            type="button" 
            @click="router.push('/usuario')" 
            class="btn btn-outline"
          >
            Cancelar
          </button>
          <FormKit
            type="submit"
            :label="editId ? 'Actualizar Usuario' : 'Guardar Usuario'"
            input-class="btn btn-primary px-8"
          />
        </div>
      </FormKit>
    </div>
  </div>
</template>
