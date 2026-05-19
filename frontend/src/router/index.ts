import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import MainView from '../views/MainView.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'login',
    component: LoginView,
  },
  {
    path: '/main',
    name: 'main',
    component: MainView,
  },
]

// Only add analytics route if enabled
if (import.meta.env.VITE_ENABLE_ANALYTICS === 'true') {
  const AnalyticsView = () => import('../views/AnalyticsView.vue')
  routes.push({
    path: '/analytics',
    name: 'analytics',
    component: AnalyticsView,
  })
}

// Redirect root to login
routes.push({
  path: '/',
  redirect: '/login',
})

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

// Navigation guards for protected routes
router.beforeEach((to, _from, next) => {
  const token = localStorage.getItem('authToken')
  const protectedRoutes = ['main', 'analytics']

  if (protectedRoutes.includes(to.name as string)) {
    if (!token) {
      next('/login')
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router
