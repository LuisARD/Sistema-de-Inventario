import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";

export const useUsuarioStore = defineStore("usuarios", () => {
  const usuarios = ref([]);
  const usuarioActual = ref(null);

  const {
    fetchItems,
    createItemApi,
    updateItemApi,
    deleteItemApi,
    loading,
    error,
  } = useCrudApi({ usuarios });

  const fetchUsuario = () => fetchItems("Usuarios", "usuarios");
  const addItem = (nuevoUsuario) => createItemApi("Usuarios", "usuarios", nuevoUsuario);
  const editItem = (usuarioID, datosActualizado) =>
    updateItemApi("Usuarios", "usuarios", usuarioID, datosActualizado);
  const deleteItem = (usuarioID) => deleteItemApi("Usuarios", "usuarios", usuarioID);

  return {
    usuarios,
    usuarioActual,
    fetchUsuario,
    addItem,
    editItem,
    deleteItem,
    loading,
    error,
  };
});
