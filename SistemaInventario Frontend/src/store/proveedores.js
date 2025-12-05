import { defineStore } from "pinia";
import { ref } from "vue";
import { useCrudApi } from "../composable/useCrudApi";

export const useProveedoresStore = defineStore("proveedores", () => {

const proveedores = ref([])


    const {fetchItems, createItemApi, editItemAPi, deleteItemApi, loading, error} = useCrudApi({proveedores})

    const fetchProveedores = () => fetchItems("Proveedores", "proveedores")

    const addItem = (nuevoProveedores)  => createItemApi("Proveedores", "proveedores", nuevoProveedores)

    const editItem = (proveedoresID, datosActualizados)  => editItemAPi("Proveedores", "proveedores", proveedoresID, datosActualizados)

    const deleteItem = (proveedoresID) => deleteItemApi("Proveedores", "proveedores", proveedoresID)


    return {proveedores, fetchProveedores, addItem, editItem, deleteItem, loading, error}
})