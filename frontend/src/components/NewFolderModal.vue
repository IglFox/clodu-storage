<template>
  <div class="modal-mask" @click.self="$emit('close')">
    <div class="modal-container">
      <h3>Новая папка</h3>
      <input type="text" v-model="folderName" placeholder="Название папки" @keyup.enter="create" />
      <div class="modal-actions">
        <button @click="$emit('close')">Отмена</button>
        <button @click="create">Создать</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';

const emit = defineEmits(['close', 'created']);
const folderName = ref('');

const create = () => {
  if (folderName.value.trim()) {
    emit('created', folderName.value.trim());
    folderName.value = '';
  }
};
</script>

<style scoped>
.modal-mask {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0,0,0,0.6);
  backdrop-filter: blur(4px);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 200;
}
.modal-container {
  background: var(--bg-card);
  border-radius: var(--radius);
  padding: 1.5rem;
  width: 300px;
}
.modal-container input {
  width: 100%;
  padding: 0.6rem;
  margin: 1rem 0;
  background: var(--bg-surface);
  border: 1px solid var(--border);
  border-radius: var(--radius-sm);
  color: var(--text);
}
.modal-actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
}
</style>
