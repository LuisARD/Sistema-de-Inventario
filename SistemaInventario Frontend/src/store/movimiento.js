// store/movimientos.js
import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";

export const useMovimientoStore = defineStore("movimientos", () => {
  const movimientos = ref([]);
  const movimientoActual = ref(null);

  const {
    fetchItems,
    createItemApi,
    updateItemApi,
    deleteItemApi,
    loading,
    error,
  } = useCrudApi({ movimientos });

  // 👇 Cambia SOLO el endpoint y el nombre del array
  const fetchMovimiento = () => fetchItems("Movimientos", "movimientos");
  const addItem = (nuevo) =>
    createItemApi("Movimientos", "movimientos", nuevo);
  const editItem = (id, datos) =>
    updateItemApi("Movimientos", "movimientos", id, datos);
  const deleteItem = (id) =>
    deleteItemApi("Movimientos", "movimientos", id);

  return {
    movimientos,
    movimientoActual,
    fetchMovimiento,
    addItem,
    editItem,
    deleteItem,
    loading,
    error,
  };
});
