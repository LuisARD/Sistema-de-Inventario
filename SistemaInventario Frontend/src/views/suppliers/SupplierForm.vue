<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { FormKit, defaultConfig } from "@formkit/vue";
import { FormKitSchema } from "@formkit/vue";
import { useProveedoresStore } from "../../store/proveedores";
import { proveedorSchema } from "../../schema/proveedoresSchema";
import { useCrudForm } from "../../composable/useCrudForm";

const router = useRouter();
const proveedorStore = useProveedoresStore();
const formRef = ref(null);

const proveedorBase = {
  nombre_empresa: "",
  nombre_contacto: "",
  telefono: "",
  email: "",
  direccion: "",
};

const { formData, editId, saveEdit, handleSubmit } = useCrudForm(
  proveedorStore,
  proveedorBase,
  formRef
);

const formDataForFormKit = computed(() => ({}));

onMounted(() => {
  if (proveedorStore.proveedorActual) {
    const p = proveedorStore.proveedorActual;

    Object.assign(formData.value, {
      nombre_empresa: p.NombreEmpresa ?? "",
      nombre_contacto: p.NombreContacto ?? "",
      telefono: p.Telefono ?? "",
      email: p.Email ?? "",
      direccion: p.Direccion ?? "",
    });

    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...formData.value });
    }

    editId.value = p.ProveedorId;
  }
});

const onSubmit = (data) => {
  const payload = {
    ProveedorId: editId.value,
    NombreEmpresa: data.nombre_empresa,
    NombreContacto: data.nombre_contacto,
    Telefono: data.telefono,
    Email: data.email,
    Direccion: data.direccion,
  };

  console.log('Editando proveedor:', editId.value, payload);

  if (editId.value) {
    saveEdit(payload);
  } else {
    handleSubmit({
      NombreEmpresa: data.nombre_empresa,
      NombreContacto: data.nombre_contacto,
      Telefono: data.telefono,
      Email: data.email,
      Direccion: data.direccion,
    });
  }

  proveedorStore.proveedorActual = null;
  formRef.value?.node?.reset();
  router.push('/suppliers');
};
</script>

<template>
  <div class="min-h-screen p-6 flex justify-center">
    <div class="card w-full max-w-3xl bg-base-100 shadow p-8">

      <h1 class="text-2xl font-semibold mb-6 flex items-center gap-3">
        <img src="/btn1.svg" class="w-7 h-7" />
        {{ editId ? 'Editar Proveedor' : 'Crear Proveedor' }}
      </h1>

      <FormKit
        ref="formRef"
        type="form"
        :actions="false"
        @submit="onSubmit"
        class="space-y-4"
      >
        <FormKitSchema :schema="proveedorSchema" :data="formDataForFormKit" />

        <div class="flex justify-end gap-3 pt-4">
          <button
            type="button"
            class="btn btn-ghost rounded-full px-10"
            @click="router.push('/suppliers')"
          >
            Cancelar
          </button>
          <button
            type="submit"
            class="btn btn-primary rounded-full px-8"
          >
            {{ editId ? 'Actualizar Proveedor' : 'Crear Proveedor' }}
          </button>
        </div>
      </FormKit>
    </div>
  </div>
</template>
