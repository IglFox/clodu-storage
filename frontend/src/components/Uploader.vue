<template>
  <button class="upload-btn" @click="triggerUpload">📤 Загрузить</button>
  <input type="file" ref="fileInput" style="display:none" multiple @change="onFileChange" />
</template>

<script setup>
import { ref } from 'vue';

const emit = defineEmits(['upload']);
const fileInput = ref(null);

const triggerUpload = () => {
  fileInput.value.click();
};

const onFileChange = (e) => {
  Array.from(e.target.files).forEach(file => emit('upload', file));
  e.target.value = '';
};
</script>

<style scoped>
.upload-btn {
  padding: 0.4rem 0.8rem;
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: 40px;
  cursor: pointer;
}
</style>
