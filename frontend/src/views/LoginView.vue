<template>
  <div class="auth-container">
    <div class="auth-card">
      <h2>Вход в Clode Storage</h2>
      <form @submit.prevent="handleLogin">
        <div class="field">
          <label>Email</label>
          <input type="email" v-model="email" required autofocus />
        </div>
        <div class="field">
          <label>Пароль</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit" :disabled="loading">Войти</button>
        <p class="auth-switch">
          Нет аккаунта?
          <router-link to="/register">Зарегистрироваться</router-link>
        </p>
        <div v-if="error" class="error">{{ error }}</div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";

const email = ref("");
const password = ref("");
const loading = ref(false);
const error = ref("");
const router = useRouter();
const authStore = useAuthStore();

const handleLogin = async () => {
  loading.value = true;
  error.value = "";
  try {
    await authStore.login(email.value, password.value);
    router.push("/files");
  } catch (err) {
    error.value = err.message || "Ошибка входа";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
.auth-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 70vh;
}
.auth-card {
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 2rem;
  width: 360px;
}
.auth-card h2 {
  margin-bottom: 1.5rem;
  text-align: center;
}
.field {
  margin-bottom: 1rem;
}
.field label {
  display: block;
  font-size: 0.8rem;
  margin-bottom: 0.3rem;
  color: var(--text-muted);
}
.field input {
  width: 100%;
  padding: 0.6rem;
  background: var(--bg-surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-sm);
  color: var(--text);
  font-size: 0.9rem;
}
button {
  width: 100%;
  padding: 0.6rem;
  background: var(--accent);
  border: none;
  border-radius: var(--radius-sm);
  color: white;
  font-weight: 600;
  cursor: pointer;
  margin-top: 0.5rem;
}
button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.auth-switch {
  text-align: center;
  margin-top: 1rem;
  font-size: 0.8rem;
}
.auth-switch a {
  color: var(--accent);
  text-decoration: none;
}
.error {
  background: var(--accent-red-dim);
  color: var(--accent-red);
  padding: 0.5rem;
  border-radius: var(--radius-sm);
  margin-top: 1rem;
  text-align: center;
  font-size: 0.8rem;
}
</style>
