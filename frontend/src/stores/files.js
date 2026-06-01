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

  // Navigate to parent folder
  function navigateUp() {
    if (currentFolderId.value === 'root') return;
    const currentFolder = items.value.find(i => i.id === currentFolderId.value);
    if (!currentFolder) return navigateTo('root');
    
    const parentId = currentFolder.parentId || 'root';
    navigateTo(parentId);
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

  // Fetch all files
  async function fetchFiles() {
    try {
      const response = await api.get('/api/Files');
      const data = response.data || response;
      if (Array.isArray(data)) {
        items.value = data.map(file => ({
          id: file.id.toString(),
          name: file.name,
          type: 'file',
          parentId: 'root',
          category: 'File',
          size: file.sizeBytes,
          formattedSize: (file.sizeBytes / 1024).toFixed(2) + ' KB'
        }));
      }
    } catch (e) {
      console.error('Failed to fetch files:', e);
    }
  }

  // Upload file
  async function uploadFile(file) {
    const formData = new FormData();
    formData.append('file', file);
    
    try {
      await api.post('/api/Files/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      });
      await fetchFiles(); // Refresh list after upload
      return true;
    } catch (e) {
      console.error(e);
      return false;
    }
  }

  // Delete file
  async function deleteFile(fileId) {
    try {
      await api.delete(`/api/Files/${fileId}`);
      await fetchFiles();
      return true;
    } catch (e) {
      console.error('Failed to delete file:', e);
      return false;
    }
  }

  // Download file
  async function downloadFile(file) {
    try {
      const token = localStorage.getItem("dcs_auth_token");
      const response = await fetch(`/api/Files/${file.id}`, {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = file.name;
      document.body.appendChild(a);
      a.click();
      a.remove();
      window.URL.revokeObjectURL(url);
    } catch (e) {
      console.error('Failed to download file:', e);
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
    navigateUp,
    createFolder,
    deleteFile,
    uploadFile,
    fetchFiles,
    downloadFile
  };
});
