<script setup>
import { ref, onMounted } from "vue";
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
};

// useCrudForm reutilizado
const { formData, editId, handleSubmit, saveEdit } = useCrudForm(
  usuarioStore,
  usuarioBase,
  formRef
);

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
  const payload = {
    
    NombreCompleto: data.nombre,
    Email: data.email,
    Password: data.password,
    RolId: (data.rol),
  };

  if (editId.value) {
    delete payload.Password;
      const payloadEdit = {
    UsuarioId: editId.value,   // 👈 NECESARIO
    ...payload
  };

  saveEdit(payloadEdit);
  } else {
    handleSubmit(payload);
  }

  formRef.value?.node.reset();
  router.push("/usuario");
};

</script>

<template>
  <div class="min-h-screen p-6">
    <div class="max-w-3xl mx-auto bg-base-100 p-10 rounded-xl shadow">
      <h1 class="text-2xl font-semibold text-center mb-6">
        {{ editId ? "Editar Usuario" : "Crear Usuario" }}
      </h1>

      <FormKit ref="formRef" type="form" :actions="false" @submit="onSubmit">
        <FormKitSchema
          :schema="usuarioSchema"
          :classes="{ outer: 'grid grid-cols-1 md:grid-cols-2 gap-6' }"
        />

        <FormKit
          type="submit"
          :label="editId ? 'Actualizar' : 'Guardar'"
          input-class="btn btn-primary rounded-full px-10 mt-8"
        />
      </FormKit>
    </div>
  </div>
</template>
