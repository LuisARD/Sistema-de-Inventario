export function useCrudStore(){

    const addItem = (item, key) => {
        store[key].value.push({...item})
    }

    const deleteItem = (id, key) => {
        const index = store[key].value.findIndex((item) => item.id === id);

        if(index !== -1){
            store[key].value.slice(index, 1)
        }
    };


    const editItem = (id, data, key) => {
        const index = store[key].value.findIndex((item) => item.id === id);


        if(index !== -1){
            store[key].value[index] = {
                ...store[key].value[index],
                ...data,
            }
        }
    }

    return {addItem, deleteItem, editItem}

}