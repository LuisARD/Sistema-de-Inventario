import { defineStore } from "pinia";
import { ref } from "vue";
import {useCrudApi} from "../composable/useCrudApi";

export const useCategoríasStore = defineStore("Categorias", () => {
  const categorias = ref([]);

  const { fetchItems, createItemApi, loading, error } = useCrudApi({
    categorias,
  }); 

  const fetchCategorías = () => fetchItems("Categorias", "categorias");
  const addItem = (nuevaCategoría) =>
    createItemApi("Categorias", "categorias", nuevaCategoría);

  return {
    categorias,
    fetchCategorías,
    addItem,
    loading,
    error,
  };
});
