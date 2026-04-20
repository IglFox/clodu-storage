import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "../stores/auth";
import FilesView from "../views/FilesView.vue";
import InsightsView from "../views/InsightsView.vue";
import LoginView from "../views/LoginView.vue";
import RegisterView from "../views/RegisterView.vue";

const routes = [
  { path: "/login", component: LoginView, meta: { guest: true } },
  { path: "/register", component: RegisterView, meta: { guest: true } },
  { path: "/files", component: FilesView, meta: { requiresAuth: true } },
  { path: "/insights", component: InsightsView, meta: { requiresAuth: true } },
  { path: "/", redirect: "/files" },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Глобальная защита
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  if (to.meta.requiresAuth && !isAuthenticated) {
    next("/login");
  } else if (to.meta.guest && isAuthenticated) {
    next("/files");
  } else {
    next();
  }
});

export default router;
