<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { FormKit, defaultConfig } from "@formkit/vue"; // si usas FormKit global, puedes quitar esta línea
import { FormKitSchema } from "@formkit/vue"; // si tu setup usa el componente
import { useProveedoresStore } from "../../store/proveedores";
import { proveedorSchema } from "../../schema/proveedoresSchema"; // tu schema

const router = useRouter();
const proveedorStore = useProveedoresStore();
const formRef = ref(null);

// FormKit data (items para selects - no hay aquí pero lo dejo por si se necesita)
const formDataForFormKit = computed(() => ({}));

// payload shape local (opcional, para reset)
const proveedorBase = {
  nombre_empresa: "",
  nombre_contacto: "",
  telefono: "",
  email: "",
  direccion: "",
};

// Form reactivo que usaremos para leer/llenar (coincide con los names del schema)
const formValues = ref({ ...proveedorBase });



// Crear proveedor (SIEMPRE crea, no edita)
const crearProveedor = async () => {
  try {
    // Obtén los valores directamente desde FormKit si quieres:
    const data = formRef.value?.node?.value ?? formValues.value;

    // Mapea al payload que el backend espera
    const payload = {
      // No enviamos ProveedorId porque no vamos a editar
      NombreEmpresa: data.nombre_empresa,
      NombreContacto: data.nombre_contacto,
      Telefono: data.telefono,
      Email: data.email,
      Direccion: data.direccion,
    };

    // Llamada al store -> create
    await proveedorStore.addItem(payload);

    // Limpiar proveedorActual en store (si venías desde editar)
    proveedorStore.proveedorActual = null;

    // Reset FormKit UI y formValues
    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...proveedorBase });
    }
    Object.assign(formValues.value, { ...proveedorBase });

    // Redirigir a la lista de proveedores (opcional)
    
  } catch (err) {
    console.error("Error creando proveedor:", err);
    // aquí podrías mostrar un toast o notificación
  }
};
</script>

<template>
  <div class="min-h-screen p-6 flex justify-center">
    <div class="card w-full max-w-3xl bg-base-100 shadow p-8">

      <h1 class="text-2xl font-semibold mb-6 flex items-center gap-3">
        <img src="/btn1.svg" class="w-7 h-7" />
        Crear Proveedor
      </h1>

      <!-- FormKit form usando tu schema -->
      <FormKit
        ref="formRef"
        type="form"
        :actions="false"
        @submit="crearProveedor"
        class="space-y-4"
      >
        <FormKitSchema :schema="proveedorSchema" :data="formDataForFormKit" />

        <div class="flex justify-end pt-4">
          <button
            type="submit"
            class="btn btn-primary rounded-full px-8"
          >
            Crear Proveedor
          </button>
        </div>
      </FormKit>
    </div>
  </div>
</template>
