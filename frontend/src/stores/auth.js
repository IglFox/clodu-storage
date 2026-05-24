import { defineStore } from 'pinia';
import { ref } from 'vue';
import { api } from '../utils/api';

export const useAuthStore = defineStore('auth', () => {
  const currentUser = ref(null);
  const authError = ref('');
  const authSuccess = ref('');
  const isLoading = ref(false);

  const processUser = (user) => {
    const emojis = ['👤', '🛡️', '⚡', '🔒', '📦', '🏢', '🤖', '🌐', '🛠️', '💼'];
    const randomEmoji = emojis[Math.floor(Math.random() * emojis.length)];
    
    return {
      ...user,
      name: user.name || user.username || user.email?.split('@')[0] || 'Unknown User',
      avatar: randomEmoji,
      role: user.role || 'Storage Contributor',
      registrationDate: user.registrationDate || new Date().toISOString().split('T')[0]
    };
  };

  // Register a new user
  async function register(name, email, password) {
    authError.value = '';
    authSuccess.value = '';
    isLoading.value = true;
    
    console.log('Attempting registration for:', email);
    try {
      await api.post('/api/Auth/register', { username: name, email, password });
      authSuccess.value = 'Account created successfully! You can now log in.';
      console.log('Registration successful');
      return true;
    } catch (error) {
      console.error('Registration failed:', error);
      authError.value = error.message;
      return false;
    } finally {
      isLoading.value = false;
    }
  }

  // Login verification
  async function login(email, password) {
    authError.value = '';
    authSuccess.value = '';
    isLoading.value = true;
    
    try {
      const response = await api.post('/api/Auth/login', { email, password });
      
      if (response.token) {
        api.setToken(response.token);
      }
      
      if (response.user) {
        currentUser.value = processUser(response.user);
        localStorage.setItem('dcs_session', JSON.stringify(currentUser.value));
      } else {
        await fetchCurrentUser();
      }
      
      return true;
    } catch (error) {
      authError.value = error.message;
      return false;
    } finally {
      isLoading.value = false;
    }
  }

  async function fetchCurrentUser() {
    try {
      const user = await api.get('/api/Auth/me');
      currentUser.value = processUser(user);
      localStorage.setItem('dcs_session', JSON.stringify(currentUser.value));
    } catch (error) {
      logout();
      throw error;
    }
  }

  // Load session from storage and verify it
  async function loadSession() {
    const token = localStorage.getItem('dcs_auth_token');
    if (!token) return;

    try {
      await fetchCurrentUser();
    } catch (error) {
      console.error('Session verification failed:', error);
    }
  }

  // Logout
  async function logout() {
    try {
      await api.post('/api/Auth/logout').catch(() => {}); // Best effort logout on server
    } finally {
      currentUser.value = null;
      api.clearToken();
      try {
        localStorage.removeItem('dcs_session');
        sessionStorage.removeItem('dcs_master_key');
      } catch (e) {
        console.error(e);
      }
    }
  }

  return {
    currentUser,
    authError,
    authSuccess,
    isLoading,
    register,
    login,
    logout,
    loadSession
  };
});
