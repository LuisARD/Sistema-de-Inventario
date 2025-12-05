import { defineStore } from "pinia";
import { ref } from "vue";
import {useCrudApi} from "../composable/useCrudApi";

export const useCategoríasStore = defineStore("Categorias", () => {
  const categorias = ref([]);
  const categoriaActual = ref(null);

  const { fetchItems, createItemApi, updateItemApi, deleteItemApi, loading, error } = useCrudApi({
    categorias,
  }); 

  const fetchCategorías = () => fetchItems("Categorias", "categorias");
  const addItem = (nuevaCategoría) =>
    createItemApi("Categorias", "categorias", nuevaCategoría);
  const editItem = (id, datosActualizados) =>
    updateItemApi("Categorias", "categorias", id, datosActualizados);
  const deleteItem = (id) =>
    deleteItemApi("Categorias", "categorias", id);

  return {
    categorias,
    categoriaActual,
    fetchCategorías,
    addItem,
    editItem,
    deleteItem,
    loading,
    error,
  };
});
