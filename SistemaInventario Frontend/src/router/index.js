import { createRouter, createWebHistory } from 'vue-router'
import ProductsList from '../views/products/ProductsList.vue'
import MovementsList from '../views/movements/MovementsList.vue'
import MovementsForm from '../views/movements/MovementsForm.vue'
import CategoriesList from '../views/categories/CategoriesList.vue'
import CategoriesForm from '../views/categories/CategoriesForm.vue'
import SuppliersList from '../views/suppliers/SuppliersList.vue'
import ReportsView from '../views/reports/ReportsView.vue'
import LoginView from '../views/LoginView.vue'
import ProductForm from '../views/products/ProductForm.vue'  
import Home from '../views/Home.vue'
import MainLayout from '../layouts/MainLayout.vue'
import SupplierForm from '../views/suppliers/SupplierForm.vue'



const routes = [
  {
    path: "/login",
    component: LoginView,
    name: "Login",
  },

   // CATEGORÍAS
  { 
    path: '/categories/create',
    name: 'CategoryCreate',
    component: CategoriesForm
  },

  {
    path: "/",
    component: MainLayout,
    // meta: { requiresAuth: true },
    children: [
      { path: "", component: Home, name: "Home" },


      { path: "products", component: ProductsList, name: "Products" },
      { path: "products/create", component: ProductForm, name: "ProductCreate" },


      { path: "movements", component: MovementsList, name: "Movements" },
      { path: "movements/create", component: MovementsForm, name: "MovementCreate" },

     
      { path: "categories", component: CategoriesList, name: "Categories" },

     
      { path: "suppliers", component: SuppliersList, name: "Suppliers" },

      
      { path: "reports", component: ReportsView, name: "Reports" },

    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
