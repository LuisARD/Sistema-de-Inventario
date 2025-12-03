import { defineStore } from "pinia";
import { useCrudApi } from "../composable/useCrudApi";
import { ref } from "vue";

export const useMovimientoStore = defineStore("movimientos", () => {

    const movimientos = ref([])


    const {fetchItems, createItemApi, loading, error} = useCrudApi({movimientos})


    const fetchMovimientos = () => fetchItems("Movimientos", "movimientos")

    const addItem = (nuevoMovimiento) => createItemApi("Movimientos", "movimientos", nuevoMovimiento)


    return {
        fetchMovimientos, addItem, loading,error
    }
})