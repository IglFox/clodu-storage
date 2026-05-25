import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from './stores/auth';

import LoginRegister from './views/LoginRegister.vue';
import Dashboard from './views/Dashboard.vue';
import SafeMode from './views/SafeMode.vue';
import OfficeMode from './views/OfficeMode.vue';

const routes = [
  {
    path: '/login',
    name: 'login',
    component: LoginRegister,
    meta: { guest: true }
  },
  {
    path: '/',
    name: 'dashboard',
    component: Dashboard,
    meta: { auth: true }
  },
  {
    path: '/safe',
    name: 'safe',
    component: SafeMode,
    meta: { auth: true }
  },
  {
    path: '/office',
    name: 'office',
    component: OfficeMode,
    meta: { auth: true }
  },
  // Redirect any other route to dashboard
  {
    path: '/:pathMatch(.*)*',
    redirect: '/'
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore();
  
  // Ensure session is loaded before check
  if (!authStore.currentUser && localStorage.getItem('dcs_auth_token')) {
    await authStore.loadSession();
  }

  const isAuthenticated = !!authStore.currentUser;

  if (to.meta.auth && !isAuthenticated) {
    next('/login');
  } else if (to.meta.guest && isAuthenticated) {
    next('/');
  } else {
    next();
  }
});

export default router;
