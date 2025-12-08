export const almacenSchema = [
  {
    $formkit: "text",
    name: "Nombre",
    label: "Nombre del Almacén",
    placeholder: "Ej: Almacén Principal",
    validation: "required",
    outerClass: "col-span-2",
  },
  {
    $formkit: "text",
    name: "Ubicacion",
    label: "Ubicación",
    placeholder: "Ej: Calle 123, Ciudad",
    outerClass: "col-span-2",
  },
];
