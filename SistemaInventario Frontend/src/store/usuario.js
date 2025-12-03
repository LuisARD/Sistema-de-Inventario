import { defineStore } from "pinia";
import API from "../services/axios";

export const useUsuarioStore = defineStore("usuario", {
  state: () => ({
    cargando: false,
    error: null,
    mensaje: null,
  }),

  action: {
    async crearUsuario(nuevoUsuario) {
      this.cargando = true;
      this.error = null;
      this.mensaje = null;

      try {
        const { data } = await API.post("/Acceso/Registrarse", nuevoUsuario);

        this.mensaje = data?.mensaje || "Usuario creado correctamente";

        return data;
      } catch (err) {
        this.error =
          err.response?.data?.message ||
          err.response?.data ||
          "Ocurrio un error al registrar el usuario";
        console.error("Error al crear usuario:", this.error);
        throw err;
      }finally{
        this.cargando = false
      }
    },
  },
});
