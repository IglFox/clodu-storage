<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useFileStore } from '../stores/files';
import FileGrid from '../components/FileGrid.vue';
import UploadModal from '../components/UploadModal.vue';
import { 
  FolderPlus,
  UploadCloud,
  ChevronRight,
  X
} from '@lucide/vue';

const authStore = useAuthStore();
const fileStore = useFileStore();

const isUploadOpen = ref(false);
const isNewFolderOpen = ref(false);
const newFolderName = ref('');
const folderError = ref('');

function handleCreateFolder() {
  folderError.value = '';
  if (!newFolderName.value.trim()) {
    folderError.value = 'Please provide a name.';
    return;
  }
  
  const success = fileStore.createFolder(newFolderName.value.trim());
  if (success) {
    newFolderName.value = '';
    isNewFolderOpen.value = false;
  }
}
</script>

<template>
  <div class="max-w-6xl mx-auto space-y-10 animate-fade-fast">
    <!-- View Header -->
    <div class="flex items-end justify-between border-b border-border pb-6">
      <div>
        <div class="flex items-center gap-2 text-text-secondary text-[11px] font-bold uppercase tracking-widest mb-1.5">
          <span>Enterprise Cloud</span>
          <ChevronRight class="w-3 h-3" />
          <span class="text-text-primary">File Explorer</span>
        </div>
        <h1 class="text-2xl font-light text-text-primary tracking-tight">
          Welcome, {{ authStore.currentUser?.name.split(' ')[0] }}
        </h1>
      </div>

      <div class="flex items-center gap-3">
        <button 
          @click="isNewFolderOpen = true"
          class="px-4 py-1.5 rounded-lg border border-folder bg-surface text-text-primary text-xs font-medium hover:bg-accent-bg active:scale-[0.98] transition-all flex items-center gap-2 shadow-sm"
        >
          <FolderPlus class="w-3.5 h-3.5" />
          <span>New Directory</span>
        </button>
        <button 
          @click="isUploadOpen = true"
          class="px-4 py-1.5 rounded-lg bg-folder text-white text-xs font-medium hover:opacity-90 active:scale-[0.98] transition-all flex items-center gap-2"
        >
          <UploadCloud class="w-3.5 h-3.5" />
          <span>Upload</span>
        </button>
      </div>
    </div>

    <!-- Explorer -->
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h2 class="text-[11px] font-bold text-text-secondary uppercase tracking-widest">
          {{ fileStore.pathHistory[fileStore.pathHistory.length - 1].name }}
        </h2>
        <div class="flex items-center gap-4 text-[11px] font-medium text-text-secondary">
          <span>{{ fileStore.currentFolderStats.foldersCount }} Folders</span>
          <span class="w-1 h-1 rounded-full bg-border"></span>
          <span>{{ fileStore.currentFolderStats.filesCount }} Files</span>
        </div>
      </div>

      <FileGrid />
    </div>

    <!-- Modals -->
    <UploadModal :is-open="isUploadOpen" @close="isUploadOpen = false" />

    <!-- Create Folder Modal -->
    <div 
      v-if="isNewFolderOpen"
      class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-folder/20 backdrop-blur-[2px]"
      @click.self="isNewFolderOpen = false"
    >
      <div class="w-full max-w-sm bg-surface border border-border rounded-xl p-8 shadow-2xl animate-fade-fast relative">
        <button @click="isNewFolderOpen = false" class="absolute top-4 right-4 text-text-secondary hover:text-text-primary">
          <X class="w-4 h-4" />
        </button>
        
        <h3 class="text-lg font-medium text-text-primary tracking-tight mb-1">New Directory</h3>
        <p class="text-[12px] text-text-secondary mb-6">Enter a name for the virtual container</p>

        <div v-if="folderError" class="mb-4 text-[11px] text-red-600 bg-red-50 p-2 rounded border border-red-100">
          {{ folderError }}
        </div>

        <input 
          v-model="newFolderName"
          type="text" 
          placeholder="e.g. Finance-2026"
          class="input-neutral mb-6"
          @keydown.enter="handleCreateFolder"
          autofocus
        />

        <div class="flex gap-3">
          <button @click="isNewFolderOpen = false" class="flex-1 btn-neutral h-10">Cancel</button>
          <button @click="handleCreateFolder" class="flex-1 h-10 bg-folder text-white text-[13px] font-medium rounded-lg hover:opacity-95 active:scale-[0.98] transition-all">Create</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style>
.animate-fade-fast {
  animation: fadeIn 0.2s ease-out;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(4px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
