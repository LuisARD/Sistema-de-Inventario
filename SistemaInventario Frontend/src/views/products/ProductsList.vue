<template>
  <div class="products-list">
    <div class="header">
      <div class="header-left">
        <div class="icon">
          <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"></path>
            <polyline points="9 22 9 12 15 12 15 22"></polyline>
          </svg>
        </div>
        <h1>Gestión de Productos</h1>
      </div>
      <div class="header-right">
        <span class="user-greeting">¡Hola, Bienvenido!</span>
        <div class="user-icon">👤</div>
        <button class="btn-logout" @click="cerrarSesion">Cerrar sesión</button>
      </div>
    </div>

    <div class="container">
      <div class="controls">
        <div class="search-box">
          <input 
            type="text" 
            v-model="searchQuery" 
            placeholder="🔍"
            class="search-input"
          >
        </div>
        <button class="btn-create" @click="crearProducto">Crear Productos</button>
      </div>

      <div class="table-container">
        <table class="products-table">
          <thead>
            <tr>
              <th>código</th>
              <th>Nombre</th>
              <th>Descripción</th>
              <th>categoría</th>
              <th>proveedor</th>
              <th>Precio compra</th>
              <th>Precio venta</th>
              <th>unidad de medida</th>
              <th>Stock mínimo</th>
              <th>Acción</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="product in filteredProducts" :key="product.codigo">
              <td class="codigo">{{ product.codigo }}</td>
              <td class="nombre">{{ product.nombre }}</td>
              <td class="descripcion">{{ product.descripcion }}</td>
              <td>{{ product.categoria }}</td>
              <td class="proveedor">{{ product.proveedor }}</td>
              <td>{{ product.precioCompra }}</td>
              <td>{{ product.precioVenta }}</td>
              <td>{{ product.unidadMedida }}</td>
              <td>{{ product.stockMinimo }}</td>
              <td class="actions">
                <button class="btn-icon btn-edit" @click="editarProducto(product)" title="Editar">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                    <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                  </svg>
                </button>
                <button class="btn-icon btn-delete" @click="eliminarProducto(product)" title="Eliminar">
                  <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="3 6 5 6 21 6"></polyline>
                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                  </svg>
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="footer">
        <span>¿Necesita asistencia?</span>
        <div class="help-icon">🎧</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProductsList',
  data() {
    return {
      searchQuery: '',
      products: [
        {
          codigo: 'P-001',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-002',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-003',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-004',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-005',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-006',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-007',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-008',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-009',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        },
        {
          codigo: 'P-010',
          nombre: 'Café Molido',
          descripcion: 'Café molido premium 1 lb',
          categoria: 'Alimentos',
          proveedor: 'Distribuidora Castillo',
          precioCompra: 210,
          precioVenta: 300,
          unidadMedida: '25',
          stockMinimo: 50
        }
      ]
    }
  },
  computed: {
    filteredProducts() {
      if (!this.searchQuery) {
        return this.products
      }
      const query = this.searchQuery.toLowerCase()
      return this.products.filter(product => 
        product.nombre.toLowerCase().includes(query) ||
        product.codigo.toLowerCase().includes(query) ||
        product.categoria.toLowerCase().includes(query)
      )
    }
  },
  methods: {
    crearProducto() {
      this.$router.push('/products/create')
    },
    editarProducto(product) {
      console.log('Editar producto:', product)
      // Aquí puedes navegar a la vista de edición
      // this.$router.push(`/products/edit/${product.codigo}`)
    },
    eliminarProducto(product) {
      if (confirm(`¿Está seguro de eliminar el producto ${product.nombre}?`)) {
        console.log('Eliminar producto:', product)
        // Aquí puedes hacer la llamada a tu API para eliminar
      }
    },
    cerrarSesion() {
      console.log('Cerrando sesión...')
      this.$router.push('/login')
    }
  }
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.products-list {
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
}

.search-box {
  flex: 0 0 250px;
}

.search-input {
  width: 100%;
  padding: 10px 15px;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 14px;
}

.search-input:focus {
  outline: none;
  border-color: #ff6b35;
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
  margin-bottom: 20px;
}

.products-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 13px;
}

.products-table thead {
  background-color: #ffddd0;
}

.products-table th {
  padding: 12px 10px;
  text-align: left;
  font-weight: 500;
  color: #2c3e50;
  border-bottom: 2px solid #ffb396;
  white-space: nowrap;
}

.products-table tbody tr {
  background-color: #e6d5ff;
  border-bottom: 1px solid #d4bfff;
  transition: background-color 0.2s;
}

.products-table tbody tr:hover {
  background-color: #dac8ff;
}

.products-table td {
  padding: 12px 10px;
  color: #2c3e50;
}

.products-table td.codigo {
  color: #5b21b6;
  font-weight: 600;
}

.products-table td.nombre {
  font-weight: 500;
}

.products-table td.descripcion {
  max-width: 200px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.products-table td.proveedor {
  font-size: 12px;
}

.products-table td.actions {
  display: flex;
  gap: 8px;
  align-items: center;
}

.btn-icon {
  background: none;
  border: none;
  cursor: pointer;
  padding: 4px;
  border-radius: 4px;
  transition: background-color 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-edit {
  color: #5b21b6;
}

.btn-edit:hover {
  background-color: rgba(91, 33, 182, 0.1);
}

.btn-delete {
  color: #5b21b6;
}

.btn-delete:hover {
  background-color: rgba(91, 33, 182, 0.1);
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
  .header {
    flex-direction: column;
    gap: 15px;
  }
  
  .header-right {
    width: 100%;
    justify-content: space-between;
  }
  
  .controls {
    flex-direction: column;
    gap: 15px;
  }
  
  .search-box {
    width: 100%;
  }
}
</style>