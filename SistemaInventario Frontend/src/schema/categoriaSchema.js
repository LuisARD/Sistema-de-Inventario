// schema/categoriaSchema.js
export const categoriaSchema = [
  {
    $formkit: "text",
    name: "nombre",
    label: "Nombre de la categoría",
    validation: "required|length:3",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El nombre es obligatorio",
      length: "Debe tener al menos 3 caracteres",
    },
  },
  {
    $formkit: "text",
    name: "descripcion",
    label: "Descripción",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
  },
];
