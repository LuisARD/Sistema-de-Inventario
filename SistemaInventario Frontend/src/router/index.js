import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '../views/DashboardView.vue'
import ProductsList from '../views/products/ProductsList.vue'
import MovementsList from '../views/movements/MovementsList.vue'
import MovementsForm from '../views/movements/MovementsForm.vue'
import CategoriesList from '../views/categories/CategoriesList.vue'
import SuppliersList from '../views/suppliers/SuppliersList.vue'
import ReportsView from '../views/reports/ReportsView.vue'
import LoginView from '../views/LoginView.vue'
import ProductForm from '../views/products/ProductForm.vue'  

const routes = [
  { path: '/', component: DashboardView, name: 'Dashboard' },

  // PRODUCTOS
  { path: '/products', component: ProductsList, name: 'Products' },

  { 
    path: '/products/create',
    name: 'ProductCreate',
    component: ProductForm  
  },

  // MOVIMIENTOS
  { 
    path: '/movements', 
    component: MovementsList, 
    name: 'Movements' 
  },

  { 
    path: '/movements/create',
    name: 'MovementCreate',
    component: MovementsForm
  },

  // CATEGORÍAS
  { path: '/categories', component: CategoriesList, name: 'Categories' },

  // PROVEEDORES
  { path: '/suppliers', component: SuppliersList, name: 'Suppliers' },

  // REPORTES
  { path: '/reports', component: ReportsView, name: 'Reports' },

  // LOGIN
  { path: '/login', component: LoginView, name: 'Login' }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
