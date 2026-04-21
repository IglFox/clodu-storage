<script setup lang="ts">
import { ref, computed } from 'vue'
import { Mail, Lock, User, Eye, EyeOff, ArrowRight, Cloud } from 'lucide-vue-next'
import { REGISTRATION_CONFIG, API_MESSAGES } from '../config/apiConfig'

const emit = defineEmits<{
  switchToLogin: []
}>()

const formData = ref({
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  confirmPassword: '',
  agreeToTerms: false,
})

const showPassword = ref(false)
const showConfirmPassword = ref(false)
const isLoading = ref(false)
const registrationSuccess = ref(false)
const errorMessage = ref('')

// Password validation
const passwordValidation = computed(() => {
  const pwd = formData.value.password
  return {
    hasMinLength: pwd.length >= REGISTRATION_CONFIG.minPasswordLength,
    hasUppercase: /[A-Z]/.test(pwd),
    hasNumber: /[0-9]/.test(pwd),
    hasSpecialChar: /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(pwd),
  }
})

const isPasswordValid = computed(() => {
  const validation = passwordValidation.value
  return (
    validation.hasMinLength &&
    validation.hasUppercase &&
    validation.hasNumber &&
    validation.hasSpecialChar &&
    formData.value.password === formData.value.confirmPassword &&
    formData.value.password.length > 0
  )
})

// Email validation
const isEmailValid = computed(() => {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
  return emailRegex.test(formData.value.email)
})

// Username validation
const isUsernameValid = computed(() => {
  const firstName = formData.value.firstName.trim()
  return firstName.length >= REGISTRATION_CONFIG.minUsernameLength && firstName.length <= REGISTRATION_CONFIG.maxUsernameLength
})

// Overall form validation
const isFormValid = computed(
  () => isEmailValid.value && isPasswordValid.value && isUsernameValid.value && formData.value.agreeToTerms
)

// Handle registration
async function handleRegister() {
  if (!isFormValid.value) {
    errorMessage.value = 'Пожалуйста, заполните все поля корректно.'
    return
  }

  isLoading.value = true
  errorMessage.value = ''

  try {
    // Simulate API call
    await new Promise((resolve) => setTimeout(resolve, 2000))

    // In a real application, you would call the API endpoint:
    // const response = await fetch(buildEndpointUrl(AUTH_ENDPOINTS.register), {
    //   method: 'POST',
    //   headers: DEFAULT_HEADERS,
    //   body: JSON.stringify({
    //     firstName: formData.value.firstName,
    //     lastName: formData.value.lastName,
    //     email: formData.value.email,
    //     password: formData.value.password,
    //   }),
    // })

    registrationSuccess.value = true
  } catch (error) {
    errorMessage.value = API_MESSAGES.error.serverError
  } finally {
    isLoading.value = false
  }
}

function resetForm() {
  formData.value = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    agreeToTerms: false,
  }
  registrationSuccess.value = false
  errorMessage.value = ''
}

