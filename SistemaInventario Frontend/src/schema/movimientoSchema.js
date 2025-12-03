export const movimientoSchema = [
  {
    $formkit: "date",
    name: "fecha",
    label: "Fecha del movimiento",
    validation: "required",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "La fecha es obligatoria",
    },
  },
  {
    $formkit: "select",
    name: "tipo",
    label: "Tipo de movimiento",
    options: [
      { value: "entrada", label: "Entrada" },
      { value: "salida", label: "Salida" },    ],
    validation: "required",
    inputClass: "select select-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    placeholder: "Seleccione un tipo",
    validationMessages: {
      required: "El tipo es obligatorio",
    },
  },
  {
    $formkit: "text",
    name: "usuario_responsable",
    label: "Usuario responsable",
    validation: "required|length:3",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El usuario responsable es obligatorio",
      length: "Debe tener al menos 3 caracteres",
    },
  },
  {
    $formkit: "textarea",
    name: "motivo",
    label: "Motivo del movimiento",
    validation: "required|length:5",
    inputClass: "textarea textarea-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El motivo es obligatorio",
      length: "Debe tener al menos 5 caracteres",
    },
  },
  {
    $formkit: "number",
    name: "id_documento",
    label: "ID del documento",
    validation: "required|number",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El ID del documento es obligatorio",
      number: "Debe ser un número válido",
    },
  },
];
