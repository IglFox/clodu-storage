<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { Chart as ChartJS, ArcElement, CategoryScale, LinearScale, PointElement, LineElement, BarElement, Tooltip, Legend, DoughnutController, LineController, BarController, Filler } from 'chart.js'
import { mockFiles, getTotalSize, getFileStats, getLargeFiles } from '../stores/mockData'

ChartJS.register(ArcElement, CategoryScale, LinearScale, PointElement, LineElement, BarElement, Tooltip, Legend, DoughnutController, LineController, BarController, Filler)

const doughnutChartRef = ref<HTMLCanvasElement | null>(null)
const activityChartRef = ref<HTMLCanvasElement | null>(null)
let doughnutChart: any = null
let activityChart: any = null

const fileStats = computed(() => getFileStats())
const largeFiles = computed(() => getLargeFiles(50 * 1024 * 1024))

const stats = computed(() => {
  const totalSize = getTotalSize()
  const maxSize = 10 * 1024 * 1024 * 1024 // 10 GB
  const usagePercent = Math.round((totalSize / maxSize) * 100)

  return {
    totalSize,
    usagePercent,
    maxSize,
  }
})

// Generate activity data (last 7 days)
const activityData = computed(() => {
  const today = new Date()
  const days = []
  const data = []

  for (let i = 6; i >= 0; i--) {
    const date = new Date(today)
    date.setDate(date.getDate() - i)
    days.push(date.toLocaleDateString('en-US', { weekday: 'short' }))
    // Random activity data
    data.push(Math.floor(Math.random() * 100) + 20)
  }

  return { days, data }
})

function formatSize(bytes: number): string {
  if (bytes === 0) return '0 B'
  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i]
}

function formatDate(date: Date): string {
  return new Date(date).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  })
}

