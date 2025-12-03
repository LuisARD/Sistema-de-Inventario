<template>
  <div class="movements-list">
    <!-- HEADER -->
    <div class="header">
      <div class="header-left">
        <div class="icon">
          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" 
            viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M3 3v18h18"></path>
            <path d="M18 17V9a1 1 0 0 0-1-1H8"></path>
            <path d="M8 3v6h6"></path>
          </svg>
        </div>
        <h1>Movimientos de Inventario</h1>
      </div>

      <div class="header-right">
        <span class="user-greeting">¡Hola, Bienvenido!</span>
        <div class="user-icon">👤</div>
        <button class="btn-logout" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </div>

    <!-- CONTENIDO -->
    <div class="container">

      <!-- CONTROLES -->
      <div class="controls">
        <div class="search-box">
          <input 
            type="text"
            v-model="searchQuery"
            placeholder="🔍"
            class="search-input"
          >
        </div>

        <select v-model="selectedType" class="filter-select">
          <option value="">Todos los tipos</option>
          <option value="Entrada">Entrada</option>
          <option value="Salida">Salida</option>
          <option value="Ajuste">Ajuste</option>
        </select>

        <button class="btn-create" @click="crearMovimiento">Crear movimiento</button>
      </div>

      <!-- TABLA -->
      <div class="table-container">
        <table class="movements-table">
          <thead>
            <tr>
              <th>Fecha</th>
              <th>Tipo</th>
              <th>Motivo</th>
              <th>Referencia documento</th>
              <th>Usuario</th>
              <th>Origen</th>
              <th>Destino</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="m in filteredMovements" :key="m.id">
              <td>{{ m.fecha }}</td>
              <td>{{ m.tipo }}</td>
              <td>{{ m.motivo }}</td>
              <td>{{ m.referencia }}</td>
              <td>{{ m.usuario }}</td>
              <td>{{ m.origen }}</td>
              <td>{{ m.destino }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- FOOTER -->
      <div class="footer">
        <span>¿Necesita asistencia?</span>
        <div class="help-icon">🎧</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "MovementList",
  data() {
    return {
      searchQuery: "",
      selectedType: "",
      movimientos: [
        {
          id: 1,
          fecha: "2025-12-01",
          tipo: "Entrada",
          motivo: "Compra",
          referencia: "Distribuidora Caribe",
          usuario: "Distribuidora Caribe",
          origen: "210",
          destino: "290"
        },
        {
          id: 2,
          fecha: "2025-12-02",
          tipo: "Salida",
          motivo: "Alimentos",
          referencia: "Distribuidora Caribe",
          usuario: "Distribuidora Castillo",
          origen: "210",
          destino: "290"
        },
        {
          id: 3,
          fecha: "2025-12-02",
          tipo: "Entrada",
          motivo: "Reposición",
          referencia: "Distribuidora Caribe",
          usuario: "Distribuidora Castillo",
          origen: "210",
          destino: "290"
        }
      ]
    };
  },

  computed: {
    filteredMovements() {
      return this.movimientos.filter(m => {
        const matchesText =
          m.motivo.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          m.referencia.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          m.usuario.toLowerCase().includes(this.searchQuery.toLowerCase());

        const matchesType =
          this.selectedType === "" || m.tipo === this.selectedType;

        return matchesText && matchesType;
      });
    }
  },

  methods: {
    crearMovimiento() {
      this.$router.push("/movements/create");
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

.movements-list {
  background-color: #f5f5f5;
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
  padding: 30px;
}

.controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 25px;
  gap: 15px;
}

.search-input {
  padding: 10px 15px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  width: 200px;
}

.search-input:focus {
  border-color: #ff6b35;
  outline: none;
}

.filter-select {
  padding: 10px 15px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  cursor: pointer;
}

.btn-create {
  background-color: #ff6b35;
  color: white;
  border: none;
  padding: 10px 25px;
  border-radius: 25px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  transition: background-color 0.3s;
}

.btn-create:hover {
  background-color: #e55a2b;
}

.table-container {
  overflow-x: auto;
}

.movements-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}

.movements-table thead {
  background-color: #ffddd0;
}

.movements-table th {
  padding: 12px 10px;
  text-align: left;
  font-weight: 500;
  color: #2c3e50;
  border-bottom: 2px solid #ffb396;
}

.movements-table tbody tr {
  background-color: #e6d5ff;
  border-bottom: 1px solid #d4bfff;
}

.movements-table tbody tr:hover {
  background-color: #dac8ff;
}

.movements-table td {
  padding: 12px 10px;
  color: #2c3e50;
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
</style>
