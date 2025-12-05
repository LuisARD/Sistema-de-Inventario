<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useMovimientoStore } from "../../store/movimiento";
import { useCrudForm } from "../../composable/useCrudForm";

const router = useRouter();

const buscar = ref("");
const filtroTipo = ref("TODOS");

const modalVisible = ref(false);
const movimientoSeleccionado = ref(null);

const verDetalles = (m) => {
  movimientoSeleccionado.value = m;
  modalVisible.value = true;
};

// STORE
const movimientoStore = useMovimientoStore();

const movimientoBase = {
  fecha: "",
  tipo: "",
  usuario: "",
  motivo: "",
  referencia: "",
  origen: "",
  destino: "",
};

const { handleRemove, handleEdit } = useCrudForm(
  movimientoStore,
  movimientoBase,
  null
);

onMounted(() => {
  movimientoStore.fetchMovimiento();
});

// ========== FILTROS ==========
const movimientosFiltrados = computed(() => {
  return movimientoStore.movimientos.filter((m) => {
    const text = buscar.value.toLowerCase();

    const coincideBusqueda =
      m.Motivo.toLowerCase().includes(text) ||
      m.UsuarioNombre.toLowerCase().includes(text) ||
      m.ReferenciaDocumento.toLowerCase().includes(text);

    const coincideTipo =
      filtroTipo.value === "TODOS" ||
      m.TipoMovimiento === filtroTipo.value;

    return coincideBusqueda && coincideTipo;
  });
});

// ========== ACCIONES ==========
const crearMovimiento = () => {
  movimientoStore.movimientoActual = null;
  router.push("/movements/create");
};

const editarMovimiento = (m) => {
  movimientoStore.movimientoActual = m;
  router.push("/movements/create");
};
</script>

<template>
  <div class="p-6">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold">Movimientos de Inventario</h1>

      <button class="btn btn-primary" @click="crearMovimiento">
        Crear movimiento
      </button>
    </div>

    <!-- FILTROS -->
    <div class="flex gap-4 mb-4">
      <input
        v-model="buscar"
        type="text"
        placeholder="Buscar movimiento…"
        class="input input-bordered w-full max-w-xs"
      />

      <select v-model="filtroTipo" class="select select-bordered">
        <option value="TODOS">Todos</option>
        <option value="ENTRADA">Entrada</option>
        <option value="SALIDA">Salida</option>
      </select>
    </div>

    <!-- TABLA -->
    <table class="table w-full">
      <thead class="bg-base-200">
        <tr>
          <th>Fecha</th>
          <th>Tipo</th>
          <th>Motivo</th>
          <th>Referencia</th>
          <th>Usuario</th>
          <th>Origen</th>
          <th>Destino</th>
          <th class="text-center">Acciones</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="m in movimientosFiltrados" :key="m.MovimientoId">
          <td>{{ m.FechaMovimiento }}</td>
          <td>
            <span
              :class="{
                'badge badge-success': m.TipoMovimiento === 'ENTRADA',
                'badge badge-error': m.TipoMovimiento === 'SALIDA'
              }"
            >
              {{ m.TipoMovimiento }}
            </span>
          </td>

          <td>{{ m.Motivo }}</td>
          <td>{{ m.ReferenciaDocumento }}</td>
          <td>{{ m.UsuarioNombre }}</td>
          <td>{{ m.AlmacenOrigenNombre }}</td>
          <td>{{ m.AlmacenDestinoNombre }}</td>

          <td class="flex gap-2 justify-center">
            <button class="btn btn-sm btn-info" @click="verDetalles(m)">
              Ver detalles
            </button>

          
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <!-- =================== MODAL =================== -->
  <dialog class="modal" :open="modalVisible">
    <div class="modal-box w-11/12 max-w-3xl">
      <h3 class="font-bold text-2xl mb-4">Detalles del Movimiento</h3>

      <div v-if="movimientoSeleccionado">
        <p><strong>ID:</strong> {{ movimientoSeleccionado.MovimientoId }}</p>
        <p><strong>Fecha:</strong> {{ movimientoSeleccionado.FechaMovimiento }}</p>
        <p><strong>Tipo:</strong> {{ movimientoSeleccionado.TipoMovimiento }}</p>
        <p><strong>Motivo:</strong> {{ movimientoSeleccionado.Motivo }}</p>
        <p><strong>Referencia:</strong> {{ movimientoSeleccionado.ReferenciaDocumento }}</p>
        <p><strong>Usuario:</strong> {{ movimientoSeleccionado.UsuarioNombre }}</p>
        <p><strong>Origen:</strong> {{ movimientoSeleccionado.AlmacenOrigenNombre }}</p>
        <p><strong>Destino:</strong> {{ movimientoSeleccionado.AlmacenDestinoNombre }}</p>

        <h3 class="font-bold text-xl mt-4 mb-2">Productos del movimiento</h3>

        <table class="table table-zebra w-full">
          <thead>
            <tr>
              <th>Producto</th>
              <th>SKU</th>
              <th>Cantidad</th>
              <th>Costo Unitario</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="d in movimientoSeleccionado.Detalles"
              :key="d.DetalleId"
            >
              <td>{{ d.ProductoNombre }}</td>
              <td>{{ d.CodigoSku }}</td>
              <td>{{ d.Cantidad }}</td>
              <td>{{ d.CostoUnitarioHistorico }}</td>
              <td>{{ d.CostoTotal }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="modal-action">
        <button class="btn" @click="modalVisible = false">Cerrar</button>
      </div>
    </div>
  </dialog>
</template>
