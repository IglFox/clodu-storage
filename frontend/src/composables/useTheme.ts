import { ref, onMounted, watch } from 'vue'

export function useTheme() {
  const isDark = ref(false)

  // Initialize theme from localStorage on mount
  onMounted(() => {
    const savedTheme = localStorage.getItem('theme-preference')
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches

    if (savedTheme === 'dark' || (!savedTheme && prefersDark)) {
      isDark.value = true
    } else {
      isDark.value = false
    }
  })

  // Watch isDark and update DOM
  watch(
    isDark,
    (newValue) => {
      const html = document.documentElement
      if (newValue) {
        html.classList.add('dark')
        localStorage.setItem('theme-preference', 'dark')
      } else {
        html.classList.remove('dark')
        localStorage.setItem('theme-preference', 'light')
      }
    },
    { immediate: true }
  )

  const toggleTheme = () => {
    isDark.value = !isDark.value
  }

  return {
    isDark,
    toggleTheme,
  }
}
