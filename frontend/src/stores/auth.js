import { defineStore } from "pinia";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: null,
    token: localStorage.getItem("token") || null,
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    // Мок-логин (позже замените на реальный API-вызов)
    async login(email, password) {
      // Имитация задержки сети
      await new Promise((resolve) => setTimeout(resolve, 500));

      // Простейшая валидация: любой email + любой пароль (кроме пустого)
      if (!email || !password) throw new Error("Заполните поля");

      // Создаём фиктивный токен и пользователя
      const fakeToken = "fake-jwt-token-" + Date.now();
      const fakeUser = { email, name: email.split("@")[0] };

      this.token = fakeToken;
      this.user = fakeUser;
      localStorage.setItem("token", fakeToken);
      localStorage.setItem("user", JSON.stringify(fakeUser));

      return { user: fakeUser };
    },

    // Мок-регистрация
    async register(email, password, name) {
      await new Promise((resolve) => setTimeout(resolve, 500));
      if (!email || !password || !name) throw new Error("Заполните все поля");
      // Можно добавить проверку, что пользователь не существует (в моке всегда успех)
      const fakeToken = "fake-jwt-token-" + Date.now();
      const fakeUser = { email, name };
      this.token = fakeToken;
      this.user = fakeUser;
      localStorage.setItem("token", fakeToken);
      localStorage.setItem("user", JSON.stringify(fakeUser));
      return { user: fakeUser };
    },

    logout() {
      this.token = null;
      this.user = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },

    // Восстановление сессии при загрузке приложения
    checkAuth() {
      const token = localStorage.getItem("token");
      const user = localStorage.getItem("user");
      if (token && user) {
        this.token = token;
        this.user = JSON.parse(user);
      }
    },
  },
});
