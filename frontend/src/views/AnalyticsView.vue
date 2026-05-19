<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useApi } from '../composables/useApi'
import StatCard from '../components/StatCard.vue'
import DonutChart from '../components/DonutChart.vue'

const router = useRouter()
const { data: analyticsData, loading, execute: fetchAnalytics } = useApi()

const stats = ref<{
  totalStorage?: string
  usedSpace?: string
  freeSpace?: string
  fileCount?: number
}>({})

const storageTypes = ref<any[]>([])
const largestFiles = ref<any[]>([])

const analyticsPath = import.meta.env.VITE_ANALYTICS_PATH || '/storage/analytics'

onMounted(async () => {
  const response = await fetchAnalytics(analyticsPath)

  if (response) {
    stats.value = {
      totalStorage: response.totalStorage,
      usedSpace: response.usedSpace,
      freeSpace: response.freeSpace,
      fileCount: response.fileCount,
    }
    storageTypes.value = response.types || []
    largestFiles.value = response.largestFiles || []
  }
})

const handleBack = () => {
  router.push('/main')
}
</script>

<template>
  <div class="min-h-screen bg-slate-50">
    <div class="max-w-4xl mx-auto px-4 py-8">
      <!-- Back button -->
      <button
        @click="handleBack"
        type="button"
        class="mb-8 text-sm text-gray-600 hover:text-gray-900 transition-colors flex items-center gap-1"
      >
        <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
          <path
            fill-rule="evenodd"
            d="M7.707 7.293a1 1 0 010 1.414L5.414 10l2.293 2.293a1 1 0 11-1.414 1.414l-3-3a1 1 0 010-1.414l3-3a1 1 0 011.414 0zm5.586 0a1 1 0 011.414 0l3 3a1 1 0 010 1.414l-3 3a1 1 0 11-1.414-1.414L14.586 10l-2.293-2.293a1 1 0 010-1.414z"
            clip-rule="evenodd"
          />
        </svg>
        Back
      </button>

      <!-- Heading -->
      <h1 class="text-xl font-semibold text-gray-700 mb-8">Storage Analytics</h1>

      <!-- Stats grid -->
      <div class="grid grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
        <StatCard
          label="Total Storage"
          :value="stats.totalStorage"
          :loading="loading"
        />
        <StatCard label="Used Space" :value="stats.usedSpace" :loading="loading" />
        <StatCard label="Free Space" :value="stats.freeSpace" :loading="loading" />
        <StatCard label="File Count" :value="stats.fileCount" :loading="loading" />
      </div>

      <!-- Donut chart -->
      <div class="mb-8">
        <DonutChart :data="storageTypes" />
      </div>

      <!-- Largest files -->
      <div v-if="largestFiles && largestFiles.length" class="mt-8">
        <h2 class="text-sm font-semibold text-gray-700 mb-4">Top 5 Largest Files</h2>
        <div class="space-y-2">
          <div
            v-for="file in largestFiles"
            :key="file.id"
            class="flex items-center justify-between px-4 py-3 bg-white border border-gray-200 rounded text-sm"
          >
            <span class="text-gray-900 truncate">{{ file.name }}</span>
            <span class="text-gray-500 flex-shrink-0 ml-4">{{ file.size }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
