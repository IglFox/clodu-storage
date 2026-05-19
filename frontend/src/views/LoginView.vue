<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useApi } from '../composables/useApi'

const router = useRouter()
const { loading, error, execute } = useApi()

const mode = ref<'login' | 'register'>('login')
const email = ref('')
const password = ref('')
const passwordConfirm = ref('')

const handleLogin = async () => {
  if (!email.value || !password.value) return

  const authPath = import.meta.env.VITE_AUTH_PATH || '/auth/login'
  const response = await execute(authPath, {
    method: 'POST',
    body: { email: email.value, password: password.value },
  })

  console.log('Login response:', response)

  if (response) {
    // Проверяем разные варианты где может быть токен
    const token =
      response.token || response.access_token || response.accessToken || response.data?.token

    if (token) {
      localStorage.setItem('authToken', token)
      console.log('Token saved, redirecting...')
      router.push('/main')
    } else if (response.success || response.ok) {
      // Если просто успешный ответ без токена
      localStorage.setItem('authToken', 'user-session')
      console.log('Auth successful, redirecting...')
      router.push('/main')
    }
  }
}

const handleRegister = async () => {
  if (!email.value || !password.value || !passwordConfirm.value) return
  if (password.value !== passwordConfirm.value) {
    return
  }

  const registerPath = import.meta.env.VITE_REGISTER_PATH || '/auth/register'
  const response = await execute(registerPath, {
    method: 'POST',
    body: { email: email.value, password: password.value },
  })

  console.log('Register response:', response)

  if (response) {
    // Проверяем разные варианты где может быть токен
    const token =
      response.token || response.access_token || response.accessToken || response.data?.token

    if (token) {
      localStorage.setItem('authToken', token)
      console.log('Token saved after registration, redirecting...')
      router.push('/main')
    } else if (response.success || response.ok) {
      // Если просто успешный ответ, считаем что регистрация прошла
      localStorage.setItem('authToken', 'user-session')
      console.log('Registration successful, redirecting...')
      router.push('/main')
    }
  }
}

const toggleMode = () => {
  mode.value = mode.value === 'login' ? 'register' : 'login'
  password.value = ''
  passwordConfirm.value = ''
}

const appName = import.meta.env.VITE_APP_NAME || 'CloudBox'
</script>

<template>
  <div class="min-h-screen flex items-center justify-center bg-slate-50 px-4">
    <div class="w-full max-w-sm">
      <h1 class="text-2xl font-semibold text-gray-900 mb-8 text-center">{{ appName }}</h1>

      <!-- Login Form -->
      <form v-if="mode === 'login'" @submit.prevent="handleLogin" class="space-y-4">
        <div>
          <input
            v-model="email"
            type="email"
            placeholder="Email or username"
            required
            class="w-full px-4 py-2 border border-gray-200 rounded text-sm text-black focus:outline-none focus:border-blue-600 focus:ring-1 focus:ring-blue-600"
          />
        </div>

        <div>
          <input
            v-model="password"
            type="password"
            placeholder="Password"
            required
            class="w-full px-4 py-2 border border-gray-200 rounded text-sm text-black focus:outline-none focus:border-blue-600 focus:ring-1 focus:ring-blue-600"
          />
        </div>

        <button
          :disabled="loading"
          class="w-full py-2 px-4 bg-blue-600 text-white font-medium rounded text-sm hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
        >
          {{ loading ? 'Logging in...' : 'Log in' }}
        </button>

        <div v-if="error" class="text-sm text-red-600 text-center mt-2">
          {{ error }}
        </div>

        <div class="text-center text-sm text-gray-600">
          Don't have an account?
          <button
            type="button"
            @click="toggleMode"
            class="text-blue-600 hover:text-blue-700 font-medium"
          >
            Sign up
          </button>
        </div>
      </form>

      <!-- Register Form -->
      <form v-else @submit.prevent="handleRegister" class="space-y-4">
        <div>
          <input
            v-model="email"
            type="email"
            placeholder="Email"
            required
            class="w-full px-4 py-2 border border-gray-200 rounded text-sm text-black focus:outline-none focus:border-blue-600 focus:ring-1 focus:ring-blue-600"
          />
        </div>

        <div>
          <input
            v-model="password"
            type="password"
            placeholder="Password"
            required
            class="w-full px-4 py-2 border border-gray-200 rounded text-sm text-black focus:outline-none focus:border-blue-600 focus:ring-1 focus:ring-blue-600"
          />
        </div>

        <div>
          <input
            v-model="passwordConfirm"
            type="password"
            placeholder="Confirm password"
            required
            class="w-full px-4 py-2 border border-gray-200 rounded text-sm text-black focus:outline-none focus:border-blue-600 focus:ring-1 focus:ring-blue-600"
          />
        </div>

        <div
          v-if="password && passwordConfirm && password !== passwordConfirm"
          class="text-sm text-red-600 text-center"
        >
          Passwords do not match
        </div>

        <button
          :disabled="loading || (password && passwordConfirm && password !== passwordConfirm)"
          class="w-full py-2 px-4 bg-blue-600 text-white font-medium rounded text-sm hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed transition-colors"
        >
          {{ loading ? 'Signing up...' : 'Sign up' }}
        </button>

        <div v-if="error" class="text-sm text-red-600 text-center mt-2">
          {{ error }}
        </div>

        <div class="text-center text-sm text-gray-600">
          Already have an account?
          <button
            type="button"
            @click="toggleMode"
            class="text-blue-600 hover:text-blue-700 font-medium"
          >
            Log in
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
