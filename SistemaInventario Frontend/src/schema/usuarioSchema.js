export const usuarioSchema = [
  {
    $formkit: "text",
    name: "nombre",
    label: "Nombre completo",
    validation: "required|length:3",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "El nombre es obligatorio",
      length: "Debe tener al menos 3 caracteres",
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
    $formkit: "password",
    name: "password",
    label: "Contraseña",
    validation: "required|length:6",
    inputClass: "input input-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    validationMessages: {
      required: "La contraseña es obligatoria",
      length: "Debe tener al menos 6 caracteres",
    },
  },
  {
    $formkit: "select",
    name: "rol",
    label: "Rol del usuario",
    options: [
      { value: 1, label: "Administrador" },
      { value: 2, label: "Usuario" },
      { value: 3, label: "Supervisor" },
    ],
    validation: "required",
    inputClass: "select select-bordered w-full",
    outerClass: "mb-4 flex flex-col gap-1",
    placeholder: "Seleccione un rol",
    validationMessages: {
      required: "El rol es obligatorio",
    },
  },
  {
    $formkit: "checkbox",
    name: "activo",
    label: "Usuario activo",
    inputClass: "checkbox checkbox-primary",
    outerClass: "mb-4 flex flex-row items-center gap-3",
    help: "Marque esta opción si el usuario está activo",
  },
  ];