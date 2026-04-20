<template>
  <div class="file-grid">
    <div v-for="item in items" :key="item.id" class="file-card" @dblclick="$emit('open', item)">
      <div class="file-icon">{{ item.type === 'folder' ? '📁' : getFileIcon(item.name) }}</div>
      <div class="file-info">
        <div class="file-name">{{ item.name }}</div>
        <div class="file-meta">
          <span v-if="item.type !== 'folder'">{{ formatSize(item.size) }}</span>
          <span>{{ formatDate(item.modified) }}</span>
        </div>
      </div>
      <div class="file-actions">
        <button class="action-btn" @click.stop="$emit('delete', item.id)">🗑️</button>
      </div>
    </div>
    <div v-if="items.length === 0" class="empty-state">📂 Папка пуста</div>
  </div>
</template>

<script setup>
import { formatSize, formatDate, getFileIcon } from '../utils/helpers';

defineProps(['items']);
defineEmits(['open', 'delete']);
</script>

<style scoped>
.file-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 0.75rem;
}
.file-card {
  background: var(--bg-card);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius);
  padding: 0.9rem;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  cursor: pointer;
}
.file-card:hover {
  background: var(--bg-card-hover);
  transform: translateY(-2px);
}
.file-icon { font-size: 1.8rem; }
.file-info { flex: 1; }
.file-name { font-weight: 600; }
.file-meta { font-size: 0.7rem; color: var(--text-faint); }
.action-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1rem;
}
.empty-state { text-align: center; padding: 2rem; color: var(--text-faint); }
</style>
