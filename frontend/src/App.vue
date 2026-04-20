<template>
  <div class="app">
    <div class="nav">
      <div class="nav-inner">
        <div class="nav-brand">
          <svg width="24" height="24" viewBox="0 0 28 28" fill="none" stroke="currentColor" stroke-width="1.6">
            <rect x="3" y="3" width="22" height="22" rx="5" />
            <path d="M8 12h12M8 16h8" stroke-linecap="round" />
            <circle cx="19" cy="16" r="1.5" fill="currentColor" stroke="none" />
          </svg>
          <span>Clode Storage</span>
        </div>
        <div class="nav-tabs">
          <router-link to="/files" class="tab" active-class="active">Файлы</router-link>
          <router-link to="/insights" class="tab" active-class="active">Аналитика</router-link>
        </div>
        <div class="nav-actions">
          <ThemeToggle />
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
import ThemeToggle from './components/ThemeToggle.vue';
</script>

<style scoped>
.app {
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 1.25rem 2rem;
}
.nav {
  position: sticky;
  top: 0;
  z-index: 100;
  background: var(--nav-bg);
  backdrop-filter: blur(12px);
  border-bottom: 1px solid var(--border-subtle);
  margin: 0 -1.25rem;
  padding: 0 1.25rem;
}
.nav-inner {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  height: 60px;
}
.nav-brand {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 700;
  color: var(--text);
}
.nav-tabs {
  display: flex;
  gap: 0.25rem;
  background: var(--bg-surface);
  border-radius: 40px;
  padding: 0.2rem;
}
.tab {
  padding: 0.35rem 1rem;
  text-decoration: none;
  color: var(--text-muted);
  font-weight: 500;
  border-radius: 32px;
}
.tab.active {
  background: var(--accent-dim);
  color: var(--accent);
}
.nav-actions {
  margin-left: auto;
}
.main {
  margin-top: 1.5rem;
}
.footer {
  margin-top: 2.5rem;
  padding-top: 1rem;
  border-top: 1px solid var(--border-subtle);
  display: flex;
  justify-content: space-between;
  font-size: 0.7rem;
  color: var(--text-faint);
}
</style>
