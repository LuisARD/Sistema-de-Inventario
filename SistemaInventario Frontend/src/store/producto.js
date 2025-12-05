import { defineStore } from "pinia"
import { useCrudApi } from "../composable/useCrudApi"
import { ref } from "vue"



export const useProductosStore = defineStore("productos", () => {

    const productos = ref([])
        const productoActual = ref(null) // <--- AQUI


    const {fetchItems, createItemApi, updateItemApi, deleteItemApi, loading, error} = useCrudApi({productos})


    const fetchProductos = () => fetchItems("Productos", "productos")
    const addItem = (nuevoProducto) => createItemApi("Productos", "productos", nuevoProducto)
    const editItem = (productoID, datosActualizado) => updateItemApi("Productos", "productos", productoID, datosActualizado)
    const deleteItem = (productoID) => deleteItemApi("Productos", "productos", productoID)



    return{
        productos,
        productoActual,
        fetchProductos,
        addItem,
        editItem,
        deleteItem,
        loading,
        error,
    }
})