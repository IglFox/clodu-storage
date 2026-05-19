<script setup lang="ts">
import { computed } from 'vue'

interface DataItem {
  label: string
  value: number
}

interface Props {
  data?: DataItem[]
}

const props = defineProps<Props>()

const colors = ['#2563eb', '#10b981', '#f59e0b', '#6b7280']

const total = computed(() => {
  return (props.data || []).reduce((sum, item) => sum + item.value, 0)
})

interface DataWithCalculations extends DataItem {
  percentage: number
  strokeDashoffset: number
}

const processedData = computed((): DataWithCalculations[] => {
  if (!props.data) return []

  let offset = 0
  return props.data.map((item, index) => {
    const percentage = total.value > 0 ? (item.value / total.value) * 100 : 0
    const circumference = 2 * Math.PI * 45 // radius = 45
    const dashoffset = circumference - (percentage / 100) * circumference
    const prevOffset = offset
    offset = dashoffset

    return {
      ...item,
      percentage,
      strokeDashoffset: prevOffset,
    }
  })
})
</script>

<template>
  <div class="p-4 bg-white border border-gray-200 rounded">
    <div class="flex items-center justify-center gap-8">
      <!-- SVG Donut -->
      <svg v-if="data && data.length" width="120" height="120" viewBox="0 0 120 120">
        <circle
          v-for="(item, index) in processedData"
          :key="item.label"
          cx="60"
          cy="60"
          r="45"
          fill="none"
          :stroke="colors[index % colors.length]"
          stroke-width="8"
          :stroke-dasharray="`${(item.percentage / 100) * 2 * Math.PI * 45} ${2 * Math.PI * 45}`"
          :stroke-dashoffset="item.strokeDashoffset"
          stroke-linecap="round"
          style="transform: rotate(-90deg); transform-origin: 60px 60px"
        />
      </svg>

      <!-- Legend -->
      <div v-if="data && data.length" class="space-y-2">
        <div v-for="(item, index) in data" :key="item.label" class="flex items-center gap-2">
          <div
            class="w-3 h-3 rounded-full"
            :style="{ backgroundColor: colors[index % colors.length] }"
          />
          <div class="text-xs text-gray-600">
            <div class="font-medium">{{ item.label }}</div>
            <div class="text-gray-500">
              {{
                total > 0
                  ? ((item.value / total) * 100).toFixed(1)
                  : '0'
              }}%
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
