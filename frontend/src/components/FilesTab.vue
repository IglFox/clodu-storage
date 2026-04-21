<script setup lang="ts">
import { ref, computed } from 'vue'
import { Search, Upload, Plus, X, ChevronRight, File, Folder, Eye } from 'lucide-vue-next'
import { mockFiles, getFilesByParent, getFileById, getTotalSize, searchFiles } from '../stores/mockData'
import type { FileItem } from '../types'

const currentFolderId = ref<string>('root')
const searchQuery = ref('')
const showNewFolderModal = ref(false)
const newFolderName = ref('')
const selectedFile = ref<FileItem | null>(null)
const showFilePreview = ref(false)

// Get current folder
const currentFolder = computed(() => {
  return currentFolderId.value === 'root'
    ? null
    : mockFiles.find((f) => f.id === currentFolderId.value && f.type === 'folder')
})

// Get breadcrumb path
const breadcrumbPath = computed(() => {
  const path = []
  let currentId: string | null = currentFolderId.value

  while (currentId && currentId !== 'root') {
    const folder = mockFiles.find((f) => f.id === currentId && f.type === 'folder')
    if (!folder) break
    path.unshift(folder)
    currentId = folder.parentId
  }

  return path
})

// Get files to display
const displayedFiles = computed(() => {
  if (searchQuery.value) {
    return searchFiles(searchQuery.value)
  }
  return getFilesByParent(currentFolderId.value)
})

// Calculate stats
const stats = computed(() => {
  const allFiles = mockFiles
  const totalFiles = allFiles.filter((f) => f.type === 'file').length
  const totalFolders = allFiles.filter((f) => f.type === 'folder' && f.parentId !== null).length
  const totalSize = getTotalSize()
  const maxSize = 10 * 1024 * 1024 * 1024 // 10 GB
  const usagePercent = Math.round((totalSize / maxSize) * 100)

  return {
    totalFiles,
    totalFolders,
    totalSize,
    usagePercent,
    maxSize,
  }
})

// Format bytes to readable size
function formatSize(bytes: number): string {
  if (bytes === 0) return '0 B'
  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i]
}

// Format date
function formatDate(date: Date): string {
  return new Date(date).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  })
}

// Get icon for file type
function getFileIcon(file: FileItem): string {
  if (file.type === 'folder') return '📁'

  const ext = file.extension?.toLowerCase() || ''
  const iconMap: { [key: string]: string } = {
    pdf: '📄',
    doc: '📝',
    docx: '📝',
    txt: '📄',
    md: '📝',
    xlsx: '📊',
    xls: '📊',
    csv: '📊',
    ppt: '🎨',
    pptx: '🎨',
    jpg: '🖼️',
    jpeg: '🖼️',
    png: '🖼️',
    gif: '🖼️',
    svg: '🖼️',
    mp4: '🎬',
    avi: '🎬',
    mov: '🎬',
    mkv: '🎬',
    mp3: '🎵',
    wav: '🎵',
    flac: '🎵',
    zip: '📦',
    rar: '📦',
    '7z': '📦',
    sql: '🗄️',
    figma: '🎨',
  }

  return iconMap[ext] || '📋'
}

// Navigate to folder
function navigateToFolder(folderId: string) {
  currentFolderId.value = folderId
  searchQuery.value = ''
}

// Create new folder
function createNewFolder() {
  if (!newFolderName.value.trim()) return

  const newFolder: FileItem = {
    id: 'folder_' + Date.now(),
    name: newFolderName.value,
    type: 'folder',
    parentId: currentFolderId.value,
    size: 0,
    modified: new Date(),
    tags: ['folder'],
  }

  mockFiles.push(newFolder)
  newFolderName.value = ''
  showNewFolderModal.value = false
}

// Upload file (simulation)
function uploadFile() {
  const newFile: FileItem = {
    id: 'file_' + Date.now(),
    name: 'Document_' + Math.random().toString(36).substring(7).toUpperCase() + '.pdf',
    type: 'file',
    parentId: currentFolderId.value,
    size: Math.floor(Math.random() * 100) * 1024 * 1024,
    modified: new Date(),
    extension: 'pdf',
    tags: ['uploaded'],
    contentPreview: 'Uploaded document content preview...',
  }

  mockFiles.push(newFile)
}

// Preview file
function previewFile(file: FileItem) {
  if (file.type === 'folder') {
    navigateToFolder(file.id)
  } else {
    selectedFile.value = file
    showFilePreview.value = true
  }
}

// Close preview
function closePreview() {
  showFilePreview.value = false
  selectedFile.value = null
}
</script>

