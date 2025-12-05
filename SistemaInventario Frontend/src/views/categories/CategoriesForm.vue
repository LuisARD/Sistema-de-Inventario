<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useCategoríasStore } from "../../store/categorias";
import { categoriaSchema } from "../../schema/categoriaSchema";
import { useCrudForm } from "../../composable/useCrudForm";

const router = useRouter();
const categoriasStore = useCategoríasStore();
const formRef = ref(null);

const categoriaBase = {
  nombre: "",
  descripcion: "",
};

const { formData, editId, saveEdit, handleSubmit } = useCrudForm(
  categoriasStore,
  categoriaBase,
  formRef
);

onMounted(() => {
  if (categoriasStore.categoriaActual) {
    const c = categoriasStore.categoriaActual;

    Object.assign(formData.value, {
      nombre: c.Nombre ?? "",
      descripcion: c.Descripcion ?? "",
    });

    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...formData.value });
    }

    editId.value = c.CategoriaId;
  }
});

const onSubmit = (data) => {
  const payload = {
    CategoriaId: editId.value,
    Nombre: data.nombre || "",
    Descripcion: data.descripcion || ""
  };

  console.log('Editando categoría:', editId.value, 'Payload:', payload);

  if (editId.value) {
    saveEdit(payload);
  } else {
    handleSubmit({
      Nombre: data.nombre || "",
      Descripcion: data.descripcion || ""
    });
  }

  categoriasStore.categoriaActual = null;
  formRef.value?.node?.reset();
  router.push('/categories');
};
</script>

<template>
  <div class="min-h-screen p-6 flex justify-center">
    <div class="card w-full max-w-3xl bg-base-100 shadow p-8">
      <h1 class="text-2xl font-semibold mb-6 flex items-center gap-3">
        <img src="/iconTitle.svg" class="w-7 h-7" />
        {{ editId ? 'Editar Categoría' : 'Crear Categoría' }}
      </h1>

      <FormKit
        ref="formRef"
        type="form"
        :actions="false"
        @submit="onSubmit"
        class="space-y-6"
      >
        <FormKitSchema :schema="categoriaSchema" />

        <div class="flex justify-end gap-3 pt-4">
          <button
            type="button"
            class="btn btn-ghost rounded-full px-10"
            @click="router.push('/categories')"
          >
            Cancelar
          </button>
          <button
            type="submit"
            class="btn btn-info rounded-full px-10"
          >
            {{ editId ? 'Actualizar Categoría' : 'Crear Categoría' }}
          </button>
        </div>
      </FormKit>
    </div>
  </div>
</template>
