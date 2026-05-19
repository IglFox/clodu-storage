<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useApi } from '../composables/useApi'
import SearchBar from '../components/SearchBar.vue'
import FileItem from '../components/FileItem.vue'
import RagAnswerCard from '../components/RagAnswerCard.vue'

const router = useRouter()
const { data: filesData, loading: filesLoading, execute: fetchFiles } = useApi()
const { data: ragData, loading: ragLoading, execute: fetchRag } = useApi()

const searchQuery = ref('')
const files = ref<any[]>([])
const ragAnswer = ref<string>('')
const viewMode = ref<'grid' | 'list'>(
  (import.meta.env.VITE_DEFAULT_VIEW as 'grid' | 'list') || 'grid',
)

const enableRag = import.meta.env.VITE_ENABLE_RAG === 'true'
const enableAnalytics = import.meta.env.VITE_ENABLE_ANALYTICS === 'true'
const filesPath = import.meta.env.VITE_FILES_PATH || '/storage/files'
const searchPath = import.meta.env.VITE_SEARCH_PATH || '/storage/search'
const ragPath = import.meta.env.VITE_RAG_QUERY_PATH || '/rag/query'

// Debounced search
let searchTimeout: ReturnType<typeof setTimeout>
watch(
  searchQuery,
  async (newQuery) => {
    clearTimeout(searchTimeout)

    if (!newQuery.trim()) {
      // Load all files
      const response = await fetchFiles(filesPath)
      files.value = response?.files || []
    } else {
      // Search files
      searchTimeout = setTimeout(async () => {
        const response = await fetchFiles(`${searchPath}?q=${encodeURIComponent(newQuery)}`)
        files.value = response?.files || []
      }, 300)
    }
  },
)

// Handle search submission for RAG
const handleSearchSubmit = async () => {
  if (!searchQuery.value.trim() || !enableRag) return

  ragAnswer.value = ''
  const response = await fetchRag(ragPath, {
    method: 'POST',
    body: { query: searchQuery.value },
  })

  if (response?.answer) {
    ragAnswer.value = response.answer
  }
}

// Load initial files
const loadInitialFiles = async () => {
  const response = await fetchFiles(filesPath)
  files.value = response?.files || []
}

loadInitialFiles()

// Navigation
const handleLogout = () => {
  localStorage.removeItem('authToken')
  router.push('/login')
}

const handleAnalytics = () => {
  router.push('/analytics')
}

const toggleViewMode = () => {
  viewMode.value = viewMode.value === 'grid' ? 'list' : 'grid'
}
</script>

<template>
  <div class="min-h-screen bg-slate-50">
    <!-- Main content -->
    <div class="max-w-4xl mx-auto px-4 py-8">
      <!-- Search bar -->
      <div class="mb-8">
        <SearchBar
          v-model="searchQuery"
          @submit="handleSearchSubmit"
          @clear="searchQuery = ''"
        />
      </div>

      <!-- RAG Answer Card -->
      <RagAnswerCard
        :answer="ragAnswer"
        :loading="ragLoading"
        @dismiss="ragAnswer = ''"
      />

      <!-- File list -->
      <div v-if="files.length" class="space-y-1">
        <FileItem
          v-for="file in files"
          :key="file.id"
          :name="file.name"
          :size="file.size"
          :type="file.type"
          :modified="file.modified"
        />
      </div>
      <div v-else-if="!filesLoading" class="text-center py-8 text-gray-500 text-sm">
        {{ searchQuery ? 'No files found' : 'No files' }}
      </div>
      <div v-else class="text-center py-8 text-gray-400 text-sm">Loading…</div>
    </div>

    <!-- Top-right controls -->
    <div class="fixed top-4 right-4 z-10 flex gap-2">
      <!-- Analytics button -->
      <button
        v-if="enableAnalytics"
        @click="handleAnalytics"
        type="button"
        class="p-2 hover:bg-gray-100 rounded transition-colors"
        title="Analytics"
      >
        <svg class="w-5 h-5 text-gray-600" fill="currentColor" viewBox="0 0 20 20">
          <path
            d="M3 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zm0 4a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z"
          />
        </svg>
      </button>

      <!-- Logout button -->
      <button
        @click="handleLogout"
        type="button"
        class="p-2 hover:bg-gray-100 rounded transition-colors"
        title="Logout"
      >
        <svg class="w-5 h-5 text-gray-600" fill="currentColor" viewBox="0 0 20 20">
          <path
            fill-rule="evenodd"
            d="M3 3a1 1 0 011 1v12a1 1 0 11-2 0V4a1 1 0 011-1zm7.707 5.293a1 1 0 010 1.414L9.414 10l1.293 1.293a1 1 0 01-1.414 1.414l-2-2a1 1 0 010-1.414l2-2a1 1 0 011.414 0zm2.586 0a1 1 0 011.414 0l2 2a1 1 0 010 1.414l-2 2a1 1 0 11-1.414-1.414L14.586 10l-1.293-1.293a1 1 0 010-1.414z"
            clip-rule="evenodd"
          />
        </svg>
      </button>
    </div>
  </div>
</template>
