import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";
import API from "../services/axios";

export const useExistenciasStore = defineStore("existencias", () => {
  const existencias = ref([]);
  const stockBajo = ref([]);
  const existenciasPorProducto = ref([]);
  const existenciasPorAlmacen = ref([]);

  const {
    fetchItems,
    createItemApi,
    updateItemApi,
    deleteItemApi,
    loading,
    error,
  } = useCrudApi({ existencias });

  const fetchExistencias = () => fetchItems("Existencias", "existencias");
  
  const fetchStockBajo = async () => {
    try {
      const response = await API.get("/Existencias/stock-bajo");
      stockBajo.value = response.data;
    } catch (err) {
      console.error("Error al obtener stock bajo:", err);
    }
  };

  const fetchExistenciasPorProducto = async (productoId) => {
    try {
      const response = await API.get(`/Existencias/producto/${productoId}`);
      existenciasPorProducto.value = response.data;
    } catch (err) {
      console.error("Error al obtener existencias por producto:", err);
    }
  };

  const fetchExistenciasPorAlmacen = async (almacenId) => {
    try {
      const response = await API.get(`/Existencias/almacen/${almacenId}`);
      existenciasPorAlmacen.value = response.data;
    } catch (err) {
      console.error("Error al obtener existencias por almacén:", err);
    }
  };

  return {
    existencias,
    stockBajo,
    existenciasPorProducto,
    existenciasPorAlmacen,
    fetchExistencias,
    fetchStockBajo,
    fetchExistenciasPorProducto,
    fetchExistenciasPorAlmacen,
    loading,
    error,
  };
});