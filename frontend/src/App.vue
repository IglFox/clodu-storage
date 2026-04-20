<template>
  <div class="app">
    <div class="nav">
      <div class="nav-inner">
        <div class="nav-brand">
          <svg
            width="24"
            height="24"
            viewBox="0 0 28 28"
            fill="none"
            stroke="currentColor"
            stroke-width="1.6"
          >
            <rect x="3" y="3" width="22" height="22" rx="5" />
            <path d="M8 12h12M8 16h8" stroke-linecap="round" />
            <circle cx="19" cy="16" r="1.5" fill="currentColor" stroke="none" />
          </svg>
          <span>Clode Storage</span>
        </div>
        <div class="nav-tabs" v-if="authStore.isAuthenticated">
          <router-link to="/files" class="tab" active-class="active"
            >Файлы</router-link
          >
          <router-link to="/insights" class="tab" active-class="active"
            >Аналитика</router-link
          >
        </div>
        <div class="nav-actions">
          <ThemeToggle />
          <template v-if="authStore.isAuthenticated">
            <span class="user-name">{{ authStore.user?.name }}</span>
            <button class="btn-icon" @click="logout">Выйти</button>
          </template>
        </div>
      </div>
    </div>
    <main class="main">
      <router-view />
    </main>
    <footer class="footer">
      <span>Clode Storage — безопасное облако с шифрованием</span>
      <span>© 2026</span>
    </footer>
  </div>
</template>

<script setup>
import { useRouter } from "vue-router";
import { useAuthStore } from "./stores/auth";
import ThemeToggle from "./components/ThemeToggle.vue";

const authStore = useAuthStore();
const router = useRouter();

// При загрузке приложения проверим, есть ли сохранённый токен
authStore.checkAuth();

const logout = () => {
  authStore.logout();
  router.push("/login");
};
</script>

<style scoped>
/* ... ваши существующие стили ... */
.user-name {
  font-size: 0.85rem;
  color: var(--text-muted);
  background: var(--bg-surface);
  padding: 0.3rem 0.7rem;
  border-radius: 40px;
}
.btn-icon {
  padding: 0.3rem 0.7rem;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 40px;
  cursor: pointer;
  color: var(--text);
}
</style>
