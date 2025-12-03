<template>
  <div class="category-form">
    <!-- HEADER -->
    <div class="header">
      <div class="header-left">
        <div class="icon">+</div>
        <h1>Categorías de productos</h1>
      </div>

      <div class="header-right">
        <span class="user-greeting">¡Hola, Bienvenido!</span>
        <div class="user-icon">👤</div>
        <button class="btn-logout" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </div>

    <!-- FORMULARIO -->
    <div class="container">
      <form @submit.prevent="crearCategoria">
        <div class="form-grid">
          <div class="form-group">
            <label for="nombre">Nombre de la categoría</label>
            <input
              type="text"
              id="nombre"
              v-model="categoria.nombre"
              required
            >
          </div>

          <div class="form-group">
            <label for="descripcion">Descripción</label>
            <input
              type="text"
              id="descripcion"
              v-model="categoria.descripcion"
            >
          </div>
        </div>

        <div class="button-container">
          <button type="submit" class="btn-create">Crear Categoría</button>
        </div>
      </form>

      <div class="footer">
        <span>¿Necesita asistencia?</span>
        <div class="help-icon">🎧</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "CategoriesForm",
  data() {
    return {
      categoria: {
        nombre: "",
        descripcion: ""
      }
    };
  },
  methods: {
    crearCategoria() {
      console.log("Categoría creada:", this.categoria);
      this.limpiarFormulario();
    },
    limpiarFormulario() {
      this.categoria = {
        nombre: "",
        descripcion: ""
      };
    },
    cerrarSesion() {
      this.$router.push("/login");
    }
  }
};
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.category-form {
  background-color: hsl(0, 0%, 100%);
  min-height: 100vh;
  padding: 20px;
}

.header {
  padding: 20px;
  margin-bottom: 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-radius: 8px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 15px;
}

.icon {
  width: 40px;
  height: 40px;
  background-color: #ff6b35;
  border-radius: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  color: white;
}

.header h1 {
  font-size: 28px;
  color: #2c3e50;
  font-weight: 400;
}

.header-right {
  display: flex;
  gap: 15px;
  align-items: center;
}

.user-greeting {
  color: #666;
  font-size: 14px;
}

.user-icon {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 2px solid #666;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-logout {
  background-color: #ff6b35;
  color: white;
  border: none;
  padding: 10px 25px;
  border-radius: 25px;
  cursor: pointer;
  font-size: 14px;
  transition: background-color 0.3s;
}

.btn-logout:hover {
  background-color: #e55a2b;
}

.container {
  border-radius: 12px;
  padding: 40px;
  max-width: 900px;
  margin: 0 auto;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 25px;
  margin-bottom: 30px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  font-size: 14px;
  color: #2c3e50;
  margin-bottom: 8px;
}

input {
  padding: 12px 15px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  background-color: white;
  transition: border-color 0.3s;
}

input:focus {
  outline: none;
  border-color: #ff6b35;
}

.button-container {
  display: flex;
  justify-content: flex-end;
  margin-top: 30px;
}

.btn-create {
  background-color: #ff6b35;
  color: white;
  border: none;
  padding: 12px 35px;
  border-radius: 25px;
  cursor: pointer;
  font-size: 15px;
  font-weight: 500;
  transition: background-color 0.3s;
}

.btn-create:hover {
  background-color: #e55a2b;
}

.footer {
  text-align: right;
  margin-top: 30px;
  color: #999;
  font-size: 13px;
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 8px;
}

.help-icon {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  border: 2px solid #999;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 14px;
}

/* Responsive */
@media (max-width: 768px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
  
  .header {
    flex-direction: column;
    gap: 15px;
  }
  
  .header-right {
    width: 100%;
    justify-content: space-between;
  }
}
</style>
