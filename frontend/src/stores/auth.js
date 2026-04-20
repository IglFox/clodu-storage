import { defineStore } from "pinia";

const API_BASE = "http://localhost:8081";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: null,
    token: localStorage.getItem("token") || null,
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
  },

  actions: {
    // Регистрация: создаём пользователя, затем автоматически логинимся
    async register(email, password, name) {
      // 1. Регистрация
      const registerRes = await fetch(`${API_BASE}/auth/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const registerData = await registerRes.json();

      if (!registerRes.ok) {
        if (registerData.error === "User already exists") {
          throw new Error("Пользователь с таким email уже существует");
        }
        throw new Error(registerData.error || "Ошибка регистрации");
      }

      // 2. После успешной регистрации – логин (получаем токен и профиль)
      await this.login(email, password);
    },

    // Вход: получаем токен, затем загружаем профиль
    async login(email, password) {
      // 1. Запрос на получение токена
      const loginRes = await fetch(`${API_BASE}/auth/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      const loginData = await loginRes.json();

      if (!loginRes.ok) {
        if (loginData.error === "Invalid email or password") {
          throw new Error("Неверный email или пароль");
        }
        throw new Error(loginData.error || "Ошибка входа");
      }

      // Сохраняем токен
      this.token = loginData.access_token;
      localStorage.setItem("token", this.token);

      // 2. Загружаем профиль пользователя по токену
      await this.fetchUserProfile();
    },

    // Получение профиля с текущим токеном
    async fetchUserProfile() {
      if (!this.token) {
        throw new Error("Нет токена авторизации");
      }

      const profileRes = await fetch(`${API_BASE}/auth/me`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.token}`,
        },
      });

      if (!profileRes.ok) {
        // Если токен невалиден – разлогиниваемся
        if (profileRes.status === 401) {
          this.logout();
          throw new Error("Сессия истекла, войдите снова");
        }
        throw new Error("Ошибка получения профиля");
      }

      const profileData = await profileRes.json();
      this.user = profileData; // { id, email }
      localStorage.setItem("user", JSON.stringify(this.user));
    },

    // Выход из системы
    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },

    // Проверка сохранённой сессии при загрузке приложения
    async checkAuth() {
      const token = localStorage.getItem("token");
      if (!token) return;

      this.token = token;
      try {
        await this.fetchUserProfile();
      } catch (error) {
        // Если профиль не загрузился (невалидный токен) – очищаем состояние
        this.logout();
        console.warn("Автоматический выход: недействительный токен");
      }
    },
  },
});
