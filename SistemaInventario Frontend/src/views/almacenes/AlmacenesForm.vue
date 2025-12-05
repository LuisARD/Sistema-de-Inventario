<script setup>
import { ref, onMounted, nextTick } from "vue";
import { useRouter } from "vue-router";
import { useAlmacenesStore } from "../../store/almacenes";
import { almacenSchema } from "../../schema/almacenSchema";

const router = useRouter();
const almacenesStore = useAlmacenesStore();

const formData = ref({
  Nombre: "",
  Ubicacion: "",
  Activo: true,
});

const editId = ref(null);

onMounted(async () => {
  await nextTick();
  if (almacenesStore.almacenActual) {
    editId.value = almacenesStore.almacenActual.AlmacenId;
    formData.value = {
      Nombre: almacenesStore.almacenActual.Nombre || "",
      Ubicacion: almacenesStore.almacenActual.Ubicacion || "",
      Activo: almacenesStore.almacenActual.Activo ?? true,
    };
  }
});

const onSubmit = async () => {
  try {
    const payload = {
      Nombre: formData.value.Nombre,
      Ubicacion: formData.value.Ubicacion || null,
    };

    if (editId.value) {
      // Editar
      payload.AlmacenId = editId.value;
      payload.Activo = formData.value.Activo;
      await almacenesStore.editItem(payload);
    } else {
      // Crear
      await almacenesStore.addItem(payload);
    }

    router.push("/almacenes");
  } catch (error) {
    console.error("Error al guardar almacén:", error);
  }
};

const cancelar = () => {
  router.push("/almacenes");
};
</script>

<template>
  <main class="min-h-screen p-4 sm:p-6">
    
    <!-- HEADER -->
    <header class="max-w-4xl mx-auto mb-6">
      <div class="p-4 sm:p-5 bg-base-100 shadow rounded-xl">
        <h1 class="text-xl sm:text-2xl lg:text-3xl font-medium text-neutral">
          {{ editId ? 'Editar Almacén' : 'Crear Almacén' }}
        </h1>
      </div>
    </header>

    <!-- FORMULARIO -->
    <section class="max-w-4xl mx-auto bg-base-100 p-4 sm:p-6 lg:p-8 rounded-xl shadow">
      
      <form @submit.prevent="onSubmit">
        <div class="grid grid-cols-1 gap-6">
          
          <!-- Nombre -->
          <div class="form-control">
            <label class="label">
              <span class="label-text font-semibold">Nombre del Almacén *</span>
            </label>
            <input 
              v-model="formData.Nombre"
              type="text" 
              placeholder="Ej: Almacén Principal"
              class="input input-bordered w-full"
              required
            />
          </div>

          <!-- Ubicación -->
          <div class="form-control">
            <label class="label">
              <span class="label-text font-semibold">Ubicación</span>
            </label>
            <input 
              v-model="formData.Ubicacion"
              type="text" 
              placeholder="Ej: Calle 123, Ciudad"
              class="input input-bordered w-full"
            />
          </div>

          <!-- Activo (solo en edición) -->
          <div v-if="editId" class="form-control">
            <label class="label cursor-pointer justify-start gap-3">
              <input 
                v-model="formData.Activo"
                type="checkbox" 
                class="checkbox checkbox-primary"
              />
              <span class="label-text font-semibold">Almacén Activo</span>
            </label>
          </div>

        </div>

        <!-- BOTONES -->
        <div class="flex flex-col sm:flex-row justify-end gap-3 mt-6">
          <button 
            type="button" 
            @click="cancelar" 
            class="btn btn-ghost rounded-full px-10 w-full sm:w-auto"
          >
            Cancelar
          </button>
          <button 
            type="submit" 
            class="btn btn-primary rounded-full px-10 w-full sm:w-auto"
          >
            {{ editId ? 'Actualizar' : 'Crear' }}
          </button>
        </div>
      </form>

    </section>

  </main>
</template>
