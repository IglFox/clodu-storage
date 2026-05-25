import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { api, globalAvgLatency } from '../utils/api';

export const useFileStore = defineStore('files', () => {
  const currentFolderId = ref('root');
  
  // Breadcrumb navigation tracking
  const pathHistory = ref([{ id: 'root', name: 'Root Space' }]);
  
  const items = ref([]);
  const transfers = ref([]);

  const avgLatency = computed(() => {
    return (globalAvgLatency.value || 0) + 'ms';
  });

  // Global statistics
  const totalFilesCount = computed(() => items.value.filter(i => i.type === 'file').length);
  const totalFoldersCount = computed(() => items.value.filter(i => i.type === 'folder').length);

  // Computed: items filtered by current directory
  const currentItems = computed(() => {
    return items.value.filter(item => item.parentId === currentFolderId.value);
  });

  // Computed: items count in current folder
  const currentFolderStats = computed(() => {
    const list = currentItems.value;
    const foldersCount = list.filter(item => item.type === 'folder').length;
    const filesCount = list.filter(item => item.type === 'file').length;
    return { foldersCount, filesCount };
  });

  // Search filter query
  const searchQuery = ref('');
  const searchResults = computed(() => {
    if (!searchQuery.value) return [];
    return items.value.filter(item => 
      item.name.toLowerCase().includes(searchQuery.value.toLowerCase())
    );
  });

  // Total allocated space used by files
  const totalStorageUsed = computed(() => {
    return items.value
      .filter(item => item.type === 'file')
      .reduce((sum, file) => sum + (file.size || 0), 0);
  });

  // Navigate deeper into directories
  function navigateTo(folderId) {
    if (folderId === 'root') {
      currentFolderId.value = 'root';
      pathHistory.value = [{ id: 'root', name: 'Root Space' }];
      return;
    }
    
    const folder = items.value.find(item => item.id === folderId && item.type === 'folder');
    if (!folder) return;

    currentFolderId.value = folderId;
    
    // Reconstruct breadcrumbs tree
    const newPath = [];
    let current = folder;
    while (current) {
      newPath.unshift({ id: current.id, name: current.name });
      if (current.parentId === 'root') {
        break;
      }
      current = items.value.find(item => item.id === current.parentId && item.type === 'folder');
    }
    newPath.unshift({ id: 'root', name: 'Root Space' });
    pathHistory.value = newPath;
  }

  // Create folder inside current directory
  function createFolder(name) {
    if (!name) return false;
    
    const newFolder = {
      id: `folder_${Date.now()}`,
      name,
      type: 'folder',
      parentId: currentFolderId.value,
      createdAt: new Date().toISOString().split('T')[0]
    };
    
    items.value.push(newFolder);
    return true;
  }

  // Delete folder/file
  function deleteItem(itemId) {
    const idx = items.value.findIndex(item => item.id === itemId);
    if (idx === -1) return false;
    
    // Recursive deleting for subfolders
    const item = items.value[idx];
    if (item.type === 'folder') {
      const childs = items.value.filter(c => c.parentId === itemId);
      childs.forEach(c => deleteItem(c.id));
    }
    
    items.value.splice(idx, 1);
    return true;
  }

  // Upload file
  async function uploadFile(file) {
    const formData = new FormData();
    formData.append('file', file);
    
    try {
      await api.post('/api/Files/upload', formData, true);
      return true;
    } catch (e) {
      console.error(e);
      return false;
    }
  }

  return {
    currentFolderId,
    pathHistory,
    items,
    transfers,
    avgLatency,
    totalFilesCount,
    totalFoldersCount,
    currentItems,
    currentFolderStats,
    searchQuery,
    searchResults,
    totalStorageUsed,
    navigateTo,
    createFolder,
    deleteItem,
    uploadFile
  };
});
