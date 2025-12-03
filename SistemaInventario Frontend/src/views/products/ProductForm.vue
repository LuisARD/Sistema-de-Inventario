<template>
  <div class="product-management">
    <div class="header">
      <div class="header-left">
        <div class="icon">+</div>
        <h1>Gestión de Productos</h1>
      </div>
      <div class="header-right">
        <span class="user-greeting">¡Hola, Bienvenido!</span>
        <div class="user-icon">👤</div>
        <button class="btn-logout" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </div>

    <div class="container">
      <form @submit.prevent="crearProducto">
        <div class="form-grid">
          <div class="form-group">
            <label for="nombre">Nombre</label>
            <input 
              type="text" 
              id="nombre" 
              v-model="producto.nombre"
              required
            >
          </div>

          <div class="form-group">
            <label for="stock">Stock mínimo.</label>
            <input 
              type="number" 
              id="stock" 
              v-model="producto.stockMinimo"
              required
            >
          </div>

          <div class="form-group">
            <label for="descripcion">descripcion</label>
            <input 
              type="text" 
              id="descripcion" 
              v-model="producto.descripcion"
            >
          </div>

          <div class="form-group">
            <label for="precio-compra">Precio de compra</label>
            <input 
              type="number" 
              id="precio-compra" 
              v-model="producto.precioCompra"
              step="0.01"
              required
            >
          </div>

          <div class="form-group">
            <label for="categoria">Categoria</label>
            <select 
              id="categoria" 
              v-model="producto.categoria"
              required
            >
              <option value=""></option>
              <option 
                v-for="cat in categorias" 
                :key="cat.id" 
                :value="cat.id"
              >
                {{ cat.nombre }}
              </option>
            </select>
          </div>

          <div class="form-group">
            <label for="precio-venta">Precio de venta</label>
            <input 
              type="number" 
              id="precio-venta" 
              v-model="producto.precioVenta"
              step="0.01"
              required
            >
          </div>

          <div class="form-group">
            <label for="proveedor">Proveedor</label>
            <select 
              id="proveedor" 
              v-model="producto.proveedor"
              required
            >
              <option value=""></option>
              <option 
                v-for="prov in proveedores" 
                :key="prov.id" 
                :value="prov.id"
              >
                {{ prov.nombre }}
              </option>
            </select>
          </div>

          <div class="form-group full-width">
            <label for="unidad">Unidad de medida</label>
            <input 
              type="text" 
              id="unidad" 
              v-model="producto.unidadMedida"
              required
            >
          </div>
        </div>

        <div class="button-container">
          <button type="submit" class="btn-create">Crear Producto</button>
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
  name: 'ProductForm',
  data() {
    return {
      producto: {
        nombre: '',
        stockMinimo: null,
        descripcion: '',
        precioCompra: null,
        categoria: '',
        precioVenta: null,
        proveedor: '',
        unidadMedida: ''
      },
      categorias: [
        { id: 1, nombre: 'Categoría 1' },
        { id: 2, nombre: 'Categoría 2' },
        { id: 3, nombre: 'Categoría 3' }
      ],
      proveedores: [
        { id: 1, nombre: 'Proveedor 1' },
        { id: 2, nombre: 'Proveedor 2' },
        { id: 3, nombre: 'Proveedor 3' }
      ]
    }
  },
  methods: {
    crearProducto() {
      console.log('Producto creado:', this.producto);
      // Aquí puedes hacer la llamada a tu API
      // Por ejemplo:
      // this.$emit('producto-creado', this.producto);
      // O usar axios/fetch para enviar los datos al backend
      
      // Limpiar el formulario después de crear
      this.limpiarFormulario();
    },
    limpiarFormulario() {
      this.producto = {
        nombre: '',
        stockMinimo: null,
        descripcion: '',
        precioCompra: null,
        categoria: '',
        precioVenta: null,
        proveedor: '',
        unidadMedida: ''
      };
    },
    cerrarSesion() {
      console.log('Cerrando sesión...');
      // Aquí puedes implementar la lógica de cierre de sesión
      // Por ejemplo: this.$router.push('/login');
    }
  },
  mounted() {
    // Aquí puedes cargar las categorías y proveedores desde tu API
    // Por ejemplo:
    // this.cargarCategorias();
    // this.cargarProveedores();
  }
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.product-management {
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
  max-width: 800px;
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

.form-group.full-width {
  grid-column: 1 / -1;
}

label {
  font-size: 14px;
  color: #2c3e50;
  margin-bottom: 8px;
  font-weight: 400;
}

input, select {
  padding: 12px 15px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
  transition: border-color 0.3s;
  background-color: white;
}

input:focus, select:focus {
  outline: none;
  border-color: #ff6b35;
}

select {
  cursor: pointer;
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg width='12' height='8' viewBox='0 0 12 8' fill='none' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath d='M1 1.5L6 6.5L11 1.5' stroke='%23666' stroke-width='2' stroke-linecap='round'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 15px center;
  padding-right: 40px;
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