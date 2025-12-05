import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";

export const useAlmacenesStore = defineStore("almacenes", () => {
  const almacenes = ref([]);
  const almacenActual = ref(null);

  const {
    fetchItems,
    createItemApi,
    updateItemApi,
    deleteItemApi,
    loading,
    error,
  } = useCrudApi({ almacenes });

  const fetchAlmacenes = () => fetchItems("Almacenes", "almacenes");

  const addItem = (almacen) => createItemApi("Almacenes", "almacenes", almacen);

  const editItem = (almacen) =>
    updateItemApi("Almacenes", "almacenes", almacen.AlmacenId, almacen);

  const deleteItem = (id) => deleteItemApi("Almacenes", "almacenes", id);

  return {
    almacenes,
    almacenActual,
    fetchAlmacenes,
    addItem,
    editItem,
    deleteItem,
    loading,
    error,
  };
});
