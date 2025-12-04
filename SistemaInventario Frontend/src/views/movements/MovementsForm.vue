<template>
  <div class="movement-form">
    <div class="header">
      <div class="header-left">
        <div class="icon">+</div>
        <h1>Registro de movimiento</h1>
      </div>

      <div class="header-right">
        <span class="user-greeting">¡Hola, Bienvenido!</span>
        <div class="user-icon">👤</div>
        <button class="btn-logout" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </div>

    <div class="container">
      <form @submit.prevent="crearMovimiento">
        <div class="form-grid">

          <div class="form-group">
            <label for="fecha">Fecha</label>
            <input type="date" id="fecha" v-model="movimiento.fecha" required>
          </div>

          <div class="form-group">
            <label for="tipo">Tipo</label>
            <select id="tipo" v-model="movimiento.tipo" required>
              <option value=""></option>
              <option value="Entrada">Entrada</option>
              <option value="Salida">Salida</option>
            </select>
          </div>

          <div class="form-group">
            <label for="usuario">Usuario responsable</label>
            <input type="text" id="usuario" v-model="movimiento.usuario" required>
          </div>

          <div class="form-group">
            <label for="motivo">Motivo</label>
            <input type="text" id="motivo" v-model="movimiento.motivo" required>
          </div>

          <div class="form-group full-width">
            <label for="documento">ID de documentos</label>
            <input type="text" id="documento" v-model="movimiento.documento">
          </div>

        </div>

        <div class="button-container">
          <button type="submit" class="btn-create">Crear Movimiento</button>
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
  name: "MovementForm",
  data() {
    return {
      movimiento: {
        fecha: "",
        tipo: "",
        usuario: "",
        motivo: "",
        documento: ""
      }
    };
  },
  methods: {
    crearMovimiento() {
      console.log("Movimiento creado:", this.movimiento);
      this.limpiar();
    },
    limpiar() {
      this.movimiento = {
        fecha: "",
        tipo: "",
        usuario: "",
        motivo: "",
        documento: ""
      };
    },
    cerrarSesion() {
      this.$router.push('/login');
    }
  }
};
</script>

<style scoped>

.movement-form {
  background: white;
  min-height: 100vh;
  padding: 20px;
}

.header {
  padding: 20px;
  margin-bottom: 30px;
  display: flex;
  justify-content: space-between;
  align-items: center;
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

h1 {
  font-size: 26px;
  color: #1e3455;
  font-weight: 500;
}

.header-right {
  display: flex;
  align-items: center;
  gap: 20px;
}

.user-greeting {
  color: #444;
}

.btn-logout {
  background: #ff6b35;
  color: white;
  border: none;
  padding: 8px 22px;
  border-radius: 25px;
  cursor: pointer;
}

.container {
  max-width: 900px;
  margin: 0 auto;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 25px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group.full-width {
  grid-column: span 2;
}

label {
  color: #1e3455;
  margin-bottom: 8px;
  font-size: 14px;
}

input, select {
  border: 1px solid #1e3455;
  padding: 12px;
  border-radius: 8px;
  font-size: 14px;
  background: white;
}

input:focus,
select:focus {
  outline: none;
  border-color: #ff6b35;
}

.button-container {
  display: flex;
  justify-content: flex-end;
  margin-top: 30px;
}

.btn-create {
  background: #ff6b35;
  color: white;
  padding: 12px 30px;
  border: none;
  border-radius: 25px;
  cursor: pointer;
}

.footer {
  margin-top: 30px;
  text-align: right;
  color: #777;
  display: flex;
  justify-content: flex-end;
  gap: 8px;
}

.help-icon {
  width: 26px;
  height: 26px;
  border: 2px solid #777;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
