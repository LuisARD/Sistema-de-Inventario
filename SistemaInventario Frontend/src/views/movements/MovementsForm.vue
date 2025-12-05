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
    // Construir payload base
    const payload = {
      TipoMovimiento: formData.value.TipoMovimiento,
      Motivo: formData.value.Motivo,
      ReferenciaDocumento: formData.value.ReferenciaDocumento,
      UsuarioId: authStore.user.UsuarioId,
      Detalles: detalles.value.map((d) => ({
        ProductoId: d.ProductoId,
        Cantidad: d.Cantidad,
        CostoUnitarioHistorico: d.CostoUnitarioHistorico,
      })),
    };

    // Agregar almacenes según el tipo de movimiento
    if (formData.value.TipoMovimiento === "ENTRADA") {
      // Para ENTRADA: solo AlmacenDestinoId es requerido
      payload.AlmacenDestinoId = formData.value.AlmacenDestinoId;
      if (formData.value.AlmacenOrigenId) {
        payload.AlmacenOrigenId = formData.value.AlmacenOrigenId;
      }
    } else {
      // Para SALIDA: solo AlmacenOrigenId es requerido
      payload.AlmacenOrigenId = formData.value.AlmacenOrigenId;
      if (formData.value.AlmacenDestinoId) {
        payload.AlmacenDestinoId = formData.value.AlmacenDestinoId;
      }
    }

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
  <main class="min-h-screen p-4 sm:p-6">
    <article class="max-w-6xl mx-auto bg-base-100 p-4 sm:p-6 lg:p-8 rounded-xl shadow">

      <header class="mb-6 flex flex-col sm:flex-row sm:items-center gap-3">
        <img src="/iconCamion.svg" class="w-7 h-7 sm:w-8 sm:h-8" />
        <h1 class="text-xl sm:text-2xl font-semibold">
          Registrar Movimiento de Inventario
        </h1>
      </header>

      <form @submit.prevent="onSubmit" class="space-y-6">

        <!-- INFORMACIÓN GENERAL -->
        <section class="bg-base-200 p-4 rounded-lg">
          <h2 class="text-base sm:text-lg font-semibold mb-4">Información General</h2>

          <div class="grid grid-cols-1 lg:grid-cols-3 gap-4">
            <div class="form-control">
              <label class="label">Tipo de Movimiento *</label>
              <select v-model="formData.TipoMovimiento" class="select select-bordered w-full" required>
                <option value="ENTRADA">Entrada</option>
                <option value="SALIDA">Salida</option>
              </select>
            </div>

            <div class="form-control">
              <label class="label">Referencia Documento *</label>
              <input
                v-model="formData.ReferenciaDocumento"
                type="text"
                class="input input-bordered w-full"
                placeholder="FAC-001"
                required
              />
            </div>

            <div class="form-control">
              <label class="label">Usuario Responsable</label>
              <input
                :value="authStore.user?.NombreCompleto"
                class="input input-bordered w-full"
                readonly
                disabled
              />
            </div>
          </div>

          <div class="form-control mt-4">
            <label class="label">Motivo *</label>
            <textarea
              v-model="formData.Motivo"
              class="textarea textarea-bordered w-full"
              rows="2"
              required
            ></textarea>
          </div>
        </section>

        <!-- ALMACENES -->
        <section class="bg-base-200 p-4 rounded-lg">
          <h2 class="text-base sm:text-lg font-semibold mb-4">Almacenes</h2>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div class="form-control">
              <label class="label">Almacén Origen *</label>
              <select v-model="formData.AlmacenOrigenId" class="select select-bordered w-full" required>
                <option :value="null" disabled>Seleccione</option>
                <option v-for="a in almacenesOptions" :key="a.value" :value="a.value">
                  {{ a.label }}
                </option>
              </select>
            </div>

            <div class="form-control">
              <label class="label">Almacén Destino *</label>
              <select v-model="formData.AlmacenDestinoId" class="select select-bordered w-full" required>
                <option :value="null" disabled>Seleccione</option>
                <option v-for="a in almacenesOptions" :key="a.value" :value="a.value">
                  {{ a.label }}
                </option>
              </select>
            </div>
          </div>
        </section>

        <!-- PRODUCTOS -->
        <section class="bg-base-200 p-4 rounded-lg">
          <div class="flex flex-col sm:flex-row sm:justify-between gap-3 mb-4">
            <h2 class="text-base sm:text-lg font-semibold">Productos</h2>
            <button type="button" @click="agregarProducto" class="btn btn-sm btn-info w-full sm:w-auto">
              + Agregar Producto
            </button>
          </div>

          <div class="overflow-x-auto">
            <table class="table w-full min-w-[750px]">
              <thead>
                <tr>
                  <th>Producto</th>
                  <th>Cantidad</th>
                  <th>Costo Unitario</th>
                  <th>Total</th>
                  <th></th>
                </tr>
              </thead>

              <tbody>
                <tr v-for="(detalle, index) in detalles" :key="index">
                  <td>
                    <select
                      v-model="detalle.ProductoId"
                      @change="onProductoChange(index)"
                      class="select select-bordered select-sm w-full"
                      required
                    >
                      <option :value="null" disabled>Seleccione</option>
                      <option v-for="p in productosOptions" :key="p.value" :value="p.value">
                        {{ p.label }}
                      </option>
                    </select>
                  </td>

                  <td>
                    <input
                      v-model.number="detalle.Cantidad"
                      type="number"
                      min="1"
                      class="input input-bordered input-sm w-full"
                      required
                    />
                  </td>

                  <td>
                    <input
                      v-model.number="detalle.CostoUnitarioHistorico"
                      type="number"
                      class="input input-bordered input-sm w-full bg-base-300"
                      readonly
                    />
                  </td>

                  <td class="font-semibold">
                    ${{ calcularCostoTotal(detalle).toFixed(2) }}
                  </td>

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
                  <td colspan="3" class="text-right">TOTAL:</td>
                  <td class="text-primary text-lg">${{ totalGeneral.toFixed(2) }}</td>
                  <td></td>
                </tr>
              </tfoot>
            </table>
          </div>
        </section>

        <!-- ACCIONES -->
        <footer class="flex flex-col sm:flex-row justify-end gap-3 pt-4">
          <button type="button" @click="cancelar" class="btn btn-ghost w-full sm:w-auto">
            Cancelar
          </button>

          <button type="submit" class="btn btn-info w-full sm:w-auto">
            Registrar Movimiento
          </button>
        </footer>

      </form>
    </article>
  </main>
</template>