onMounted(() => {
  // Doughnut Chart
  if (doughnutChartRef.value) {
    const ctx = doughnutChartRef.value.getContext('2d')
    if (ctx) {
      const colors = ['#3b82f6', '#8b5cf6', '#ec4899', '#f59e0b', '#64748b']
      const data = Object.entries(fileStats.value)
        .map(([key, stat]) => stat.size)
        .sort((a, b) => b - a)

      doughnutChart = new ChartJS(ctx, {
        type: 'doughnut',
        data: {
          labels: Object.entries(fileStats.value)
            .sort((a, b) => b[1].size - a[1].size)
            .map(([_, stat]) => stat.displayName),
          datasets: [
            {
              data: data,
              backgroundColor: colors.slice(0, data.length),
              borderColor: '#fff',
              borderWidth: 2,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: true,
          plugins: {
            legend: {
              position: 'bottom',
              labels: {
                padding: 20,
                font: {
                  size: 13,
                  weight: '500',
                },
                usePointStyle: true,
              },
            },
          },
        },
      })
    }
  }

  // Activity Chart
  if (activityChartRef.value) {
    const ctx = activityChartRef.value.getContext('2d')
    if (ctx) {
      activityChart = new ChartJS(ctx, {
        type: 'line',
        data: {
          labels: activityData.value.days,
          datasets: [
            {
              label: 'Upload Count',
              data: activityData.value.data,
              borderColor: '#0ea5e9',
              backgroundColor: '#0ea5e910',
              borderWidth: 3,
              fill: true,
              tension: 0.4,
              pointRadius: 5,
              pointBackgroundColor: '#0ea5e9',
              pointBorderColor: '#fff',
              pointBorderWidth: 2,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: true,
          plugins: {
            legend: {
              labels: {
                font: {
                  size: 13,
                  weight: '500',
                },
              },
            },
          },
          scales: {
            y: {
              beginAtZero: true,
              max: 150,
            },
          },
        },
      })
    }
  }
})
</script>

<template>
  <div class="space-y-6">
    <!-- Main Stats -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <!-- Storage Progress -->
      <div class="bg-white dark:bg-slate-800 rounded-lg p-6 shadow-card">
        <h2 class="text-lg font-bold mb-4">Storage Usage</h2>
        <div class="space-y-4">
          <div class="text-4xl font-bold text-primary-600 dark:text-primary-400">
            {{ formatSize(stats.totalSize) }}
          </div>
          <p class="text-slate-600 dark:text-slate-400">of 10 GB used</p>

          <div class="w-full bg-slate-200 dark:bg-slate-700 rounded-full h-3">
            <div
              class="bg-gradient-to-r from-primary-500 to-accent-500 h-3 rounded-full transition-all duration-500"
              :style="{ width: stats.usagePercent + '%' }"
            />
          </div>

          <p class="text-sm text-slate-600 dark:text-slate-400">{{ stats.usagePercent }}% capacity</p>
        </div>
      </div>

      <!-- File Type Distribution -->
      <div class="bg-white dark:bg-slate-800 rounded-lg p-6 shadow-card">
        <h2 class="text-lg font-bold mb-4">File Type Distribution</h2>
        <div class="space-y-3">
          <div v-for="(stat, key) in fileStats" :key="key" class="flex items-center justify-between">
            <div class="flex items-center gap-3">
              <span class="text-2xl">{{ stat.icon }}</span>
              <div>
                <p class="font-medium text-sm">{{ stat.displayName }}</p>
                <p class="text-xs text-slate-600 dark:text-slate-400">{{ stat.count }} files</p>
              </div>
            </div>
            <p class="font-bold text-primary-600 dark:text-primary-400">{{ formatSize(stat.size) }}</p>
          </div>
        </div>
      </div>
    </div>

    <!-- Charts -->
    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Doughnut Chart -->
      <div class="bg-white dark:bg-slate-800 rounded-lg p-6 shadow-card">
        <h2 class="text-lg font-bold mb-4">Space by Type</h2>
        <div class="h-64">
          <canvas ref="doughnutChartRef"></canvas>
        </div>
      </div>

      <!-- Activity Chart -->
      <div class="bg-white dark:bg-slate-800 rounded-lg p-6 shadow-card">
        <h2 class="text-lg font-bold mb-4">Activity (Last 7 Days)</h2>
        <div class="h-64">
          <canvas ref="activityChartRef"></canvas>
        </div>
      </div>
    </div>

    <!-- Large Files -->
    <div class="bg-white dark:bg-slate-800 rounded-lg p-6 shadow-card">
      <h2 class="text-lg font-bold mb-4">Largest Files (>50 MB)</h2>
      <div v-if="largeFiles.length > 0" class="space-y-3">
        <div
          v-for="file in largeFiles"
          :key="file.id"
          class="flex items-center justify-between p-3 bg-slate-50 dark:bg-slate-700/50 rounded-lg hover:bg-slate-100 dark:hover:bg-slate-700 transition-colors"
        >
          <div class="flex items-center gap-3 min-w-0">
            <span class="text-2xl flex-shrink-0">
              {{ file.extension === 'pdf' ? '📄' : file.extension === 'zip' ? '📦' : file.extension === 'mp4' ? '🎬' : file.extension === 'sql' ? '🗄️' : file.extension === 'jpg' ? '🖼️' : '📋' }}
            </span>
            <div class="min-w-0">
              <p class="font-medium truncate">{{ file.name }}</p>
              <p class="text-xs text-slate-600 dark:text-slate-400">{{ formatDate(file.modified) }}</p>
            </div>
          </div>
          <p class="font-bold text-primary-600 dark:text-primary-400 text-right flex-shrink-0 ml-4">
            {{ formatSize(file.size) }}
          </p>
        </div>
      </div>
      <div v-else class="text-center py-8 text-slate-600 dark:text-slate-400">
        <p>No files larger than 50 MB</p>
      </div>
    </div>
  </div>
</template>
