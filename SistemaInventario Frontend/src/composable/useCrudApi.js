import API from "../services/axios";
import { useCrudStore } from "./UseCrudStore";
import { ref } from "vue";

export function useCrudApi(store) {
  const { addItem, editItem, deleteItem } = useCrudStore(store);


  const loading = ref(false);
  const error = ref(null);

  const fetchItems = async (resource, key) => {
    loading.value = true;
    error.value = null;

    try {
      const { data } = await API.get(`/${resource}`);
      store[key].value.splice(0, store[key].value.length, ...data);
      return data;
    } catch (err) {
      console.log(`Error Al cargar ${resource}`, err);
      error.value = err;
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const createItemApi = async (resource, key, nuevo, tipo) => {
    error.value = null;

    



    try {
      const { data } = await API.post(`/${resource}`, nuevo);
      addItem(data, key);
      return data;
    } catch (err) {
      console.error(`Error al crear ${resource}`, err);
      error.value = err;
      throw err;
    }
  };

  const updateItemApi = async (resource, key, id, datosActualizados) => {
    error.value = null;

    try {
      await API.put(`/${resource}/${id}`, datosActualizados);
      editItem(id, datosActualizados, key);
    } catch (err) {
      console.error(`Error al actualizar ${resource}`, err);
      error.value = err;
      throw err;
    }
  };

  const deleteItemApi = async (resource, key, id) => {
    // Elimina un elemento de la API y del store
    error.value = null;
    try {
      await API.delete(`/${resource}/${id}`);
      deleteItem(id, key);
    } catch (err) {
      console.error(`Error al eliminar ${resource}`, err);
      error.value = err;
      throw err;
    }
  };

  return {
    loading,
    error,
    fetchItems,
    createItemApi,
    updateItemApi,
    deleteItemApi,
  };
}
