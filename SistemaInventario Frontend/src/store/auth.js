import { defineStore } from "pinia";
import {jwtDecode} from "jwt-decode";
import API from "../services/axios";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: localStorage.getItem("token") || null,
    user: JSON.parse(localStorage.getItem("user")) || null,
  }),

  actions: {
    async login(nombreUsuario, passwordUsuario) {
      try {
        const respuesta = await API.post(`/Auth/login`, {
          Email: nombreUsuario,
          Password: passwordUsuario,
        });

        // depuración: ver estructura recibida

        // Guardar toda la respuesta en localStorage
        try {
          localStorage.setItem("authResponse", JSON.stringify(respuesta.data));
        } catch (e) {
          console.warn("No se pudo guardar authResponse:", e);
        }

        // soportar varias formas: Token (mayúscula), token, accessToken...
        const token =
          respuesta?.data?.token ||
          respuesta?.data?.Token ||
          respuesta?.data?.accessToken ||
          null;

        if (!token || typeof token !== "string") {
          console.error("Login: token inválido o ausente:", respuesta?.data);
          return false;
        }

        this.token = token;
        localStorage.setItem("token", this.token);

        // Si el backend ya devuelve un objeto Usuario, úsalo directamente
        if (respuesta?.data?.Usuario) {
          this.user = respuesta.data.Usuario;
        } else {
          // si no, intenta decodificar el token para construir user
          let decodedToken;
          try {
            decodedToken = jwtDecode(this.token);
          } catch (e) {
            console.error("jwtDecode falló:", e);
            // aun así guardamos la respuesta completa y el token
            localStorage.setItem("user", JSON.stringify(this.user));
            return true;
          }

          this.user = {
            id:
              decodedToken[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
              ] || decodedToken.sub,
            Email:
              decodedToken[
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/user"
              ] || decodedToken.email,
            rol:
              decodedToken[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
              ] || decodedToken.role,
            // añadir otros claims si se necesitan
          };
        }

        // guardar user en localStorage
        try {
          localStorage.setItem("user", JSON.stringify(this.user));
        } catch (e) {
          console.warn("No se pudo guardar user:", e);
        }

        return true;
      } catch (error) {
        console.error("error login:", error);
        return false;
      }
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      localStorage.removeItem("authResponse");
    },
  },
});
