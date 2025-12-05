import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";

export const useAlmacenesStore = defineStore("almacenes", () => {
  const almacenes = ref([]);

  const { fetchItems, loading, error } = useCrudApi({ almacenes });

  const fetchAlmacenes = () => fetchItems("Almacenes", "almacenes");

  return {
    almacenes,
    fetchAlmacenes,
    loading,
    error,
  };
});
