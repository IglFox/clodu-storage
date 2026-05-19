<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  name: string
  size?: number
  type?: 'file' | 'folder'
  modified?: string
}

defineProps<Props>()

const formattedSize = computed(() => {
  const props = defineProps<Props>()
  if (!props.size) return ''

  const units = ['B', 'KB', 'MB', 'GB']
  let size = props.size
  let unitIndex = 0

  while (size >= 1024 && unitIndex < units.length - 1) {
    size /= 1024
    unitIndex++
  }

  return `${size.toFixed(1)} ${units[unitIndex]}`
})

const formattedDate = computed(() => {
  const props = defineProps<Props>()
  if (!props.modified) return ''

  try {
    const date = new Date(props.modified)
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    }).format(date)
  } catch {
    return props.modified
  }
})

const isFolder = computed(() => {
  const props = defineProps<Props>()
  return props.type === 'folder'
})
</script>

<template>
  <div
    class="flex items-center gap-3 px-4 py-3 hover:bg-gray-50 rounded transition-colors cursor-pointer"
  >
    <!-- Icon -->
    <div class="flex-shrink-0">
      <svg
        v-if="isFolder"
        class="w-5 h-5 text-gray-600"
        fill="currentColor"
        viewBox="0 0 20 20"
      >
        <path d="M2 6a2 2 0 012-2h5l2 2h5a2 2 0 012 2v6a2 2 0 01-2 2H4a2 2 0 01-2-2V6z" />
      </svg>
      <svg v-else class="w-5 h-5 text-gray-600" fill="currentColor" viewBox="0 0 20 20">
        <path
          fill-rule="evenodd"
          d="M8 4a3 3 0 00-3 3v4a5 5 0 0010 0V7a1 1 0 112 0v4a7 7 0 11-14 0V7a5 5 0 0110 0v4a3 3 0 11-6 0V7a1 1 0 012 0v4a1 1 0 102 0V7a3 3 0 00-3-3z"
          clip-rule="evenodd"
        />
      </svg>
    </div>

    <!-- Name -->
    <div class="flex-1 min-w-0">
      <p class="text-sm font-medium text-gray-900 truncate">{{ name }}</p>
    </div>

    <!-- Size (file only) -->
    <div v-if="!isFolder && size" class="flex-shrink-0 text-xs text-gray-500">
      {{ formattedSize }}
    </div>

    <!-- Modified date -->
    <div v-if="modified" class="flex-shrink-0 text-xs text-gray-500 min-w-fit">
      {{ formattedDate }}
    </div>
  </div>
</template>
