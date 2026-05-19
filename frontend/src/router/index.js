import { createRouter, createWebHistory } from "vue-router";
import LoginView from "../components/LoginView.vue";
import DashboardView from "../components/DashboardView.vue";
import FilesView from "../components/FilesView.vue";
import SecurityView from "../components/SecurityView.vue";
import ConfigurationView from "../components/ConfigurationView.vue";
import ProfileView from "../components/ProfileView.vue";

const routes = [
    {
        path: "/login",
        name: "login",
        component: LoginView,
        meta: { guest: true },
    },
    {
        path: "/",
        redirect: "/dashboard",
    },
    {
        path: "/dashboard",
        name: "dashboard",
        component: DashboardView,
        meta: { requiresAuth: true },
    },
    {
        path: "/files",
        name: "files",
        component: FilesView,
        meta: { requiresAuth: true },
    },
    {
        path: "/security",
        name: "security",
        component: SecurityView,
        meta: { requiresAuth: true },
    },
    {
        path: "/configuration",
        name: "configuration",
        component: ConfigurationView,
        meta: { requiresAuth: true },
    },
    {
        path: "/profile",
        name: "profile",
        component: ProfileView,
        meta: { requiresAuth: true },
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// Simple Auth Guard
router.beforeEach((to, from, next) => {
    const isLoggedIn = localStorage.getItem("isLoggedIn") === "true";

    if (to.meta.requiresAuth && !isLoggedIn) {
        next({ name: "login" });
    } else if (to.meta.guest && isLoggedIn) {
        next({ name: "dashboard" });
    } else {
        next();
    }
});

export default router;