<template>
  <div class="space-y-6">
    <!-- KPI Cards -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div class="bg-gradient-to-br from-blue-50 to-blue-100 dark:from-blue-950 dark:to-blue-900 rounded-lg p-6 shadow-card hover:shadow-card-lg transition-shadow">
        <p class="text-sm text-blue-600 dark:text-blue-400 font-medium mb-2">Total Files</p>
        <p class="text-3xl font-bold text-blue-900 dark:text-blue-100">{{ stats.totalFiles }}</p>
        <p class="text-xs text-blue-600 dark:text-blue-400 mt-2">📄 Across all folders</p>
      </div>

      <div class="bg-gradient-to-br from-purple-50 to-purple-100 dark:from-purple-950 dark:to-purple-900 rounded-lg p-6 shadow-card hover:shadow-card-lg transition-shadow">
        <p class="text-sm text-purple-600 dark:text-purple-400 font-medium mb-2">Folders</p>
        <p class="text-3xl font-bold text-purple-900 dark:text-purple-100">{{ stats.totalFolders }}</p>
        <p class="text-xs text-purple-600 dark:text-purple-400 mt-2">📁 Organized storage</p>
      </div>

      <div class="bg-gradient-to-br from-emerald-50 to-emerald-100 dark:from-emerald-950 dark:to-emerald-900 rounded-lg p-6 shadow-card hover:shadow-card-lg transition-shadow">
        <p class="text-sm text-emerald-600 dark:text-emerald-400 font-medium mb-2">Storage Used</p>
        <p class="text-3xl font-bold text-emerald-900 dark:text-emerald-100">{{ formatSize(stats.totalSize) }}</p>
        <div class="mt-3 w-full bg-emerald-200 dark:bg-emerald-800 rounded-full h-2">
          <div
            class="bg-gradient-to-r from-emerald-500 to-emerald-600 h-2 rounded-full transition-all duration-300"
            :style="{ width: stats.usagePercent + '%' }"
          />
        </div>
        <p class="text-xs text-emerald-600 dark:text-emerald-400 mt-2">{{ stats.usagePercent }}% of 10 GB</p>
      </div>
    </div>

    <!-- Search Bar -->
    <div class="flex gap-3">
      <div class="flex-1 relative">
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Search files, folders, content..."
          class="w-full px-4 py-3 pl-10 rounded-full border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-800 text-slate-900 dark:text-slate-100 placeholder-slate-500 dark:placeholder-slate-400 focus:outline-none focus:ring-2 focus:ring-primary-500 transition-all"
        />
        <Search class="absolute left-3 top-3.5 w-5 h-5 text-slate-400 pointer-events-none" />
      </div>
      <button
        @click="uploadFile"
        class="px-6 py-3 bg-primary-600 dark:bg-primary-500 text-white rounded-full font-medium hover:bg-primary-700 dark:hover:bg-primary-600 transition-colors flex items-center gap-2 shadow-card hover:shadow-card-lg"
      >
        <Upload class="w-4 h-4" />
        Upload
      </button>
      <button
        @click="showNewFolderModal = true"
        class="px-6 py-3 bg-accent-600 dark:bg-accent-500 text-white rounded-full font-medium hover:bg-accent-700 dark:hover:bg-accent-600 transition-colors flex items-center gap-2 shadow-card hover:shadow-card-lg"
      >
        <Plus class="w-4 h-4" />
        Folder
      </button>
    </div>

    <!-- Breadcrumbs -->
    <div class="flex items-center gap-2 text-sm text-slate-600 dark:text-slate-400">
      <button
        @click="navigateToFolder('root')"
        class="hover:text-primary-600 dark:hover:text-primary-400 transition-colors"
      >
        🏠 Home
      </button>
      <template v-for="folder in breadcrumbPath" :key="folder.id">
        <ChevronRight class="w-4 h-4" />
        <button
          @click="navigateToFolder(folder.id)"
          class="hover:text-primary-600 dark:hover:text-primary-400 transition-colors"
        >
          {{ folder.name }}
        </button>
      </template>
    </div>

    <!-- Files Grid -->
    <div
      v-if="displayedFiles.length > 0"
      class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 auto-rows-max"
    >
      <div
        v-for="file in displayedFiles"
        :key="file.id"
        @click="previewFile(file)"
        @dblclick="previewFile(file)"
        class="bg-white dark:bg-slate-800 rounded-lg p-4 shadow-card hover:shadow-card-lg transition-all hover:scale-105 cursor-pointer group"
      >
        <div class="text-4xl mb-3">{{ getFileIcon(file) }}</div>
        <h3 class="font-medium text-sm mb-2 line-clamp-2 group-hover:text-primary-600 dark:group-hover:text-primary-400 transition-colors">
          {{ file.name }}
        </h3>
        <div class="space-y-1 text-xs text-slate-600 dark:text-slate-400">
          <p v-if="file.type === 'file'">{{ formatSize(file.size) }}</p>
          <p>{{ formatDate(file.modified) }}</p>
          <div v-if="file.tags.length > 0" class="flex flex-wrap gap-1 mt-2">
            <span
              v-for="tag in file.tags.slice(0, 2)"
              :key="tag"
              class="inline-block bg-primary-100 dark:bg-primary-900 text-primary-700 dark:text-primary-300 px-2 py-0.5 rounded text-xs"
            >
              {{ tag }}
            </span>
          </div>
        </div>
      </div>
    </div>

    <!-- Empty State -->
    <div
      v-else
      class="text-center py-12 bg-slate-50 dark:bg-slate-800/50 rounded-lg border border-dashed border-slate-300 dark:border-slate-700"
    >
      <div class="text-4xl mb-3">🔍</div>
      <h3 class="text-lg font-medium mb-1">No files found</h3>
      <p class="text-slate-600 dark:text-slate-400">
        {{ searchQuery ? 'Try a different search term' : 'Upload or create a folder to get started' }}
      </p>
    </div>

    <!-- New Folder Modal -->
    <teleport to="body">
      <div
        v-if="showNewFolderModal"
        class="fixed inset-0 bg-black/50 flex items-center justify-center z-50"
      >
        <div class="bg-white dark:bg-slate-800 rounded-lg p-6 w-96 max-w-sm mx-4 shadow-card-lg">
          <h2 class="text-xl font-bold mb-4">Create New Folder</h2>
          <input
            v-model="newFolderName"
            @keyup.enter="createNewFolder"
            type="text"
            placeholder="Folder name..."
            class="w-full px-4 py-2 border border-slate-200 dark:border-slate-700 bg-white dark:bg-slate-900 rounded-lg mb-4 focus:outline-none focus:ring-2 focus:ring-primary-500"
            autofocus
          />
          <div class="flex gap-3">
            <button
              @click="showNewFolderModal = false"
              class="flex-1 px-4 py-2 border border-slate-200 dark:border-slate-700 text-slate-900 dark:text-slate-100 rounded-lg hover:bg-slate-50 dark:hover:bg-slate-700 transition-colors"
            >
              Cancel
            </button>
            <button
              @click="createNewFolder"
              class="flex-1 px-4 py-2 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors"
            >
              Create
            </button>
          </div>
        </div>
      </div>
    </teleport>

    <!-- File Preview Modal -->
    <teleport to="body">
      <div
        v-if="showFilePreview && selectedFile"
        class="fixed inset-0 bg-black/50 flex items-center justify-center z-50"
      >
        <div class="bg-white dark:bg-slate-800 rounded-lg p-6 w-full max-w-lg mx-4 shadow-card-lg max-h-96 overflow-auto">
          <div class="flex items-start justify-between mb-4">
            <div class="flex items-center gap-3">
              <div class="text-4xl">{{ getFileIcon(selectedFile) }}</div>
              <div>
                <h2 class="text-xl font-bold">{{ selectedFile.name }}</h2>
                <p class="text-sm text-slate-600 dark:text-slate-400">{{ selectedFile.extension?.toUpperCase() }}</p>
              </div>
            </div>
            <button
              @click="closePreview"
              class="p-1 hover:bg-slate-100 dark:hover:bg-slate-700 rounded transition-colors"
            >
              <X class="w-5 h-5" />
            </button>
          </div>

          <div class="space-y-4">
            <div class="grid grid-cols-2 gap-4 py-4 border-y border-slate-200 dark:border-slate-700">
              <div>
                <p class="text-xs text-slate-600 dark:text-slate-400 mb-1">Size</p>
                <p class="font-medium">{{ formatSize(selectedFile.size) }}</p>
              </div>
              <div>
                <p class="text-xs text-slate-600 dark:text-slate-400 mb-1">Modified</p>
                <p class="font-medium">{{ formatDate(selectedFile.modified) }}</p>
              </div>
            </div>

            <div v-if="selectedFile.contentPreview">
              <h3 class="font-medium mb-2">Content Preview</h3>
              <p class="text-sm text-slate-700 dark:text-slate-300 whitespace-pre-wrap">
                {{ selectedFile.contentPreview }}
              </p>
            </div>

            <div v-if="selectedFile.tags.length > 0">
              <h3 class="font-medium mb-2">Tags</h3>
              <div class="flex flex-wrap gap-2">
                <span
                  v-for="tag in selectedFile.tags"
                  :key="tag"
                  class="inline-block bg-primary-100 dark:bg-primary-900 text-primary-700 dark:text-primary-300 px-3 py-1 rounded-full text-xs"
                >
                  {{ tag }}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </teleport>
  </div>
</template>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
