<script setup>
import { onMounted } from 'vue';
import { useAuthStore } from './stores/auth';
import { useFileStore } from './stores/files';
import { useSettingsStore } from './stores/settings';
import { useRouter, useRoute } from 'vue-router';

// Import Components
import Sidebar from './components/Sidebar.vue';
import Header from './components/Header.vue';
import TransfersPanel from './components/TransfersPanel.vue';

// Icons for the icon-rail
import { 
  LayoutGrid, 
  ShieldCheck, 
  Briefcase, 
  LogOut,
  Cloud,
  Moon,
  Sun
} from '@lucide/vue';

const authStore = useAuthStore();
const fileStore = useFileStore();
const settingsStore = useSettingsStore();
const router = useRouter();
const route = useRoute();

const railItems = [
  { id: 'dashboard', path: '/', icon: LayoutGrid, label: 'Explorer' },
  { id: 'safe', path: '/safe', icon: ShieldCheck, label: 'Vault' },
  { id: 'office', path: '/office', icon: Briefcase, label: 'Office' },
];

onMounted(async () => {
  await authStore.loadSession();
});

async function handleLogout() {
  await authStore.logout();
  router.push('/login');
}
</script>

<template>
  <div class="min-h-screen bg-canvas font-sans text-text-primary overflow-hidden">
    <!-- Guest Routes (Login/Register) -->
    <router-view v-if="route.meta.guest" />

    <!-- Authenticated Layout -->
    <div v-else-if="authStore.currentUser" class="layout-grid h-screen overflow-hidden">
      
      <!-- COLUMN 1: Icon Rail -->
      <aside class="bg-rail-bg flex flex-col items-center py-6 gap-6 z-50 transition-colors duration-200">
        <div class="mb-2">
          <Cloud class="w-6 h-6 text-white opacity-90" />
        </div>
        
        <nav class="flex flex-col gap-4">
          <router-link 
            v-for="item in railItems" 
            :key="item.id"
            :to="item.path"
            class="p-2 rounded-lg transition-all duration-200 group relative"
            :class="route.path === item.path ? 'bg-white/10' : 'hover:bg-white/5'"
          >
            <component 
              :is="item.icon" 
              class="w-5 h-5" 
              :class="route.path === item.path ? 'text-white' : 'text-white/40 group-hover:text-white/60'" 
            />
            <div v-if="route.path === item.path" class="absolute left-0 top-1/4 bottom-1/4 w-0.5 bg-white rounded-r"></div>
          </router-link>
        </nav>

        <div class="mt-auto flex flex-col gap-4 pb-4">
          <!-- Theme Toggle -->
          <button 
            @click="settingsStore.toggleDarkMode" 
            class="p-2 text-white/40 hover:text-white/80 transition-colors"
            :title="settingsStore.isDarkMode ? 'Switch to Light Mode' : 'Switch to Dark Mode'"
          >
            <Sun v-if="settingsStore.isDarkMode" class="w-5 h-5" />
            <Moon v-else class="w-5 h-5" />
          </button>

          <button @click="handleLogout" class="p-2 text-white/40 hover:text-white/80 transition-colors" title="Sign Out">
            <LogOut class="w-5 h-5" />
          </button>
        </div>
      </aside>

      <!-- COLUMN 2: Sidebar -->
      <Sidebar :active-tab="route.name" />

      <!-- COLUMN 3: Main -->
      <div class="flex flex-col min-w-0 bg-canvas overflow-hidden">
        <Header />
        <main class="flex-1 overflow-y-auto p-8">
          <router-view v-slot="{ Component }">
            <Transition name="fade-fast" mode="out-in">
              <component :is="Component" />
            </Transition>
          </router-view>
        </main>
      </div>

      <TransfersPanel v-if="fileStore.transfers.length > 0" />
    </div>

    <!-- Loading/Transition State -->
    <div v-else class="h-screen w-full flex items-center justify-center bg-canvas"></div>
  </div>
</template>

<style>
.fade-fast-enter-active,
.fade-fast-leave-active {
  transition: opacity 0.1s ease;
}
.fade-fast-enter-from,
.fade-fast-leave-to {
  opacity: 0;
}
</style>
