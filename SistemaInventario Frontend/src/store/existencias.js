import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";

export const useExistenciasStore = defineStore("existencias", () => {
  const existencias = ref([]);


  const {
    fetchItems,
    createItemApi,
    updateItemApi,
    deleteItemApi,
    loading,
    error,
  } = useCrudApi({ existencias });

  const fetchExistencias = () => fetchItems("Existencias", "existencias");
 

  return {
    existencias,
    fetchExistencias,
    loading,
    error,
  };
});