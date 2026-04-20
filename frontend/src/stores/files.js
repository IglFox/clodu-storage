import { defineStore } from 'pinia';
import * as storageApi from '../api/storage';

export const useFileStore = defineStore('files', {
  state: () => ({
    items: [],            // все файлы/папки
    currentFolderId: 'root',
    searchQuery: '',
    loading: false,
  }),
  getters: {
    currentItems(state) {
      let items = state.items.filter(i => i.parentId === state.currentFolderId);
      if (state.searchQuery) {
        const q = state.searchQuery.toLowerCase();
        items = items.filter(i => i.name.toLowerCase().includes(q));
      }
      return items;
    },
    breadcrumbs(state) {
      const crumbs = [];
      let curId = state.currentFolderId;
      const folderMap = new Map(state.items.filter(f => f.type === 'folder').map(f => [f.id, f]));
      while (curId && curId !== 'root') {
        const folder = folderMap.get(curId);
        if (folder) {
          crumbs.unshift({ id: folder.id, name: folder.name });
          curId = folder.parentId;
        } else break;
      }
      return crumbs;
    },
    totalFiles(state) {
      return state.items.filter(f => f.type === 'file').length;
    },
    totalFolders(state) {
      return state.items.filter(f => f.type === 'folder' && f.id !== 'root').length;
    },
    usedSpaceGB(state) {
      const bytes = state.items.filter(f => f.type === 'file').reduce((sum, f) => sum + f.size, 0);
      return bytes / (1024 ** 3);
    },
    usagePercent() {
      return (this.usedSpaceGB / 10) * 100;
    },
    largeFiles(state) {
      return state.items.filter(f => f.type === 'file' && f.size > 50 * 1024 * 1024)
        .sort((a,b) => b.size - a.size);
    },
  },
  actions: {
    async loadFiles() {
      this.loading = true;
      const data = await storageApi.fetchAllFiles();
      this.items = data;
      this.loading = false;
    },
    async createFolder(name) {
      await storageApi.createFolder(name, this.currentFolderId);
      await this.loadFiles();
    },
    async deleteItem(id) {
      await storageApi.deleteItem(id);
      if (this.currentFolderId === id) this.currentFolderId = 'root';
      await this.loadFiles();
    },
    async upload(file) {
      await storageApi.uploadFile(file, this.currentFolderId);
      await this.loadFiles();
    },
    setCurrentFolder(id) {
      this.currentFolderId = id;
      this.searchQuery = '';
    },
    setSearchQuery(query) {
      this.searchQuery = query;
    },
  },
});
