<script setup>
import { ref, computed, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useMovimientoStore } from "../../store/movimiento";
import { useCrudForm } from "../../composable/useCrudForm";

const router = useRouter();
const buscar = ref("");

const movimientoStore = useMovimientoStore();

const movimientoBase = {
  fecha: "",
  tipo: "",
  usuario: "",
  motivo: "",
  referencia: "",
  origen: "",
  destino: ""
};

// Reutilizamos lógica CRUD
const { handleRemove, handleEdit } = useCrudForm(
  movimientoStore,
  movimientoBase,
  null
);

onMounted(() => {
  movimientoStore.fetchMovimiento();
});

console.log(movimientoStore.movimientos)

// Filtro
// const movimientosFiltrados = computed(() => {
//   return movimientoStore.movimientos.filter((m) =>
//     m.motivo.toLowerCase().includes(buscar.value.toLowerCase())
//   );
// });

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

    <input
      v-model="buscar"
      type="text"
      placeholder="Buscar movimiento…"
      class="input input-bordered w-full max-w-xs mb-4"
    />

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
        <tr v-for="m in movimientoStore.movimientos" :key="m.MovimientoId">
          <td>{{ m.FechaMovimiento }}</td>
          <td>{{ m.TipoMovimiento }}</td>
          <td>{{ m.Motivo }}</td>
          <td>{{ m.ReferenciaDocumento }}</td>
          <td>{{ m.UsuarioNombre }}</td>
          <td>{{ m.AlmacenOrigenNombre }}</td>
          <td>{{ m.AlmacenDestinoNombre }}</td>

          <td class="flex gap-2 justify-center">
            <button class="btn btn-sm" @click="editarMovimiento(m)">
              Editar
            </button>

            <button
              class="btn btn-sm btn-error"
              @click="handleRemove(m.MovimientoId)"
            >
              Eliminar
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
