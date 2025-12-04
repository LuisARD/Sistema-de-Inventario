import { createRouter, createWebHistory } from 'vue-router'
import ProductsList from '../views/products/ProductsList.vue'
import MovementsList from '../views/movements/MovementsList.vue'
import MovementsForm from '../views/movements/MovementsForm.vue'
import CategoriesList from '../views/categories/CategoriesList.vue'
import CategoriesForm from '../views/categories/CategoriesForm.vue'
import SuppliersList from '../views/suppliers/SuppliersList.vue'
import SupplierForm from '../views/suppliers/SupplierForm.vue'
import ReportsView from '../views/reports/ReportsView.vue'
import LoginView from '../views/LoginView.vue'
import ProductForm from '../views/products/ProductForm.vue'
import Home from '../views/Home.vue'
import MainLayout from '../layouts/MainLayout.vue'
import UserForm from '../views/users/UserForm.vue'

const routes = [
  {
    path: "/login",
    component: LoginView,
    name: "Login",
  },

  {
    path: "/",
    component: MainLayout,
    children: [
      { path: "", component: Home, name: "Home" },

      // PRODUCTOS
      { path: "products", component: ProductsList, name: "Products" },
      { path: "products/create", component: ProductForm, name: "ProductCreate" },

      // MOVIMIENTOS
      { path: "movements", component: MovementsList, name: "Movements" },
      { path: "movements/create", component: MovementsForm, name: "MovementCreate" },

      // CATEGORÍAS
      { path: "categories", component: CategoriesList, name: "Categories" },
      { path: "categories/create", component: CategoriesForm, name: "CategoryCreate" },

      // PROVEEDORES
      { path: "suppliers", component: SuppliersList, name: "Suppliers" },
      { path: "suppliers/create", component: SupplierForm, name: "SupplierCreate" },

      // USUARIOS
      { path: "users/create", component: UserForm, name: "UserCreate" },

      // REPORTES
      { path: "reports", component: ReportsView, name: "Reports" },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
