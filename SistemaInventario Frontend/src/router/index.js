import { createRouter, createWebHistory } from "vue-router";
import ProductsList from "../views/products/ProductsList.vue";
import MovementsList from "../views/movements/MovementsList.vue";
import MovementsForm from "../views/movements/MovementsForm.vue";
import CategoriesList from "../views/categories/CategoriesList.vue";
import SuppliersList from "../views/suppliers/SuppliersList.vue";
import ReportsView from "../views/reports/ReportsView.vue";
import LoginView from "../views/LoginView.vue";
import ProductForm from "../views/products/ProductForm.vue";
import Home from "../views/Home.vue";
import MainLayout from "../layouts/MainLayout.vue";
import { useAuthStore } from "../store/auth";
import SupplierForm from "../views/suppliers/SupplierForm.vue";
import CategoriesForm from "../views/categories/CategoriesForm.vue";
import UserList from "../views/users/UserList.vue";
import UserForm from "../views/users/UserForm.vue";
import ExistenciasList from "../views/existencias/ExistenciasList.vue";
import ExistenciasDetalle from "../views/existencias/ExistenciasDetalle.vue";

const routes = [
  {
    path: "/login",
    component: LoginView,
    name: "Login",
  },

  {
    path: "/",
    component: MainLayout,
    meta: { requiresAuth: true },
    children: [
      { path: "", component: Home, name: "Home" },

      { path: "products", component: ProductsList, name: "Products" },
      {
        path: "products/create",
        component: ProductForm,
        name: "ProductCreate",
      },

      { path: "movements", component: MovementsList, name: "Movements" },
      {
        path: "movements/create",
        component: MovementsForm,
        name: "MovementCreate",
      },

      { path: "categories", component: CategoriesList, name: "Categories" },
      {
        path: "categories/create",
        component: CategoriesForm,
        name: "CategoriesCreate",
      },

      { path: "suppliers", component: SuppliersList, name: "Suppliers" },
      {
        path: "suppliers/create",
        component: SupplierForm,
        name: "SuppliersCreate",
      },

      { path: "existencias", component: ExistenciasList, name: "Existencias" },
      {
        path: "existencias/producto/:productoId",
        component: ExistenciasDetalle,
        name: "ExistenciasDetalle",
      },

      { path: "reports", component: ReportsView, name: "Reports" },
      { path: "usuario", component: UserList, name: "Usuario" },
      { path: "usuario/create", component: UserForm, name: "UsuarioForm" },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to) => {
  const auth = useAuthStore();

  // 1️⃣ BLOQUEAR acceso sin login
  if (to.meta.requiresAuth && !auth.token) {
    return { path: "/login" };
  }

  // 2️⃣ BLOQUEAR login si ya está autenticado
  if (to.path === "/login" && auth.token) {
    return { path: "/" };
  }

  // 3️⃣ BLOQUEAR rutas de admin
  if (to.meta.requiresAdmin && auth.user?.RolNombre !== "admin") {
    return { path: "/" };
  }
});

export default router;
