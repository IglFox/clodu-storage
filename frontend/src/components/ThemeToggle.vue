<template>
  <button class="theme-toggle" @click="toggleTheme">
    {{ isLight ? '☀️ Светлая' : '🌙 Тёмная' }}
  </button>
</template>

<script setup>
import { useTheme } from '../composables/useTheme';
const { isLight, toggleTheme } = useTheme();
</script>

<style scoped>
.theme-toggle {
  display: flex;
  align-items: center;
  gap: 0.35rem;
  padding: 0.3rem 0.7rem;
  background: none;
  border: 1px solid var(--border);
  border-radius: 40px;
  color: var(--text-muted);
  font-size: 0.75rem;
  cursor: pointer;
}
.theme-toggle:hover {
  border-color: var(--text-faint);
  color: var(--text);
}
</style>
