<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { useMovimientoStore } from "../../store/movimiento";
import { useProductosStore } from "../../store/producto";
import { useAlmacenesStore } from "../../store/almacenes";
import { useAuthStore } from "../../store/auth";
import API from "../../services/axios";

const router = useRouter();
const movimientoStore = useMovimientoStore();
const productosStore = useProductosStore();
const almacenesStore = useAlmacenesStore();
const authStore = useAuthStore();

// Datos del formulario
const formData = ref({
  TipoMovimiento: "ENTRADA",
  Motivo: "",
  ReferenciaDocumento: "",
  AlmacenOrigenId: null,
  AlmacenDestinoId: null,
});

// Detalles de productos (tabla dinámica)
const detalles = ref([
  {
    ProductoId: null,
    Cantidad: 1,
    CostoUnitarioHistorico: 0,
  }
]);

// Cargar datos al montar
onMounted(async () => {
  await productosStore.fetchProductos();
  await almacenesStore.fetchAlmacenes();
});

// Opciones para selects
const productosOptions = computed(() =>
  productosStore.productos.map((p) => ({
    value: p.ProductoId,
    label: `${p.Nombre} (${p.CodigoSku})`,
    precioCompra: p.PrecioCompra,
  }))
);

const almacenesOptions = computed(() =>
  almacenesStore.almacenes.map((a) => ({
    value: a.AlmacenId,
    label: a.Nombre,
  }))
);

// Agregar nueva fila de producto
const agregarProducto = () => {
  detalles.value.push({
    ProductoId: null,
    Cantidad: 1,
    CostoUnitarioHistorico: 0,
  });
};

// Eliminar fila de producto
const eliminarProducto = (index) => {
  if (detalles.value.length > 1) {
    detalles.value.splice(index, 1);
  }
};

// Cuando se selecciona un producto, autocompletar el costo
const onProductoChange = (index) => {
  const detalle = detalles.value[index];
  const producto = productosOptions.value.find(
    (p) => p.value === detalle.ProductoId
  );
  if (producto) {
    detalle.CostoUnitarioHistorico = producto.precioCompra || 0;
  }
};

// Calcular costo total por producto
const calcularCostoTotal = (detalle) => {
  return (detalle.Cantidad || 0) * (detalle.CostoUnitarioHistorico || 0);
};

// Calcular total general
const totalGeneral = computed(() => {
  return detalles.value.reduce((sum, d) => sum + calcularCostoTotal(d), 0);
});

// Enviar formulario
const onSubmit = async () => {
  try {
    const payload = {
      TipoMovimiento: formData.value.TipoMovimiento,
      Motivo: formData.value.Motivo,
      ReferenciaDocumento: formData.value.ReferenciaDocumento,
      UsuarioId: authStore.user.UsuarioId,
      AlmacenOrigenId: formData.value.AlmacenOrigenId,
      AlmacenDestinoId: formData.value.AlmacenDestinoId,
      Detalles: detalles.value.map((d) => ({
        ProductoId: d.ProductoId,
        Cantidad: d.Cantidad,
        CostoUnitarioHistorico: d.CostoUnitarioHistorico,
      })),
    };

    // Determinar endpoint según tipo de movimiento
    const endpoint = formData.value.TipoMovimiento === "ENTRADA" 
      ? "/Movimientos/entrada" 
      : "/Movimientos/salida";

    console.log('Endpoint:', endpoint);
    console.log('Payload a enviar:', payload);

    await API.post(endpoint, payload);
    
    router.push("/movements");
  } catch (error) {
    console.error("Error al crear movimiento:", error);
    console.error("Detalles del error:", error.response?.data);
  }
};

const cancelar = () => {
  router.push("/movements");
};
</script>

