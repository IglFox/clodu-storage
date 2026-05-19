<script setup lang="ts">
interface Props {
  label: string
  value?: string | number
  loading?: boolean
}

defineProps<Props>()
</script>

<template>
  <div class="p-4 bg-white border border-gray-200 rounded">
    <p class="text-xs font-medium text-gray-600 mb-3">{{ label }}</p>
    <div v-if="loading" class="h-8 bg-gray-100 rounded animate-pulse" />
    <p v-else class="text-2xl font-semibold text-gray-900">{{ value }}</p>
  </div>
</template>
