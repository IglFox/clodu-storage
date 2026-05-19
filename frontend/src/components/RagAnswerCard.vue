<script setup lang="ts">
interface Props {
  answer: string
  loading?: boolean
}

defineProps<Props>()

interface Emits {
  (e: 'dismiss'): void
}

defineEmits<Emits>()
</script>

<template>
  <div
    v-if="answer || loading"
    class="mb-6 p-4 bg-white border border-gray-200 rounded"
  >
    <div class="flex gap-3">
      <!-- AI Icon -->
      <div class="flex-shrink-0">
        <svg
          v-if="!loading"
          class="w-5 h-5 text-blue-600"
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path
            d="M10.458 3c.847 0 1.668.037 2.458.111v2.753a5.5 5.5 0 00-4.916-2.737c-.849 0-1.67.038-2.458.112V3c.79-.074 1.61-.111 2.458-.111zm4.616 1.426a5.5 5.5 0 012.545 2.008V4.34c-.849-.152-1.702-.27-2.545-.352zM17 7.5c.37 0 .736.022 1.1.064v5.832a.5.5 0 01-.5.604h-1v-6.5zM3 12.5v6.5H2a.5.5 0 01-.5-.604V7.564C1.862 7.522 2.23 7.5 2.6 7.5H3zm0-5.5v5H2V7h1z"
          />
        </svg>
        <div v-else class="flex gap-1">
          <div
            class="w-1 h-1 bg-blue-600 rounded-full animate-pulse"
            style="animation-delay: 0s"
          />
          <div
            class="w-1 h-1 bg-blue-600 rounded-full animate-pulse"
            style="animation-delay: 0.1s"
          />
          <div
            class="w-1 h-1 bg-blue-600 rounded-full animate-pulse"
            style="animation-delay: 0.2s"
          />
        </div>
      </div>

      <!-- Content -->
      <div class="flex-1">
        <p v-if="loading" class="text-sm text-gray-600">AI is thinking…</p>
        <p v-else class="text-sm text-gray-700">{{ answer }}</p>
      </div>

      <!-- Dismiss button -->
      <button
        @click="$emit('dismiss')"
        type="button"
        class="flex-shrink-0 text-gray-400 hover:text-gray-600 transition-colors"
      >
        <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
          <path
            fill-rule="evenodd"
            d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
            clip-rule="evenodd"
          />
        </svg>
      </button>
    </div>
  </div>
</template>
