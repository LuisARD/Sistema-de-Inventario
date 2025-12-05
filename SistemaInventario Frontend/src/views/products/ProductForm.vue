<script setup>
import { onMounted, ref, computed, watch } from "vue";
import { productoSchema } from "../../schema/productoSchema";
import { useCrudForm } from "../../composable/useCrudForm";

import { useCategoríasStore } from "../../store/categorias";
import { useProveedoresStore } from "../../store/proveedores";
import { useProductosStore } from "../../store/producto";
import { useRouter } from "vue-router";

// STORES
const categorias = useCategoríasStore();
const proveedores = useProveedoresStore();
const productoStore = useProductosStore();
const router = useRouter();

// Form reference
const formRef = ref(null);

// ⇢ Objeto base igual que tu otro componente
const producto = {
  nombre: "",
  descripcion: "",
  precio: "",
  stock: "",
  CategoriaId: null,
  ProveedorId: null,
};

onMounted(() => {
  if (productoStore.productoActual) {
    const p = productoStore.productoActual;

    // 1. Llenamos formData
    Object.assign(formData.value, {
      ProductoId: p.ProductoId,
      codigo: p.CodigoSku,
      nombre: p.Nombre,
      descripcion: p.Descripcion,
      categoria: p.CategoriaId,
      proveedor: p.ProveedorId,
      precio_compra: p.PrecioCompra,
      precio_venta: p.PrecioVenta,
      unidad_medida: p.UnidadMedida,
      stock_minimo: p.StockMinimo,
    });

    // 2. Llenamos FormKit UI
    if (formRef.value?.node?.input) {
      formRef.value.node.input({ ...formData.value });
    }

    // 3. Marcamos como edición
    editId.value = p.ProductoId;
  }
});


// Fetch inicial
onMounted(async () => {
  try {
    await categorias.fetchCategorías();
    await proveedores.fetchProveedores();
    await productoStore.fetchProductos();
  } catch (e) {
    console.error("Error cargando:", e);
  }
});

// Opciones para selects dinámicos
const categoriasOptions = computed(
  () =>
    categorias.categorias?.map((cat) => ({
      label: cat.CategoriaNombre ?? cat.Nombre ?? "",
      value: cat.CategoriaId ?? cat.id,
    })) ?? []
);

const proveedoresOptions = computed(
  () =>
    proveedores.proveedores?.map((p) => ({
      label: p.ProveedorNombre ?? p.NombreEmpresa ?? "",
      value: p.ProveedorId ?? p.id,
    })) ?? []
);

// Data que FormKit usará dentro del schema
const formDataForFormKit = computed(() => ({
  categorias: categoriasOptions.value,
  proveedores: proveedoresOptions.value,
}));

watch(formDataForFormKit, (v) => console.log("formDataForFormKit:", v), {
  immediate: true,
});

// ⇢ Usamos la MISMA lógica que en tu ejemplo
const { formData, editId, handleEdit, saveEdit, handleSubmit, handleRemove } =
  useCrudForm(productoStore, producto, formRef);

// Funcion de envío EXACTA al estilo anterior
const onSubmit = (data) => {

  // 🔥 ARMAS EL PAYLOAD EXACTO QUE EL BACKEND QUIERE
  const payload = {
    ProductoId: editId.value,
    CodigoSku: data.codigo,
    Nombre: data.nombre,
    Descripcion: data.descripcion,
    CategoriaId: data.categoria,
    ProveedorId: data.proveedor,
    PrecioCompra: data.precio_compra,
    PrecioVenta: data.precio_venta,
    UnidadMedida: data.unidad_medida,
    StockMinimo: data.stock_minimo,
  };

  // 👇 AQUÍ reemplazas "data" por "payload"
  if (editId.value) {
    saveEdit(payload);
     router.push('/products');
   
  } else {
    handleSubmit(payload);
  }

  formRef.value.node.reset();
  
};


const schema = productoSchema;
</script>

<template>
  <div class="min-h-screen p-5">
    <div class="max-w-4xl mx-auto bg-base-100 p-10 rounded-xl shadow">
      <h1
        class="text-2xl flex justify-center font-medium text-neutral gap-x-2 mb-8"
      >
        <img class="size-10" src="/btn1.svg" alt="" />
        Gestión de Productos
      </h1>

      <FormKit
        v-if="formDataForFormKit.categorias && formDataForFormKit.proveedores"
        ref="formRef"
        type="form"
        @submit="onSubmit"
        :actions="false"
      >
        <FormKitSchema
          :schema="schema"
          :data="formDataForFormKit"
          :classes="{ outer: 'grid grid-cols-1 md:grid-cols-2 gap-6 mb-8' }"
        />

        <FormKit
          type="submit"
          :label="editId ? 'Editar' : 'Guardar'"
          input-class="btn btn-primary rounded-full px-10 mt-5"
        />
      </FormKit>
    </div>
  </div>
</template>
