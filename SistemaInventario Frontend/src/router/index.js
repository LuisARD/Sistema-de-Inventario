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
import AlmacenesList from "../views/almacenes/AlmacenesList.vue";
import AlmacenesForm from "../views/almacenes/AlmacenesForm.vue";

const routes = [
  {
    path: "/login",
    component: LoginView,
    name: "Login",
    meta: { requiresGuest: true },
  },

  {
    path: "/",
    component: MainLayout,
    meta: { requiresAuth: true },
    children: [
      { path: "", component: Home, name: "Home" },

      { 
        path: "products", 
        component: ProductsList, 
        name: "Products",
        meta: { roles: ["Admin", "Usuario", "Supervisor"] }
      },
      {
        path: "products/create",
        component: ProductForm,
        name: "ProductCreate",
        meta: { roles: ["Admin", "Usuario", "Supervisor"] }
      },

      { 
        path: "movements", 
        component: MovementsList, 
        name: "Movements",
        meta: { roles: ["Admin", "Usuario", "Supervisor"] }
      },
      {
        path: "movements/create",
        component: MovementsForm,
        name: "MovementCreate",
        meta: { roles: ["Admin", "Supervisor"] }
      },

      { 
        path: "categories", 
        component: CategoriesList, 
        name: "Categories",
        meta: { roles: ["Admin", "Usuario", "Supervisor"] }
      },
      {
        path: "categories/create",
        component: CategoriesForm,
        name: "CategoriesCreate",
        meta: { roles: ["Admin", "Supervisor"] }
      },

      { 
        path: "suppliers", 
        component: SuppliersList, 
        name: "Suppliers",
        meta: { roles: ["Admin", "Supervisor"] }
      },
      {
        path: "suppliers/create",
        component: SupplierForm,
        name: "SuppliersCreate",
        meta: { roles: ["Admin", "Supervisor"] }
      },

      { 
        path: "existencias", 
        component: ExistenciasList, 
        name: "Existencias",
        meta: { roles: ["Admin", "Supervisor"] }
      },
      {
        path: "existencias/producto/:productoId",
        component: ExistenciasDetalle,
        name: "ExistenciasDetalle",
        meta: { roles: ["Admin", "Supervisor"] }
      },

      { 
        path: "almacenes", 
        component: AlmacenesList, 
        name: "Almacenes",
        meta: { roles: ["Admin", "Supervisor"] }
      },
      {
        path: "almacenes/create",
        component: AlmacenesForm,
        name: "AlmacenesCreate",
        meta: { roles: ["Admin", "Supervisor"] }
      },

      { 
        path: "reports", 
        component: ReportsView, 
        name: "Reports",
        meta: { roles: ["Admin", "Supervisor"] }
      },
      { 
        path: "usuario", 
        component: UserList, 
        name: "Usuario",
        meta: { roles: ["Admin", "Supervisor"] }
      },
      { 
        path: "usuario/create", 
        component: UserForm, 
        name: "UsuarioForm",
        meta: { roles: ["Admin", "Supervisor"] }
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const auth = useAuthStore();
  const isAuthenticated = auth.token && auth.user;

  // Si la ruta requiere autenticación y no está logueado
  if (to.meta.requiresAuth && !isAuthenticated) {
    return next("/login");
  }

  // Si intenta acceder al login estando ya logueado
  if (to.meta.requiresGuest && isAuthenticated) {
    return next("/");
  }

  // Verificar roles si la ruta los requiere
  if (to.meta.roles && isAuthenticated) {
    const userRole = auth.user?.RolNombre;
    if (!to.meta.roles.includes(userRole)) {
      // Si no tiene permiso, redirigir al home
      return next("/");
    }
  }

  // Permitir navegación
  next();
});

export default router;
