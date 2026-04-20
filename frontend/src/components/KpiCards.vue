<template>
  <div class="kpi-grid">
    <div class="kpi-card">
      <div class="kpi-label">Всего файлов</div>
      <div class="kpi-value">{{ totalFiles }}</div>
    </div>
    <div class="kpi-card">
      <div class="kpi-label">Папок</div>
      <div class="kpi-value">{{ totalFolders }}</div>
    </div>
    <div class="kpi-card">
      <div class="kpi-label">Использовано</div>
      <div class="kpi-value">{{ usedSpace.toFixed(1) }}<span class="kpi-unit">GB</span></div>
      <div class="progress-bar"><div class="progress-fill" :style="{ width: usagePercent + '%' }"></div></div>
    </div>
  </div>
</template>

<script setup>
defineProps(['totalFiles', 'totalFolders', 'usedSpace', 'usagePercent']);
</script>

<style scoped>
.kpi-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: 0.75rem;
  margin-bottom: 1.5rem;
}
.kpi-card {
  background: var(--bg-card);
  border: 1px solid var(--border-subtle);
  border-radius: var(--radius);
  padding: 1rem;
}
.kpi-label { font-size: 0.7rem; text-transform: uppercase; color: var(--text-muted); }
.kpi-value { font-size: 1.8rem; font-weight: 700; }
.kpi-unit { font-size: 0.8rem; color: var(--text-faint); }
.progress-bar { margin-top: 0.5rem; height: 5px; background: var(--border); border-radius: 10px; overflow: hidden; }
.progress-fill { height: 100%; background: var(--accent); width: 0%; }
</style>
