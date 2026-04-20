<template>
  <div class="auth-container">
    <div class="auth-card">
      <h2>Регистрация</h2>
      <form @submit.prevent="handleRegister">
        <div class="field">
          <label>Имя</label>
          <input type="text" v-model="name" required />
        </div>
        <div class="field">
          <label>Email</label>
          <input type="email" v-model="email" required />
        </div>
        <div class="field">
          <label>Пароль</label>
          <input type="password" v-model="password" required />
        </div>
        <button type="submit" :disabled="loading">Зарегистрироваться</button>
        <p class="auth-switch">
          Уже есть аккаунт? <router-link to="/login">Войти</router-link>
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

const name = ref("");
const email = ref("");
const password = ref("");
const loading = ref(false);
const error = ref("");
const router = useRouter();
const authStore = useAuthStore();

const handleRegister = async () => {
  loading.value = true;
  error.value = "";
  try {
    await authStore.register(email.value, password.value, name.value);
    router.push("/files");
  } catch (err) {
    error.value = err.message || "Ошибка регистрации";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
/* те же стили, что в LoginView */
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
