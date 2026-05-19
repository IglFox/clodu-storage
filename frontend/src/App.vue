<script setup>
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
    ShieldCheck,
    Lock,
    Activity,
    Globe,
    CreditCard,
    Zap,
} from "lucide-vue-next";

// Components
import Sidebar from "./components/Sidebar.vue";
import Header from "./components/Header.vue";

const route = useRoute();
const router = useRouter();

// Auth State - Sync with localStorage
const isLoggedIn = ref(localStorage.getItem("isLoggedIn") === "true");

// Compute breadcrumbs from route name
const breadcrumbs = computed(() => {
    const labels = {
        dashboard: ["Dashboard"],
        files: ["My Storage"],
        security: ["Security Center"],
        configuration: ["System Configuration"],
        profile: ["User Profile"],
    };
    return labels[route.name] || [route.name || "CloudVault"];
});

// User Profile Data
const userProfile = ref({
    name: "",
    email: "",
    role: "",
    avatar: null,
    joinedDate: "",
    twoFactorEnabled: false,
});

// Pull real data via composable
import { useDashboard } from "@/composables/useDashboard";

const {
    files,
    storage: storageStats,
    security: securitySettings,
    config: configSettings,
    loading,
    error,
    refetch,
} = useDashboard();

// Handlers
const handleLogin = () => {
    localStorage.setItem("isLoggedIn", "true");
    isLoggedIn.value = true;
    router.push("/dashboard");
};

const handleLogout = () => {
    localStorage.removeItem("isLoggedIn");
    isLoggedIn.value = false;
    router.push("/login");
};

const navigate = (view) => {
    router.push({ name: view });
};

// Check auth on mount to handle direct URL entry if state changed
onMounted(() => {
    isLoggedIn.value = localStorage.getItem("isLoggedIn") === "true";
});
</script>

<template>
    <div
        class="min-h-screen bg-[#FDFCFB] text-[#1A1A1A] font-sans selection:bg-[#FFDAB9]"
    >
        <template v-if="!isLoggedIn || route.name === 'login'">
            <router-view @login="handleLogin" />
        </template>

        <template v-else>
            <Sidebar
                :current-view="route.name"
                :storage-percent="storageStats.percent"
                :storage-used="storageStats.used"
                :storage-total="storageStats.total"
                @navigate="navigate"
            />

            <main class="ml-64 p-8 min-h-screen">
                <Header
                    :breadcrumbs="breadcrumbs"
                    :user-name="userProfile.name"
                    @open-profile="navigate('profile')"
                />

                <div class="max-w-5xl mx-auto">
                    <router-view v-slot="{ Component }">
                        <component
                            :is="Component"
                            :stats="storageStats"
                            :files="files"
                            :settings="
                                route.name === 'security'
                                    ? securitySettings
                                    : configSettings
                            "
                            :user="userProfile"
                            @logout="handleLogout"
                        />
                    </router-view>
                </div>
            </main>
        </template>
    </div>
</template>

<style>
@import url("https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&family=Playfair+Display:ital,wght@0,700;1,700&display=swap");

.serif {
    font-family: "Playfair Display", serif;
}

body {
    overflow-x: hidden;
}

.shadow-sm {
    box-shadow:
        0 1px 3px rgba(0, 0, 0, 0.05),
        0 1px 2px rgba(0, 0, 0, 0.03);
}

.shadow-md {
    box-shadow:
        0 4px 6px -1px rgba(0, 0, 0, 0.05),
        0 2px 4px -1px rgba(0, 0, 0, 0.03);
}

.animate-in {
    animation-duration: 400ms;
    animation-timing-function: cubic-bezier(0.16, 1, 0.3, 1);
    animation-fill-mode: forwards;
}

.fade-in {
    animation-name: fadeIn;
}

.slide-in-from-bottom-4 {
    animation-name: slideInFromBottom4;
}

.slide-in-from-bottom-8 {
    animation-name: slideInFromBottom8;
}

@keyframes fadeIn {
    from {
        opacity: 0;
    }
    to {
        opacity: 1;
    }
}

@keyframes slideInFromBottom4 {
    from {
        transform: translateY(1rem);
    }
    to {
        transform: translateY(0);
    }
}

@keyframes slideInFromBottom8 {
    from {
        transform: translateY(2rem);
    }
    to {
        transform: translateY(0);
    }
}
</style>
