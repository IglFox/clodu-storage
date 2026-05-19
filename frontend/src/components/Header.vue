<script setup>
import { Search, Bell, ChevronRight } from 'lucide-vue-next';

defineProps({
  breadcrumbs: Array,
  userName: String
});

const emit = defineEmits(['openProfile']);
</script>

<template>
  <header class="flex justify-between items-center mb-8">
    <div class="flex items-center space-x-2 text-sm text-[#999]">
      <span v-for="(crumb, idx) in breadcrumbs" :key="idx" class="flex items-center">
        <span v-if="idx > 0" class="mx-2"><ChevronRight class="w-3 h-3" /></span>
        <span :class="{'text-black font-semibold': idx === breadcrumbs.length - 1}">{{ crumb }}</span>
      </span>
    </div>

    <div class="flex items-center space-x-4">
      <div class="relative">
        <Search class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-[#999]" />
        <input 
          type="text" 
          placeholder="Search vault..." 
          class="pl-10 pr-4 py-2 bg-white border border-[#E5E1DD] rounded-full text-sm focus:outline-none focus:border-black transition-colors w-64"
        />
      </div>
      <button class="w-10 h-10 bg-white border border-[#E5E1DD] rounded-full flex items-center justify-center hover:bg-[#F9F8F6] transition-colors">
        <Bell class="w-4 h-4" />
      </button>
      <button 
        @click="emit('openProfile')"
        class="w-10 h-10 rounded-full bg-black text-white flex items-center justify-center text-sm font-bold cursor-pointer hover:opacity-80 transition-opacity"
      >
        {{ userName.split(' ').map(n => n[0]).join('') }}
      </button>
    </div>
  </header>
</template>
