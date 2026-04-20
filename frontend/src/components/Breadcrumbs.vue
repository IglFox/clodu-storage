<template>
  <div class="breadcrumbs">
    <span class="crumb" @click="$emit('navigate', 'root')">📁 Мои файлы</span>
    <template v-for="(crumb, idx) in crumbs" :key="crumb.id">
      <span> / </span>
      <span class="crumb" :class="{ active: idx === crumbs.length-1 }" @click="$emit('navigate', crumb.id)">
        {{ crumb.name }}
      </span>
    </template>
  </div>
</template>

<script setup>
defineProps(['crumbs']);
defineEmits(['navigate']);
</script>

<style scoped>
.breadcrumbs {
  background: var(--bg-surface);
  padding: 0.4rem 1rem;
  border-radius: 40px;
  font-size: 0.85rem;
}
.crumb {
  cursor: pointer;
  color: var(--text-muted);
}
.crumb.active {
  color: var(--accent);
}
</style>
