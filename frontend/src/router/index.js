import { createRouter, createWebHistory } from 'vue-router';
import LoginView from '../components/LoginView.vue';
import DashboardView from '../components/DashboardView.vue';
import ProfileView from '../components/ProfileView.vue';
import { CryptoService } from '../utils/crypto';

const routes = [
  {
    path: '/login',
    name: 'login',
    component: LoginView,
    meta: { guest: true }
  },
  {
    path: '/',
    redirect: '/dashboard'
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: DashboardView,
    meta: { requiresAuth: true }
  },
  {
    path: '/profile',
    name: 'profile',
    component: ProfileView,
    meta: { requiresAuth: true }
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

// Simple Auth Guard
router.beforeEach(async (to, from, next) => {
  const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
  const currentUserStr = localStorage.getItem('currentUser');
  
  let currentUser = null;
  if (currentUserStr) {
    try {
      currentUser = JSON.parse(currentUserStr);
    } catch (e) {}
  }

  // Double check persistent storage for key status (scoped by user email)
  let hasMasterKey = false;
  if (isLoggedIn && currentUser && currentUser.email) {
    const idbKey = await CryptoService.getKey('master_key_' + currentUser.email);
    if (idbKey) {
      localStorage.setItem('masterKey', idbKey.value);
      hasMasterKey = true;
    } else {
      localStorage.removeItem('masterKey');
    }
  } else {
    localStorage.removeItem('masterKey');
  }

  const isVaultUnlocked = sessionStorage.getItem('isVaultUnlocked') === 'true';

  if (to.meta.requiresAuth) {
    if (!isLoggedIn || !currentUser) {
      next({ name: 'login' });
    } else if (hasMasterKey && !isVaultUnlocked) {
      // If we have a key but it's not "unlocked" in this session, force login/unlock
      next({ name: 'login' });
    } else {
      next();
    }
  } else if (to.meta.guest && isLoggedIn && currentUser) {
    // If guest page (login) and already logged in, check if vault needs unlocking
    if (hasMasterKey && !isVaultUnlocked) {
      next(); // Stay on login to unlock
    } else {
      next({ name: 'dashboard' });
    }
  } else {
    next();
  }
});

export default router;
