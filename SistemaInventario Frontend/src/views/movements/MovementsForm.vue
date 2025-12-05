<script setup>
import { ref, onMounted, computed, watch, warn } from "vue";
import { useRouter } from "vue-router";
import { movimientoSchema } from "../../schema/movimientoSchema";
import { useMovimientoStore } from "../../store/movimiento";
import { useUsuarioStore } from "../../store/usuario";
import { useCrudForm } from "../../composable/useCrudForm";

const router = useRouter();
const formRef = ref(null);

const movimientoStore = useMovimientoStore();
const usuarioStore = useUsuarioStore();

// BASE
const movimientoBase = {
  TipoMovimiento: "",
  Motivo: "",
  ReferenciaDocumento: "",
  UsuarioId: null,
  AlmacenOrigenId: "",
  AlmacenDestinoId: "",
};

// fetch usuarios primero
onMounted(async () => {
  await usuarioStore.fetchUsuario();
  usuariosOptions.value; // forzar computado
});

// generar opciones para el select de usuarios
const usuariosOptions = computed(
  () =>
    usuarioStore.usuarios?.map((u) => ({
      label: u.NombreCompleto ?? "",
      value: u.UsuarioId ?? "",
    })) ?? []
);

watch(usuariosOptions, (v) => console.log("usuariosOptions:", v), {
  immediate: true,
});
// lo que el schema usará
// const formDataForFormKit = computed(() => ({
//   usuarios: usuariosOptions.value,
// }));

// useCrudForm
const { formData, editId, handleSubmit, saveEdit } = useCrudForm(
  movimientoStore,
  movimientoBase,
  formRef
);

const formDataForFormKit = computed(() => ({
  usuarios: usuariosOptions.value,
}));


// submit final
const onSubmit = (data) => {
  const payload = {
    TipoMovimiento: data.TipoMovimiento,
    Motivo: data.Motivo,
    ReferenciaDocumento: Number(data.ReferenciaDocumento),
    UsuarioId: Number(data.UsuarioId),
    AlmacenOrigenId: Number(data.AlmacenOrigenId),
    AlmacenDestinoId: Number(data.AlmacenDestinoId),
  };

  if (editId.value) saveEdit({ id: editId.value, ...payload });
  else handleSubmit(payload);

  formRef.value?.node.reset();
  router.push("/movements");
};
</script>

<template>
  <div class="min-h-screen p-6">
    <div class="max-w-3xl mx-auto bg-base-100 p-10 rounded-xl shadow">
      <h1 class="text-2xl font-semibold text-center mb-6">
        {{ editId ? "Editar Movimiento" : "Registrar Movimiento" }}
      </h1>

      <FormKit
  v-if="usuariosOptions.length"
  ref="formRef"
  type="form"
  :actions="false"
  @submit="onSubmit"
>
  <FormKitSchema
    :schema="movimientoSchema"
    :data="formDataForFormKit"
    :classes="{ outer: 'grid grid-cols-1 md:grid-cols-2 gap-6' }"
  />

  <FormKit
    type="submit"
    :label="editId ? 'Actualizar' : 'Guardar'"
    input-class="btn btn-primary rounded-full px-10 mt-8"
  />
</FormKit>

    </div>
  </div>
</template>