function switchToLogin() {
  resetForm()
  emit('switchToLogin')
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
      <!-- Success State -->
      <div v-if="registrationSuccess" class="bg-white dark:bg-slate-800 rounded-lg p-8 shadow-card-lg text-center animate-fade-in">
        <div class="w-16 h-16 bg-emerald-100 dark:bg-emerald-900 rounded-full flex items-center justify-center mx-auto mb-4">
          <div class="text-3xl">✓</div>
        </div>
        <h2 class="text-2xl font-bold mb-2">Регистрация успешна!</h2>
        <p class="text-slate-600 dark:text-slate-400 mb-6">
          Проверьте вашу электронную почту для подтверждения учётной записи. Ссылка действительна 24 часа.
        </p>
        <p class="text-sm text-slate-500 dark:text-slate-500 mb-6">
          Письмо отправлено на: <strong>{{ formData.email }}</strong>
        </p>
        <button
          @click="switchToLogin"
          class="w-full px-4 py-3 bg-primary-600 dark:bg-primary-500 text-white rounded-lg font-medium hover:bg-primary-700 dark:hover:bg-primary-600 transition-colors flex items-center justify-center gap-2"
        >
          Перейти к входу
          <ArrowRight class="w-4 h-4" />
        </button>
      </div>

      <!-- Registration Form -->
      <div v-else class="bg-white dark:bg-slate-800 rounded-lg p-8 shadow-card-lg animate-fade-in">
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
          <h2 class="text-xl font-bold mb-2">Создайте учётную запись</h2>
          <p class="text-sm text-slate-600 dark:text-slate-400">Начните использовать облачное хранилище прямо сейчас</p>
        </div>

        <!-- Error Message -->
        <div
          v-if="errorMessage"
          class="mb-4 p-4 bg-red-50 dark:bg-red-900/30 border border-red-200 dark:border-red-800 rounded-lg text-sm text-red-700 dark:text-red-300"
        >
          {{ errorMessage }}
        </div>

        <!-- Form -->
        <form @submit.prevent="handleRegister" class="space-y-4">
          <!-- First Name -->
          <div>
            <label class="block text-sm font-medium mb-2">Имя</label>
            <div class="relative">
              <User class="absolute left-3 top-3 w-5 h-5 text-slate-400 pointer-events-none" />
              <input
                v-model="formData.firstName"
                type="text"
                placeholder="Иван"
                class="w-full pl-10 pr-4 py-2.5 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                required
              />
            </div>
            <p v-if="formData.firstName && !isUsernameValid" class="text-xs text-red-600 dark:text-red-400 mt-1">
              Имя должно быть от 3 до 30 символов
            </p>
          </div>

          <!-- Last Name -->
          <div>
            <label class="block text-sm font-medium mb-2">Фамилия</label>
            <input
              v-model="formData.lastName"
              type="text"
              placeholder="Иванов"
              class="w-full px-4 py-2.5 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
            />
          </div>

          <!-- Email -->
          <div>
            <label class="block text-sm font-medium mb-2">Электронная почта</label>
            <div class="relative">
              <Mail class="absolute left-3 top-3 w-5 h-5 text-slate-400 pointer-events-none" />
              <input
                v-model="formData.email"
                type="email"
                placeholder="your@email.com"
                class="w-full pl-10 pr-4 py-2.5 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                required
              />
            </div>
            <p v-if="formData.email && !isEmailValid" class="text-xs text-red-600 dark:text-red-400 mt-1">
              Пожалуйста, введите корректный адрес электронной почты
            </p>
          </div>

          <!-- Password -->
          <div>
            <label class="block text-sm font-medium mb-2">Пароль</label>
            <div class="relative">
              <Lock class="absolute left-3 top-3 w-5 h-5 text-slate-400 pointer-events-none" />
              <input
                v-model="formData.password"
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

            <!-- Password Requirements -->
            <div v-if="formData.password" class="mt-3 space-y-2">
              <div
                :class="[
                  'flex items-center gap-2 text-xs',
                  passwordValidation.hasMinLength
                    ? 'text-emerald-600 dark:text-emerald-400'
                    : 'text-slate-400 dark:text-slate-500',
                ]"
              >
                <span v-if="passwordValidation.hasMinLength">✓</span>
                <span v-else>✗</span>
                Минимум {{ REGISTRATION_CONFIG.minPasswordLength }} символов
              </div>
              <div
                :class="[
                  'flex items-center gap-2 text-xs',
                  passwordValidation.hasUppercase
                    ? 'text-emerald-600 dark:text-emerald-400'
                    : 'text-slate-400 dark:text-slate-500',
                ]"
              >
                <span v-if="passwordValidation.hasUppercase">✓</span>
                <span v-else>✗</span>
                Хотя бы одна заглавная буква (A-Z)
              </div>
              <div
                :class="[
                  'flex items-center gap-2 text-xs',
                  passwordValidation.hasNumber
                    ? 'text-emerald-600 dark:text-emerald-400'
                    : 'text-slate-400 dark:text-slate-500',
                ]"
              >
                <span v-if="passwordValidation.hasNumber">✓</span>
                <span v-else>✗</span>
                Хотя бы одна цифра (0-9)
              </div>
              <div
                :class="[
                  'flex items-center gap-2 text-xs',
                  passwordValidation.hasSpecialChar
                    ? 'text-emerald-600 dark:text-emerald-400'
                    : 'text-slate-400 dark:text-slate-500',
                ]"
              >
                <span v-if="passwordValidation.hasSpecialChar">✓</span>
                <span v-else>✗</span>
                Хотя бы один спецсимвол (!@#$%^&*)
              </div>
            </div>
          </div>

          <!-- Confirm Password -->
          <div>
            <label class="block text-sm font-medium mb-2">Повторите пароль</label>
            <div class="relative">
              <Lock class="absolute left-3 top-3 w-5 h-5 text-slate-400 pointer-events-none" />
              <input
                v-model="formData.confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                placeholder="••••••••"
                class="w-full pl-10 pr-10 py-2.5 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
                required
              />
              <button
                type="button"
                @click="showConfirmPassword = !showConfirmPassword"
                class="absolute right-3 top-3 text-slate-400 hover:text-slate-600 dark:hover:text-slate-300"
              >
                <Eye v-if="!showConfirmPassword" class="w-5 h-5" />
                <EyeOff v-else class="w-5 h-5" />
              </button>
            </div>
            <p
              v-if="formData.confirmPassword && formData.password !== formData.confirmPassword"
              class="text-xs text-red-600 dark:text-red-400 mt-1"
            >
              Пароли не совпадают
            </p>
          </div>

          <!-- Terms & Conditions -->
          <div class="flex items-start gap-3">
            <input
              v-model="formData.agreeToTerms"
              type="checkbox"
              id="terms"
              class="w-4 h-4 border-slate-300 rounded mt-1 cursor-pointer accent-primary-600"
            />
            <label for="terms" class="text-sm text-slate-600 dark:text-slate-400">
              Я согласен с
              <a href="#" class="text-primary-600 dark:text-primary-400 hover:underline">условиями использования</a>
              и
              <a href="#" class="text-primary-600 dark:text-primary-400 hover:underline">политикой конфиденциальности</a>
            </label>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="!isFormValid || isLoading"
            class="w-full mt-6 px-4 py-3 bg-gradient-to-r from-primary-600 to-accent-600 text-white rounded-lg font-medium hover:from-primary-700 hover:to-accent-700 disabled:from-slate-300 disabled:to-slate-400 disabled:cursor-not-allowed transition-all flex items-center justify-center gap-2"
          >
            <span v-if="!isLoading">Создать учётную запись</span>
            <span v-else>Пожалуйста, подождите...</span>
          </button>
        </form>

        <!-- Login Link -->
        <div class="mt-6 text-center text-sm text-slate-600 dark:text-slate-400">
          Уже есть учётная запись?
          <button
            @click="switchToLogin"
            class="text-primary-600 dark:text-primary-400 hover:underline font-medium ml-1"
          >
            Войти
          </button>
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
