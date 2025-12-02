import { ref } from "vue";

export function useCrudForm(store, initialData, formRef){

    console.log(store, initialData, formRef)


    const editId = ref(null)

    const formData = ref({...initialData})


    const handleEdit = (item) => {
        
        editId.value = item.id;
        formData.value = {...item};

        if(formRef?.value?.node.input){
            formRef.value.node.input({...item})
        }
    }


    const saveEdit = async (data) => {

        await store.editItem(editId.value, data);
        editId.value = null
    }

    const handleSubmit = async (data) => {
        await store.addItem(data);
    }

    const handleRemove = async(id) => {
        await store.deleteItem(id)
    }

    return{
        formData,
        editId,
        handleEdit,
        saveEdit,
        handleSubmit,
        handleRemove
    }

}