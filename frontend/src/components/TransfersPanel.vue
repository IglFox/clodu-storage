<script setup>
import { ref } from 'vue';
import { useFileStore } from '../stores/files';
import { 
  ArrowUpRight, 
  ArrowDownLeft, 
  Skull, 
  Minimize2, 
  Maximize2, 
  CheckCircle2, 
  Cpu 
} from '@lucide/vue';

const fileStore = useFileStore();
const isMinimized = ref(false);
</script>

<template>
  <div 
    class="fixed bottom-4 right-4 z-50 w-80 bg-surface border border-border rounded-xl shadow-2xl overflow-hidden transition-all duration-300 select-none"
    :class="isMinimized ? 'h-12' : 'h-72'"
  >
    <!-- Card header tray -->
    <div class="px-4 py-3 bg-accent-bg border-b border-border flex justify-between items-center">
      <div class="flex items-center gap-2">
        <Cpu class="w-3.5 h-3.5 text-text-primary animate-spin" />
        <span class="text-[11px] font-bold text-text-primary uppercase tracking-widest">Active Transfers</span>
        <span class="text-[10px] font-bold px-1.5 py-0.5 rounded bg-folder text-white">
          {{ fileStore.transfers.length }}
        </span>
      </div>
      <button 
        @click="isMinimized = !isMinimized"
        class="text-text-secondary hover:text-text-primary cursor-pointer p-1 rounded"
      >
        <Minimize2 v-if="!isMinimized" class="w-3 h-3" />
        <Maximize2 v-else class="w-3 h-3" />
      </button>
    </div>

    <!-- Active List box -->
    <div v-if="!isMinimized" class="p-4 space-y-3 overflow-y-auto h-60 bg-surface">
      <div 
        v-for="t in fileStore.transfers" 
        :key="t.id"
        class="p-3 rounded-lg bg-canvas border border-border flex flex-col justify-between gap-1.5"
      >
        <div class="flex justify-between items-start gap-2">
          <!-- Icon indicator -->
          <div class="flex items-center gap-2 min-w-0">
            <div 
              class="p-1 rounded bg-accent-bg border border-border flex-shrink-0"
              :class="t.type === 'Shred' ? 'text-red-500' : 'text-text-primary'"
            >
              <ArrowUpRight v-if="t.type === 'Upload'" class="w-3.5 h-3.5" />
              <ArrowDownLeft v-else-if="t.type === 'Download'" class="w-3.5 h-3.5" />
              <Skull v-else class="w-3.5 h-3.5" />
            </div>
            
            <div class="min-w-0">
              <p class="text-[12px] font-medium text-text-primary truncate" :title="t.name">{{ t.name }}</p>
              <p class="text-[10px] text-text-secondary mt-0.5 capitalize">{{ t.type }}</p>
            </div>
          </div>

          <!-- Speed rates -->
          <div class="text-right">
            <span class="text-[10px] font-medium text-text-primary">{{ t.speed }}</span>
            <span 
              v-if="t.status === 'Completed'" 
              class="text-[9px] font-bold text-text-primary flex items-center gap-0.5 justify-end"
            >
              <CheckCircle2 class="w-2.5 h-2.5" /> Ready
            </span>
            <span v-else class="text-[9px] font-medium text-text-secondary animate-pulse block">Processing</span>
          </div>
        </div>

        <!-- Progress meters -->
        <div class="space-y-1">
          <div class="w-full h-1 bg-border rounded-full overflow-hidden">
            <div 
              class="h-full rounded-full transition-all duration-300"
              :class="t.type === 'Shred' ? 'bg-red-500' : 'bg-folder'"
              :style="{ width: `${t.progress}%` }"
            ></div>
          </div>
          <div class="flex justify-between items-center text-[9px] text-text-secondary font-mono">
            <span>{{ t.progress }}%</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
