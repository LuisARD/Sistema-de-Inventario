<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useCategoríasStore } from "../../store/categorias";
import { categoriaSchema } from "../../schema/categoriaSchema";

const router = useRouter();
const categoriasStore = useCategoríasStore();

const formRef = ref(null);

// Base para limpiar el formulario
const categoriaBase = {
  nombre: "",
  descripcion: "",
};

// Modelo reactivo (solo para inspección si quieres)
const categoria = ref({ ...categoriaBase });

// Si viene de editar, solo PRELLENA, pero siempre crea
onMounted(() => {
  const c = categoriasStore.categoriaActual;

  if (c) {
    const values = {
      nombre: c.CategoriaNombre ?? c.nombre ?? "",
      descripcion: c.Descripcion ?? c.descripcion ?? "",
    };

    Object.assign(categoria.value, values);

    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...values });
    }
  }
});

// Crear categoría (NO EDITA)
const crearCategoria = async () => {
  try {
    const data = formRef.value?.node?.value ?? categoria.value;

    const payload = {
      Nombre: data.nombre,
      Descripcion: data.descripcion,
    };

    await categoriasStore.addItem(payload);

    categoriasStore.categoriaActual = null;

    // Reset visual y reactivo
    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...categoriaBase });
    }
    Object.assign(categoria.value, categoriaBase);

    
  } catch (e) {
    console.error("Error al crear categoría:", e);
  }
};
</script>

<template>
  <div class="min-h-screen p-6 flex justify-center">
    <div class="card w-full max-w-3xl bg-base-100 shadow p-8">
      <h1 class="text-2xl font-semibold mb-6 flex items-center gap-3">
        <img src="/btn1.svg" class="w-7 h-7" />
        Crear Categoría
      </h1>

      <FormKit
        ref="formRef"
        type="form"
        :actions="false"
        @submit="crearCategoria"
        class="space-y-6"
      >
        <FormKitSchema :schema="categoriaSchema" />

        <div class="flex justify-end pt-4">
          <button
            type="submit"
            class="btn btn-primary rounded-full px-10"
          >
            Crear Categoría
          </button>
        </div>
      </FormKit>
    </div>
  </div>
</template>
