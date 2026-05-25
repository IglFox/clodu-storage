<script setup>
import { computed } from 'vue';
import { useAuthStore } from '../stores/auth';
import { useFileStore } from '../stores/files';
import { 
  Plus, 
  Folder, 
  Clock, 
  Star, 
  Trash2,
  ChevronDown
} from '@lucide/vue';

const props = defineProps({
  activeTab: {
    type: String,
    required: true
  }
});

const authStore = useAuthStore();
const fileStore = useFileStore();

const sections = [
  { label: 'Favorites', icon: Star },
  { label: 'Recent', icon: Clock },
  { label: 'Trash', icon: Trash2 },
];
</script>

<template>
  <aside class="w-60 bg-surface border-r border-border flex flex-col z-40 select-none">
    <!-- Context Header -->
    <div class="h-14 px-4 flex items-center justify-between border-b border-border">
      <span class="text-[11px] font-bold text-text-secondary uppercase tracking-widest">
        {{ activeTab }}
      </span>
      <ChevronDown class="w-3.5 h-3.5 text-text-secondary" />
    </div>

    <!-- Navigation -->
    <div class="flex-1 py-4 overflow-y-auto">
      <!-- Action -->
      <div class="px-3 mb-6">
        <button class="w-full flex items-center justify-center gap-2 px-3 py-2 rounded-lg bg-folder text-canvas text-[12px] font-medium hover:opacity-90 transition-all border border-border">
          <Plus class="w-4 h-4" />
          <span>Add New</span>
        </button>
      </div>

      <!-- Main Folders -->
      <div class="px-2 space-y-0.5">
        <div class="px-3 py-2 text-[11px] font-semibold text-text-secondary uppercase tracking-wider">
          Library
        </div>
        <button 
          @click="fileStore.navigateTo('root')"
          class="w-full flex items-center gap-2.5 px-3 py-2 rounded-md text-[13px] transition-colors font-medium"
          :class="fileStore.currentFolderId === 'root' ? 'bg-accent-bg text-text-primary shadow-sm' : 'text-text-secondary hover:bg-accent-bg/50 hover:text-text-primary'"
        >
          <Folder class="w-4 h-4" />
          <span>Root Space</span>
        </button>
      </div>

      <!-- Secondary Sections -->
      <div class="mt-6 px-2 space-y-0.5">
        <div class="px-3 py-2 text-[11px] font-semibold text-text-secondary uppercase tracking-wider">
          Filters
        </div>
        <button 
          v-for="sec in sections" 
          :key="sec.label"
          class="w-full flex items-center gap-2.5 px-3 py-1.5 rounded-md text-[13px] text-text-secondary hover:bg-accent-bg/50 hover:text-text-primary transition-colors"
        >
          <component :is="sec.icon" class="w-4 h-4" />
          <span>{{ sec.label }}</span>
        </button>
      </div>
    </div>

    <!-- User Footer -->
    <div class="p-4 border-t border-border bg-accent-bg/20">
      <div class="flex items-center gap-3">
        <div class="w-8 h-8 rounded-full bg-folder border border-border flex items-center justify-center text-[16px] leading-none">
          {{ authStore.currentUser?.avatar }}
        </div>
        <div class="min-w-0">
          <p class="text-[12px] font-medium text-text-primary truncate">{{ authStore.currentUser?.name }}</p>
          <p class="text-[10px] text-text-secondary truncate">{{ authStore.currentUser?.role }}</p>
        </div>
      </div>
    </div>
  </aside>
</template>
