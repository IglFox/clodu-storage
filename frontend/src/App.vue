<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { Cloud, Sun, Moon, Upload, Plus, Search, X, FolderPlus, FileText, LogOut } from 'lucide-vue-next'
import FilesTab from './components/FilesTab.vue'
import AnalyticsTab from './components/AnalyticsTab.vue'
import LoginPage from './components/LoginPage.vue'
import RegisterPage from './components/RegisterPage.vue'
import { useTheme } from './composables/useTheme'

const { isDark, toggleTheme } = useTheme()
const activeTab = ref<'files' | 'analytics'>('files')

// Authentication state
type PageType = 'login' | 'register' | 'app'
const currentPage = ref<PageType>('login')
const userEmail = ref<string>('')

// Check if user is already logged in (from localStorage)
onMounted(() => {
  const savedEmail = localStorage.getItem('userEmail')
  if (savedEmail) {
    userEmail.value = savedEmail
    currentPage.value = 'app'
  }
})

// Handle login
function handleLogin(email: string) {
  userEmail.value = email
  localStorage.setItem('userEmail', email)
  currentPage.value = 'app'
}

// Handle logout
function handleLogout() {
  userEmail.value = ''
  localStorage.removeItem('userEmail')
  currentPage.value = 'login'
  activeTab.value = 'files'
}

// Switch to register page
function switchToRegister() {
  currentPage.value = 'register'
}

// Switch to login page
function switchToLogin() {
  currentPage.value = 'login'
}
</script>

<template>
  <div :class="isDark ? 'dark' : ''" class="min-h-screen transition-colors duration-300">
    <!-- Background gradients -->
    <div class="fixed inset-0 pointer-events-none overflow-hidden">
      <div
        class="absolute top-0 left-0 w-96 h-96 bg-primary-500/10 rounded-full blur-3xl opacity-50 dark:opacity-20"
      />
      <div
        class="absolute bottom-0 right-0 w-96 h-96 bg-accent-500/10 rounded-full blur-3xl opacity-50 dark:opacity-20"
      />
    </div>

    <!-- Login Page -->
    <LoginPage v-if="currentPage === 'login'" @switch-to-register="switchToRegister" @login-success="handleLogin" />

    <!-- Register Page -->
    <RegisterPage v-else-if="currentPage === 'register'" @switch-to-login="switchToLogin" />

    <!-- Main App -->
    <div v-else class="relative bg-white dark:bg-slate-950 text-slate-900 dark:text-slate-100 transition-colors duration-300">
      <!-- Header -->
      <header
        class="border-b border-slate-200 dark:border-slate-800 sticky top-0 z-40 bg-white/95 dark:bg-slate-950/95 backdrop-blur-sm transition-colors duration-300"
      >
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between">
          <div class="flex items-center gap-3">
            <div
              class="w-10 h-10 bg-gradient-to-br from-primary-500 to-accent-600 rounded-lg flex items-center justify-center shadow-lg"
            >
              <Cloud class="w-6 h-6 text-white" />
            </div>
            <div>
              <h1 class="text-xl font-bold bg-gradient-to-r from-primary-600 to-accent-600 bg-clip-text text-transparent">
                Nebula Cloud
              </h1>
              <p class="text-xs text-slate-500 dark:text-slate-400">Cloud Storage Pro</p>
            </div>
          </div>

          <div class="flex items-center gap-3">
            <button
              @click="toggleTheme"
              class="p-2 rounded-lg bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 dark:hover:bg-slate-700 transition-colors duration-200"
              :aria-label="`Switch to ${isDark ? 'light' : 'dark'} mode`"
            >
              <Sun v-if="isDark" class="w-5 h-5 text-amber-500" />
              <Moon v-else class="w-5 h-5 text-slate-600" />
            </button>

            <!-- User Email & Logout -->
            <div class="flex items-center gap-3 pl-3 border-l border-slate-200 dark:border-slate-800">
              <div class="text-right">
                <p class="text-xs text-slate-600 dark:text-slate-400">Вы вошли как:</p>
                <p class="text-sm font-medium truncate">{{ userEmail }}</p>
              </div>
              <button
                @click="handleLogout"
                class="p-2 rounded-lg bg-red-100 dark:bg-red-900/30 hover:bg-red-200 dark:hover:bg-red-900/50 transition-colors duration-200 text-red-600 dark:text-red-400"
                title="Выход"
              >
                <LogOut class="w-5 h-5" />
              </button>
            </div>
          </div>
        </div>
      </header>

      <!-- Main Content -->
      <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <!-- Tab Navigation -->
        <div class="flex gap-4 mb-8 border-b border-slate-200 dark:border-slate-800">
          <button
            @click="activeTab = 'files'"
            :class="[
              'px-4 py-3 font-medium text-sm transition-all duration-200 relative',
              activeTab === 'files'
                ? 'text-primary-600 dark:text-primary-400'
                : 'text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-200',
            ]"
          >
            📁 Files
            <span
              v-if="activeTab === 'files'"
              class="absolute bottom-0 left-0 right-0 h-0.5 bg-gradient-to-r from-primary-600 to-accent-600"
            />
          </button>
          <button
            @click="activeTab = 'analytics'"
            :class="[
              'px-4 py-3 font-medium text-sm transition-all duration-200 relative',
              activeTab === 'analytics'
                ? 'text-primary-600 dark:text-primary-400'
                : 'text-slate-600 dark:text-slate-400 hover:text-slate-900 dark:hover:text-slate-200',
            ]"
          >
            📊 Analytics
            <span
              v-if="activeTab === 'analytics'"
              class="absolute bottom-0 left-0 right-0 h-0.5 bg-gradient-to-r from-primary-600 to-accent-600"
            />
          </button>
        </div>

        <!-- Tab Content -->
        <div class="animate-fade-in">
          <FilesTab v-show="activeTab === 'files'" />
          <AnalyticsTab v-show="activeTab === 'analytics'" />
        </div>
      </main>

      <!-- Footer -->
      <footer
        class="border-t border-slate-200 dark:border-slate-800 mt-16 bg-slate-50 dark:bg-slate-900/50 transition-colors duration-300"
      >
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div class="grid grid-cols-1 md:grid-cols-3 gap-8 mb-8">
            <div>
              <h3 class="font-semibold mb-2">About</h3>
              <p class="text-sm text-slate-600 dark:text-slate-400">
                Nebula Cloud is a modern cloud storage solution with advanced file management and analytics.
              </p>
            </div>
            <div>
              <h3 class="font-semibold mb-2">Demo Data</h3>
              <p class="text-sm text-slate-600 dark:text-slate-400">
                All data shown is for demonstration purposes. Integration with a real backend is required for production use.
              </p>
            </div>
            <div>
              <h3 class="font-semibold mb-2">Status</h3>
              <p class="text-sm text-slate-600 dark:text-slate-400">
                🟢 All features operational • Responsive design • Dark mode enabled
              </p>
            </div>
          </div>
          <div class="border-t border-slate-200 dark:border-slate-800 pt-6 text-center text-sm text-slate-500 dark:text-slate-400">
            <p>© 2024 Nebula Cloud. All rights reserved. | Created with ❤️ for modern cloud storage</p>
          </div>
        </div>
      </footer>
    </div>
  </div>
</template>

<style scoped>
@keyframes fade-in {
  from {
    opacity: 0;
    transform: translateY(4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slide-up {
  from {
    opacity: 0;
    transform: translateY(8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-fade-in {
  animation: fade-in 0.3s ease-out;
}

.animate-slide-up {
  animation: slide-up 0.3s ease-out;
}
</style>
