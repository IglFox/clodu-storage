<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  modelValue: string
  placeholder?: string
}

interface Emits {
  (e: 'update:modelValue', value: string): void
  (e: 'clear'): void
  (e: 'submit'): void
}

defineProps<Props>()
defineEmits<Emits>()

const showClear = computed(() => {
  const value = (this as any).modelValue
  return value && value.length > 0
})
</script>

<template>
  <div class="relative w-full">
    <div class="relative flex items-center">
      <!-- Search icon -->
      <svg
        class="absolute left-3 w-4 h-4 text-gray-400 pointer-events-none"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
        />
      </svg>

      <input
        :value="modelValue"
        @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
        @keydown.enter="$emit('submit')"
        :placeholder="placeholder || 'Search files or ask a question…'"
        type="text"
        class="w-full pl-10 pr-10 py-2 border border-gray-200 rounded bg-white text-sm focus:outline-none focus:border-blue-600 focus:ring-1 focus:ring-blue-600"
      />

      <!-- Clear button -->
      <button
        v-if="modelValue"
        @click="$emit('clear')"
        type="button"
        class="absolute right-3 text-gray-400 hover:text-gray-600 transition-colors"
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