<template>
  <div class="min-h-screen p-6 bg-white">
    <div class="max-w-6xl mx-auto bg-base-100 p-8 rounded-xl shadow">
      <h1 class="text-2xl font-semibold mb-6 flex items-center gap-3">
        <img src="/camion.svg" class="w-8 h-8" />
        Registrar Movimiento de Inventario
      </h1>

      <form @submit.prevent="onSubmit" class="space-y-6">
        
        <!-- SECCIÓN 1: INFORMACIÓN GENERAL -->
        <div class="bg-base-200 p-4 rounded-lg">
          <h2 class="text-lg font-semibold mb-4">Información General</h2>
          <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
            
            <!-- Tipo de Movimiento -->
            <div class="form-control">
              <label class="label">
                <span class="label-text">Tipo de Movimiento *</span>
              </label>
              <select 
                v-model="formData.TipoMovimiento" 
                class="select select-bordered w-full"
                required
              >
                <option value="ENTRADA">Entrada</option>
                <option value="SALIDA">Salida</option>
              </select>
            </div>

            <!-- Referencia Documento -->
            <div class="form-control">
              <label class="label">
                <span class="label-text">Referencia Documento *</span>
              </label>
              <input 
                v-model="formData.ReferenciaDocumento" 
                type="text" 
                class="input input-bordered w-full"
                placeholder="FAC-001, OC-123, etc."
                required
              />
            </div>

            <!-- Usuario (mostrar readonly) -->
            <div class="form-control">
              <label class="label">
                <span class="label-text">Usuario Responsable</span>
              </label>
              <input 
                :value="authStore.user?.NombreCompleto" 
                type="text" 
                class="input input-bordered w-full"
                readonly
                disabled
              />
            </div>
          </div>

          <!-- Motivo (ancho completo) -->
          <div class="form-control mt-4">
            <label class="label">
              <span class="label-text">Motivo *</span>
            </label>
            <textarea 
              v-model="formData.Motivo" 
              class="textarea textarea-bordered w-full"
              placeholder="Describa el motivo del movimiento..."
              rows="2"
              required
            ></textarea>
          </div>
        </div>

        <!-- SECCIÓN 2: ALMACENES -->
        <div class="bg-base-200 p-4 rounded-lg">
          <h2 class="text-lg font-semibold mb-4">Almacenes</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            
            <!-- Almacén Origen -->
            <div class="form-control">
              <label class="label">
                <span class="label-text">Almacén Origen *</span>
              </label>
              <select 
                v-model="formData.AlmacenOrigenId" 
                class="select select-bordered w-full"
                required
              >
                <option :value="null" disabled>Seleccione un almacén</option>
                <option 
                  v-for="almacen in almacenesOptions" 
                  :key="almacen.value" 
                  :value="almacen.value"
                >
                  {{ almacen.label }}
                </option>
              </select>
            </div>

            <!-- Almacén Destino -->
            <div class="form-control">
              <label class="label">
                <span class="label-text">Almacén Destino *</span>
              </label>
              <select 
                v-model="formData.AlmacenDestinoId" 
                class="select select-bordered w-full"
                required
              >
                <option :value="null" disabled>Seleccione un almacén</option>
                <option 
                  v-for="almacen in almacenesOptions" 
                  :key="almacen.value" 
                  :value="almacen.value"
                >
                  {{ almacen.label }}
                </option>
              </select>
            </div>
          </div>
        </div>

        <!-- SECCIÓN 3: DETALLES DE PRODUCTOS -->
        <div class="bg-base-200 p-4 rounded-lg">
          <div class="flex justify-between items-center mb-4">
            <h2 class="text-lg font-semibold">Productos</h2>
            <button 
              type="button" 
              @click="agregarProducto" 
              class="btn btn-sm btn-primary"
            >
              + Agregar Producto
            </button>
          </div>

          <div class="overflow-x-auto">
            <table class="table w-full">
              <thead>
                <tr>
                  <th class="w-2/5">Producto</th>
                  <th>Cantidad</th>
                  <th>Costo Unitario</th>
                  <th>Costo Total</th>
                  <th>Acción</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(detalle, index) in detalles" :key="index">
                  <!-- Producto -->
                  <td>
                    <select 
                      v-model="detalle.ProductoId" 
                      @change="onProductoChange(index)"
                      class="select select-bordered select-sm w-full"
                      required
                    >
                      <option :value="null" disabled>Seleccione un producto</option>
                      <option 
                        v-for="producto in productosOptions" 
                        :key="producto.value" 
                        :value="producto.value"
                      >
                        {{ producto.label }}
                      </option>
                    </select>
                  </td>

                  <!-- Cantidad -->
                  <td>
                    <input 
                      v-model.number="detalle.Cantidad" 
                      type="number" 
                      min="1"
                      class="input input-bordered input-sm w-full"
                      required
                    />
                  </td>

                  <!-- Costo Unitario (readonly) -->
                  <td>
                    <input 
                      v-model.number="detalle.CostoUnitarioHistorico" 
                      type="number" 
                      step="0.01"
                      class="input input-bordered input-sm w-full bg-base-300"
                      readonly
                    />
                  </td>

                  <!-- Costo Total (calculado) -->
                  <td class="font-semibold">
                    ${{ calcularCostoTotal(detalle).toFixed(2) }}
                  </td>

                  <!-- Acción -->
                  <td>
                    <button 
                      type="button" 
                      @click="eliminarProducto(index)"
                      class="btn btn-sm btn-error btn-circle"
                      :disabled="detalles.length === 1"
                    >
                      ×
                    </button>
                  </td>
                </tr>
              </tbody>
              <tfoot>
                <tr class="font-bold">
                  <td colspan="3" class="text-right">TOTAL GENERAL:</td>
                  <td class="text-lg text-primary">${{ totalGeneral.toFixed(2) }}</td>
                  <td></td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>

        <!-- BOTONES -->
        <div class="flex justify-end gap-3 pt-4">
          <button 
            type="button" 
            @click="cancelar" 
            class="btn btn-ghost rounded-full px-10"
          >
            Cancelar
          </button>
          <button 
            type="submit" 
            class="btn btn-primary rounded-full px-10"
          >
            Registrar Movimiento
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
