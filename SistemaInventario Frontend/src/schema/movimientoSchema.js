export const movimientoSchema = [
  {
    $formkit: "select",
    name: "TipoMovimiento",
    label: "Tipo de movimiento",
    placeholder: "Seleccione un tipo",
    options: [
      { value: "entrada", label: "Entrada" },
      { value: "salida", label: "Salida" },
    ],
    validation: "required",
    inputClass: "select select-bordered w-full",
  },

  {
    $formkit: "textarea",
    name: "Motivo",
    label: "Motivo del movimiento",
    validation: "required|length:5",
    inputClass: "textarea textarea-bordered w-full",
  },

  {
    $formkit: "number",
    name: "ReferenciaDocumento",
    label: "ID del documento",
    validation: "required|number",
    inputClass: "input input-bordered w-full",
  },

  {
    $formkit: "select",
    name: "UsuarioId",
    label: "Usuario responsable",
    options: "$usuarios",
    placeholder: "Seleccione un usuario",
    validation: "required",
    inputClass: "select select-bordered w-full",
  },

  {
    $formkit: "number",
    name: "AlmacenOrigenId",
    label: "Almacén origen",
    validation: "required|number",
    inputClass: "input input-bordered w-full",
  },

  {
    $formkit: "number",
    name: "AlmacenDestinoId",
    label: "Almacén destino",
    validation: "required|number",
    inputClass: "input input-bordered w-full",
  },
];
