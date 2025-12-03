import { defineStore } from "pinia";
import { ref } from "vue";
import useCrudApi from "../composable/useCrudApi";

export const useCategoríasStore = defineStore("categorías", () => {
  const categorías = ref([]);

  const { fetchItems, createItemApi, loading, error } = useCrudApi({
    categorías,
  });

  const fetchCategorías = () => fetchItems("Categorías", "categorías");
  const addItem = (nuevaCategoría) =>
    createItemApi("categorías", "categorías", nuevaCategoría);

  return {
    fetchCategorías,
    addItem,
    loading,
    error,
  };
});
