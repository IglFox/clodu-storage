<script setup>
import { ref, onMounted } from 'vue';
import { useFileStore } from '../stores/files';
import { formatSize } from '../utils/parser';
import { 
  Folder, 
  FileText, 
  Image as ImageIcon, 
  Video, 
  Archive, 
  FileCode, 
  File, 
  Trash2, 
  MoreHorizontal,
  ArrowLeft
} from '@lucide/vue';

const fileStore = useFileStore();
const activeMenu = ref(null);

onMounted(() => {
  fileStore.fetchFiles();
  document.addEventListener('click', () => { activeMenu.value = null; });
});

function handleItemClick(item) {
  if (item.type === 'folder') {
    fileStore.navigateTo(item.id);
  }
}

function getIcon(category) {
  switch (category) {
    case 'Image': return ImageIcon;
    case 'Media': return Video;
    case 'Code': return FileCode;
    default: return FileText;
  }
}
</script>

<template>
  <div class="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-6">
    <!-- BACK BUTTON -->
    <button 
      v-if="fileStore.currentFolderId !== 'root'"
      @click="fileStore.navigateUp()"
      class="col-span-full mb-2 flex items-center gap-2 text-[12px] text-text-secondary hover:text-text-primary transition-colors"
    >
      <ArrowLeft class="w-4 h-4" />
      Back
    </button>

    <!-- FOLDERS -->
    <div 
      v-for="item in fileStore.currentItems.filter(i => i.type === 'folder')"
      :key="item.id"
      class="group"
      @click="handleItemClick(item)"
    >
      <div class="aspect-square bg-folder rounded-lg flex items-center justify-center mb-3 relative cursor-pointer hover:opacity-95 transition-opacity overflow-hidden">
        <Folder class="w-10 h-10 text-white/10" />
        <div class="absolute bottom-2 right-2 text-[11px] text-white/50 font-mono">
          {{ item.itemCount || 0 }}
        </div>
      </div>
      <div class="flex items-center justify-between px-0.5">
        <span class="text-[12px] font-medium text-text-primary truncate">{{ item.name }}</span>
        <button class="opacity-0 group-hover:opacity-100 transition-opacity">
          <MoreHorizontal class="w-3.5 h-3.5 text-text-secondary" />
        </button>
      </div>
    </div>

    <!-- FILES -->
    <div 
      v-for="item in fileStore.currentItems.filter(i => i.type === 'file')"
      :key="item.id"
      class="group relative"
    >
      <div class="aspect-square bg-surface border border-border rounded-lg flex items-center justify-center mb-3 relative cursor-pointer hover:bg-accent-bg transition-colors">
        <component :is="getIcon(item.category)" class="w-8 h-8 text-text-secondary opacity-40" />
      </div>
      <div class="flex items-center justify-between px-0.5">
        <div class="min-w-0">
          <p class="text-[12px] font-medium text-text-primary truncate">{{ item.name }}</p>
          <p class="text-[10px] text-text-secondary">{{ item.formattedSize }}</p>
        </div>
        <button 
          @click.stop="activeMenu = activeMenu === item.id ? null : item.id"
          class="transition-opacity"
          :class="activeMenu === item.id ? 'opacity-100' : 'opacity-0 group-hover:opacity-100'"
        >
          <MoreHorizontal class="w-3.5 h-3.5 text-text-secondary" />
        </button>
      </div>

      <!-- Pop-up Menu -->
      <div 
        v-if="activeMenu === item.id"
        class="absolute right-0 top-12 w-32 bg-surface border border-border rounded-lg shadow-xl z-10 py-1"
      >
        <button @click="fileStore.downloadFile(item); activeMenu = null" class="w-full text-left px-4 py-2 text-[12px] hover:bg-canvas">Скачать</button>
        <button @click="fileStore.deleteFile(item.id); activeMenu = null" class="w-full text-left px-4 py-2 text-[12px] text-red-500 hover:bg-canvas">Удалить</button>
      </div>
    </div>

    <!-- Empty state -->
    <div 
      v-if="fileStore.currentItems.length === 0"
      class="col-span-full py-20 flex flex-col items-center border border-dashed border-border rounded-xl"
    >
      <Folder class="w-10 h-10 text-border mb-2" />
      <p class="text-[12px] text-text-secondary">This directory is empty</p>
    </div>
  </div>
</template>
