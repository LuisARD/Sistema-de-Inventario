import { defineStore } from "pinia";
import { jwtDecode } from "jwt-decode";
import API from "../services/axios";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: localStorage.getItem("token") || null,
    user: JSON.parse(localStorage.getItem("user")) || null,
  }),

  actions: {
    async login(nombreUsuario, passwordUsuario) {
      try {
        const respuesta = await API.post(`/Acceso/Login`, {
          user: nombreUsuario,
          passwordUsuario: passwordUsuario,
        });

        this.token = respuesta.data.token;
        localStorage.setItem("token", this.token);

        const decodedToken = jwtDecode(this.token);
        this.user = {
          id: decodedToken[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ],
          user: decodedToken[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/user"
          ],
          rol: decodedToken[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
          ],
        };

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
    },
  },
});
