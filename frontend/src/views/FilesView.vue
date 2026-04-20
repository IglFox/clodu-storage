<template>
  <div>
    <KpiCards :total-files="fileStore.totalFiles" :total-folders="fileStore.totalFolders"
              :used-space="fileStore.usedSpaceGB" :usage-percent="fileStore.usagePercent" />

    <div class="toolbar">
      <Breadcrumbs :crumbs="fileStore.breadcrumbs" @navigate="fileStore.setCurrentFolder" />
      <div class="actions">
        <Uploader @upload="handleUpload" />
        <button class="btn-icon" @click="openNewFolderModal">➕ Папка</button>
        <input type="text" v-model="searchQuery" placeholder="Поиск..." class="search-input" />
      </div>
    </div>

    <FileGrid :items="fileStore.currentItems" @open="openItem" @delete="fileStore.deleteItem" />

    <NewFolderModal v-if="showModal" @close="showModal = false" @created="handleFolderCreated" />
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import { useFileStore } from '../stores/files';
import KpiCards from '../components/KpiCards.vue';
import Breadcrumbs from '../components/Breadcrumbs.vue';
import FileGrid from '../components/FileGrid.vue';
import Uploader from '../components/Uploader.vue';
import NewFolderModal from '../components/NewFolderModal.vue';

const fileStore = useFileStore();
const showModal = ref(false);
const searchQuery = ref('');

watch(searchQuery, (val) => fileStore.setSearchQuery(val));

const openItem = (item) => {
  if (item.type === 'folder') {
    fileStore.setCurrentFolder(item.id);
  } else {
    alert(`Предпросмотр: ${item.name} (${formatSize(item.size)})`);
  }
};

const handleUpload = (file) => fileStore.upload(file);
const openNewFolderModal = () => { showModal.value = true; };
const handleFolderCreated = () => {
  showModal.value = false;
  fileStore.loadFiles();
};

// Загрузка при монтировании
fileStore.loadFiles();
</script>

<style scoped>
.toolbar {
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 1.5rem;
}
.actions {
  display: flex;
  gap: 0.5rem;
  align-items: center;
}
.search-input {
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 40px;
  padding: 0.4rem 0.8rem;
  color: var(--text);
  outline: none;
}
.btn-icon {
  padding: 0.4rem 0.8rem;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 40px;
  cursor: pointer;
}
</style>
