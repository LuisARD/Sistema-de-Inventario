<script setup>
import { onMounted, ref, computed } from "vue";
import { useProductosStore } from "../../store/producto";
import { useRouter } from "vue-router";
import { useCrudForm } from "../../composable/useCrudForm";
const productosStore = useProductosStore();
const searchQuery = ref("");
const producto = {
  nombre: "",
  descripcion: "",
  precio: "",
  stock: "",
  CategoriaId: null,
  ProveedorId: null,
};
const formRef = ref(null);
const router = useRouter();
const {handleRemove, handleEdit} = useCrudForm(productosStore, producto, formRef);
// cargar productos al montar
onMounted(() => {
  productosStore.fetchProductos();
});



// Filtrado simple: busca en todos los campos convertidos a texto
const filteredProducts = computed(() => {
  const q = searchQuery.value?.trim().toLowerCase();
  if (!q) return productosStore.productos;
  return productosStore.productos.filter((p) =>
    Object.values(p).some((v) =>
      String(v ?? "").toLowerCase().includes(q)
    )
  );
});

const btnCrearProducto = () => {  
  router.push('/products/create');
}

const editarProducto = (product) => {
  productosStore.productoActual = product;
  router.push('/products/create');
}

// Acciones (implementar navegación/modales según tu app)


</script>


<template>
  <div class="min-h-screen bg-white p-5">

    <!-- HEADER -->
    <div class="flex items-center justify-between mb-8 p-5 bg-base-100 shadow rounded-xl">
      <div class="flex items-center gap-4">
        <div class="w-12 h-12 bg-primary text-white rounded-xl flex items-center justify-center">
          <!-- ICON -->
          <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="none"
               viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/>
            <polyline points="9 22 9 12 15 12 15 22"/>
          </svg>
        </div>

        <h1 class="text-3xl font-medium text-neutral">Gestión de Productos</h1>
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
            v-model="searchQuery"
            placeholder="🔍 Buscar"
            class="input input-bordered w-full"
          >
        </div>

        <!-- BUTTON -->
        <button 
          class="btn btn-primary rounded-full px-8"
          @click="btnCrearProducto"
        >
          Crear Productos
        </button>
      </div>

      <!-- TABLE -->
      <div class="overflow-x-auto rounded-lg border border-base-300">
        <table class="table table-zebra w-full">

          <thead class="bg-base-200">
            <tr>
            <th>Código SKU</th>
              <th>Nombre</th>
              <th>Descripción</th>
              <th>Categoría</th>
              <th>Proveedor</th>
              <th>Precio compra</th>
              <th>Precio venta</th>
              <th>Unidad</th>
              <th>Stock mínimo</th>
              <th>Acción</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="product in filteredProducts" :key="product.codigo">
                <td>{{ product.CodigoSku }}</td>
              <td>{{ product.Nombre }}</td>
              <td>{{ product.Descripcion ?? "-" }}</td>
              <td>{{ product.CategoriaNombre ?? "-" }}</td>
              <td>{{ product.ProveedorNombre ?? "-" }}</td>
              <td>{{ product.PrecioCompra }}</td>
              <td>{{ product.PrecioVenta }}</td>
              <td>{{ product.UnidadMedida }}</td>
              <td>{{ product.StockMinimo }}</td>

              <!-- ACTIONS -->
              <td class="flex gap-2">

                <!-- EDIT BUTTON -->
                <button 
                  class="btn btn-sm btn-ghost"
                  @click="editarProducto(product)"
                  title="Editar"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                       fill="none" viewBox="0 0 24 24" stroke="currentColor"
                       stroke-width="2">
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/>
                  </svg>
                </button>

                <!-- DELETE BUTTON -->
                <button 
                  class="btn btn-sm btn-error text-white"
                  @click="handleRemove(product.ProductoId)"
                  title="Eliminar"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5"
                       fill="none" viewBox="0 0 24 24" stroke="currentColor"
                       stroke-width="2">
                    <polyline points="3 6 5 6 21 6"/>
                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
                  </svg>
                </button>

              </td>
            </tr>
              <tr v-if="(filteredProducts.length === 0)">
              <td colspan="10" class="text-center py-4">No hay productos</td>
            </tr>
          </tbody>
        </table>
      </div>

     

    </div>
  </div>
</template>


