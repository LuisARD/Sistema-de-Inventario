export const proveedorSchema = [
  {
    $formkit: "text",
    name: "nombre_empresa",
    label: "Nombre de la empresa",
    validation: "required|length:3",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El nombre de la empresa es obligatorio",
      length: "Debe tener al menos 3 caracteres",
    },
  },
  {
    $formkit: "text",
    name: "nombre_contacto",
    label: "Nombre del contacto",
    validation: "required|length:3",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El nombre del contacto es obligatorio",
      length: "Debe tener al menos 3 caracteres",
    },
  },
  {
    $formkit: "text",
    name: "telefono",
    label: "Teléfono",
    validation: "required|length:10",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    placeholder: "Ej: 8290000000",
    validationMessages: {
      required: "El teléfono es obligatorio",
      length: "Debe tener al menos 10 dígitos",
    },
  },
  {
    $formkit: "email",
    name: "email",
    label: "Correo electrónico",
    validation: "required|email",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El correo es obligatorio",
      email: "Debe ser un correo válido",
    },
  },
  {
    $formkit: "textarea",
    name: "direccion",
    label: "Dirección",
    validation: "required|length:5",
    inputClass: "textarea textarea-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "La dirección es obligatoria",
      length: "Debe tener al menos 5 caracteres",
    },
  },
];
