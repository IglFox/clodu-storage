<script setup lang="ts">
import { ref, computed } from 'vue'
import { Mail, Lock, Eye, EyeOff, Cloud, CheckCircle } from 'lucide-vue-next'
import { API_MESSAGES } from '../config/apiConfig'

const emit = defineEmits<{
  switchToRegister: []
  loginSuccess: [email: string]
}>()

const email = ref('')
const password = ref('')
const rememberMe = ref(false)
const showPassword = ref(false)
const isLoading = ref(false)
const errorMessage = ref('')
const successMessage = ref('')

const isFormValid = computed(() => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailRegex.test(email.value) && password.value.length >= 6
})

async function handleLogin() {
  if (!isFormValid.value) {
    errorMessage.value = 'Пожалуйста, заполните все поля.'
    return
  }

  isLoading.value = true
  errorMessage.value = ''
  successMessage.value = ''

  try {
    // Simulate API call
    await new Promise((resolve) => setTimeout(resolve, 2000))

    // In a real application, you would call the API endpoint:
    // const response = await fetch(buildEndpointUrl(AUTH_ENDPOINTS.login), {
    //   method: 'POST',
    //   headers: DEFAULT_HEADERS,
    //   body: JSON.stringify({
    //     email: email.value,
    //     password: password.value,
    //   }),
    // })

    successMessage.value = 'Вход выполнен успешно! Переводим вас в приложение...'
    
    // Save login preference if "Remember me" is checked
    if (rememberMe.value) {
      localStorage.setItem('rememberedEmail', email.value)
    }

    // Emit success event
    setTimeout(() => {
      emit('loginSuccess', email.value)
    }, 1500)
  } catch (error) {
    errorMessage.value = API_MESSAGES.error.serverError
  } finally {
    isLoading.value = false
  }
}

function handleForgotPassword() {
  // This would typically navigate to a password reset page
  alert('Функция восстановления пароля будет реализована в API')
}

function switchToRegister() {
  email.value = ''
  password.value = ''
  errorMessage.value = ''
  successMessage.value = ''
  emit('switchToRegister')
}
</script>

<template>
  <div class="min-h-screen bg-gradient-to-br from-slate-50 to-slate-100 dark:from-slate-950 dark:to-slate-900 flex items-center justify-center p-4">
    <!-- Background decorations -->
    <div class="fixed inset-0 pointer-events-none overflow-hidden">
      <div
        class="absolute top-0 left-0 w-96 h-96 bg-primary-500/10 rounded-full blur-3xl opacity-50 dark:opacity-20"
      />
      <div
        class="absolute bottom-0 right-0 w-96 h-96 bg-accent-500/10 rounded-full blur-3xl opacity-50 dark:opacity-20"
      />
    </div>

    <div class="relative w-full max-w-md">
      <div class="bg-white dark:bg-slate-800 rounded-lg p-8 shadow-card-lg animate-fade-in">
        <!-- Header -->
        <div class="text-center mb-8">
          <div class="flex items-center justify-center gap-2 mb-4">
            <div class="w-10 h-10 bg-gradient-to-br from-primary-500 to-accent-600 rounded-lg flex items-center justify-center">
              <Cloud class="w-6 h-6 text-white" />
            </div>
            <h1 class="text-2xl font-bold bg-gradient-to-r from-primary-600 to-accent-600 bg-clip-text text-transparent">
              Nebula Cloud
            </h1>
          </div>
          <h2 class="text-xl font-bold mb-2">Добро пожаловать!</h2>
          <p class="text-sm text-slate-600 dark:text-slate-400">Введите ваши учётные данные для входа</p>
        </div>

        <!-- Success Message -->
        <div
          v-if="successMessage"
          class="mb-4 p-4 bg-emerald-50 dark:bg-emerald-900/30 border border-emerald-200 dark:border-emerald-800 rounded-lg text-sm text-emerald-700 dark:text-emerald-300 flex items-center gap-2"
        >
          <CheckCircle class="w-5 h-5 flex-shrink-0" />
          {{ successMessage }}
        </div>

        <!-- Error Message -->
        <div
          v-if="errorMessage"
          class="mb-4 p-4 bg-red-50 dark:bg-red-900/30 border border-red-200 dark:border-red-800 rounded-lg text-sm text-red-700 dark:text-red-300"
        >
          {{ errorMessage }}
        </div>

        <!-- Form -->
        <form @submit.prevent="handleLogin" class="space-y-4">
          <!-- Email -->
          <div>
            <label class="block text-sm font-medium mb-2">Электронная почта</label>
            <div class="relative">
              <Mail class="absolute left-3 top-3 w-5 h-5 text-slate-400 pointer-events-none" />
              <input
                v-model="email"
                type="email"
                placeholder="your@email.com"
                class="w-full pl-10 pr-4 py-2.5 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                required
              />
            </div>
          </div>

          <!-- Password -->
          <div>
            <div class="flex items-center justify-between mb-2">
              <label class="text-sm font-medium">Пароль</label>
              <button
                type="button"
                @click="handleForgotPassword"
                class="text-xs text-primary-600 dark:text-primary-400 hover:underline"
              >
                Забыли пароль?
              </button>
            </div>
            <div class="relative">
              <Lock class="absolute left-3 top-3 w-5 h-5 text-slate-400 pointer-events-none" />
              <input
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                placeholder="••••••••"
                class="w-full pl-10 pr-10 py-2.5 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                required
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-3 top-3 text-slate-400 hover:text-slate-600 dark:hover:text-slate-300"
              >
                <Eye v-if="!showPassword" class="w-5 h-5" />
                <EyeOff v-else class="w-5 h-5" />
              </button>
            </div>
          </div>

          <!-- Remember Me -->
          <div class="flex items-center gap-3">
            <input
              v-model="rememberMe"
              type="checkbox"
              id="remember"
              class="w-4 h-4 border-slate-300 rounded cursor-pointer accent-primary-600"
            />
            <label for="remember" class="text-sm text-slate-600 dark:text-slate-400"> Запомнить меня </label>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="!isFormValid || isLoading"
            class="w-full mt-6 px-4 py-3 bg-gradient-to-r from-primary-600 to-accent-600 text-white rounded-lg font-medium hover:from-primary-700 hover:to-accent-700 disabled:from-slate-300 disabled:to-slate-400 disabled:cursor-not-allowed transition-all"
          >
            <span v-if="!isLoading">Войти в аккаунт</span>
            <span v-else>Пожалуйста, подождите...</span>
          </button>
        </form>

        <!-- Register Link -->
        <div class="mt-6 text-center text-sm text-slate-600 dark:text-slate-400">
          Нет учётной записи?
          <button
            @click="switchToRegister"
            class="text-primary-600 dark:text-primary-400 hover:underline font-medium ml-1"
          >
            Зарегистрироваться
          </button>
        </div>

        <!-- Demo Info -->
        <div class="mt-6 p-4 bg-slate-50 dark:bg-slate-700/50 rounded-lg text-xs text-slate-600 dark:text-slate-400 border border-slate-200 dark:border-slate-700">
          <p class="font-medium mb-1">Демо учётные данные:</p>
          <p>Email: <code class="bg-slate-100 dark:bg-slate-900 px-1 rounded">demo@nebulacloud.com</code></p>
          <p>Password: <code class="bg-slate-100 dark:bg-slate-900 px-1 rounded">Demo@12345</code></p>
        </div>
      </div>
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

.animate-fade-in {
  animation: fade-in 0.3s ease-out;
}
</style>
