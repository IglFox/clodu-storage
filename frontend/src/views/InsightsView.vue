<template>
  <div>
    <div class="stats-row">
      <div class="chart-card">
        <h3>📊 Использование по типам</h3>
        <StorageChart :type-stats="typeStats" />
      </div>
      <div class="chart-card">
        <h3>💾 Детализация</h3>
        <div class="type-list">
          <div
            v-for="stat in typeStatsArray"
            :key="stat.label"
            class="type-item"
          >
            <span>{{ stat.icon }} {{ stat.label }}</span>
            <span>{{ stat.value.toFixed(2) }} GB</span>
          </div>
        </div>
        <div class="progress-bar">
          <div
            class="progress-fill"
            :style="{ width: fileStore.usagePercent + '%' }"
          ></div>
        </div>
        <p>Использовано {{ fileStore.usedSpaceGB.toFixed(1) }} GB из 10 GB</p>
      </div>
    </div>
    <div class="chart-card" style="margin-top: 1rem">
      <h3>📁 Крупные файлы (>50 МБ)</h3>
      <div v-if="fileStore.largeFiles.length === 0" class="empty-state">
        Нет крупных файлов
      </div>
      <div
        v-for="file in fileStore.largeFiles"
        :key="file.id"
        class="type-item"
      >
        <span>{{ file.name }}</span>
        <span>{{ formatSize(file.size) }}</span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { useFileStore } from "../stores/files";
import StorageChart from "../components/StorageChart.vue";
import { formatSize } from "../utils/helpers";

const fileStore = useFileStore();

const typeStats = computed(() => {
  let docs = 0,
    images = 0,
    media = 0,
    archives = 0,
    other = 0;
  fileStore.items.forEach((f) => {
    if (f.type !== "file") return;
    const ext = f.name.split(".").pop().toLowerCase();
    const sizeGb = f.size / 1024 ** 3;
    if (["pdf", "doc", "docx", "txt", "md", "xlsx"].includes(ext))
      docs += sizeGb;
    else if (["jpg", "jpeg", "png", "gif"].includes(ext)) images += sizeGb;
    else if (["mp4", "mkv", "mp3", "wav"].includes(ext)) media += sizeGb;
    else if (["zip", "rar", "7z"].includes(ext)) archives += sizeGb;
    else other += sizeGb;
  });
  return { documents: docs, images, media, archives, other };
});

const typeStatsArray = computed(() => [
  { label: "Документы", icon: "📄", value: typeStats.value.documents },
  { label: "Изображения", icon: "🖼️", value: typeStats.value.images },
  { label: "Медиа", icon: "🎬", value: typeStats.value.media },
  { label: "Архивы", icon: "🗜️", value: typeStats.value.archives },
  { label: "Прочее", icon: "📁", value: typeStats.value.other },
]);
</script>

<style scoped>
.stats-row {
  display: grid;
  grid-template-columns: 1fr 1.2fr;
  gap: 1rem;
  margin-bottom: 1rem;
}
.chart-card {
  background: var(--bg-card);
  border: 1px solid var(--border);
  border-radius: var(--radius);
  padding: 1rem;
}
.type-list {
  margin-top: 1rem;
}
.type-item {
  display: flex;
  justify-content: space-between;
  padding: 0.5rem 0;
  border-bottom: 1px solid var(--border-subtle);
}
.progress-bar {
  height: 6px;
  background: var(--border);
  border-radius: 10px;
  margin: 1rem 0;
}
.progress-fill {
  height: 100%;
  background: var(--accent);
  border-radius: 10px;
}
.empty-state {
  text-align: center;
  padding: 1rem;
  color: var(--text-faint);
}
</style>
