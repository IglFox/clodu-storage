import { ref } from 'vue'

interface UseApiOptions {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE'
  body?: Record<string, any>
}

export function useApi() {
  const data = ref<any>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const execute = async (relativePath: string, options: UseApiOptions = {}): Promise<any> => {
    const {
      method = 'GET',
      body,
    } = options

    loading.value = true
    error.value = null

    try {
      const baseUrl = import.meta.env.VITE_API_GATEWAY_URL
      if (!baseUrl) {
        throw new Error('VITE_API_GATEWAY_URL is not configured')
      }

      const url = `${baseUrl}${relativePath}`
      const token = localStorage.getItem('authToken')

      const fetchOptions: RequestInit = {
        method,
        headers: {
          'Content-Type': 'application/json',
        },
      }

      if (token) {
        fetchOptions.headers = {
          ...fetchOptions.headers,
          'Authorization': `Bearer ${token}`,
        }
      }

      if (body) {
        fetchOptions.body = JSON.stringify(body)
      }

      const response = await fetch(url, fetchOptions)

      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}))
        throw new Error(errorData.message || `HTTP ${response.status}: ${response.statusText}`)
      }

      const responseData = await response.json()
      data.value = responseData
      return responseData
    } catch (err) {
      const message = err instanceof Error ? err.message : 'An error occurred'
      error.value = message
      return null
    } finally {
      loading.value = false
    }
  }

  return {
    data,
    loading,
    error,
    execute,
  } as const
}
