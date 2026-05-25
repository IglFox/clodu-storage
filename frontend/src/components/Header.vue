<script setup>
import { useFileStore } from '../stores/files';
import { 
  Search, 
  ChevronRight, 
  Activity,
  Box
} from '@lucide/vue';

const fileStore = useFileStore();
</script>

<template>
  <header class="h-14 min-h-[56px] bg-surface border-b border-border flex items-center justify-between px-6 select-none z-30">
    <!-- Breadcrumbs -->
    <div class="flex items-center gap-1 overflow-hidden">
      <div 
        v-for="(crumb, idx) in fileStore.pathHistory" 
        :key="crumb.id"
        class="flex items-center"
      >
        <button 
          @click="fileStore.navigateTo(crumb.id)"
          class="text-[13px] font-medium transition-colors whitespace-nowrap"
          :class="idx === fileStore.pathHistory.length - 1 ? 'text-text-primary' : 'text-text-secondary hover:text-text-primary'"
        >
          {{ crumb.name }}
        </button>
        <ChevronRight 
          v-if="idx < fileStore.pathHistory.length - 1" 
          class="w-3.5 h-3.5 text-border mx-1" 
        />
      </div>
    </div>

    <!-- Actions & Stats -->
    <div class="flex items-center gap-6 pr-2">
      <!-- Network Metric -->
      <div class="hidden md:flex items-center gap-4 border-r border-border pr-6 mr-1">
        <div class="flex items-center gap-2">
          <Activity class="w-3.5 h-3.5 text-text-secondary" />
          <span class="text-[11px] font-medium text-text-secondary">{{ fileStore.avgLatency }}</span>
        </div>
        <div class="flex items-center gap-2">
          <Box class="w-3.5 h-3.5 text-text-secondary" />
          <span class="text-[11px] font-medium text-text-secondary">{{ fileStore.totalFilesCount }} items</span>
        </div>
      </div>

      <!-- Search -->
      <div class="relative w-48 md:w-64">
        <span class="absolute inset-y-0 left-0 pl-2.5 flex items-center pointer-events-none">
          <Search class="w-3.5 h-3.5 text-text-secondary" />
        </span>
        <input 
          v-model="fileStore.searchQuery"
          type="text" 
          placeholder="Search items..."
          class="w-full h-8 text-[12px] pl-8 pr-3 bg-accent-bg/50 border border-border rounded-md focus:outline-none focus:border-text-secondary/30 focus:bg-accent-bg transition-all"
        />
      </div>
    </div>
  </header>
</template>
