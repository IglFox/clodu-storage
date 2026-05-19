<script setup>
import { Key } from 'lucide-vue-next';

defineProps({
  settings: Array
});
</script>

<template>
  <div class="animate-in fade-in slide-in-from-bottom-4 duration-500">
    <div class="mb-8">
      <h2 class="text-2xl font-bold">Security Center</h2>
      <p class="text-[#666] mt-1">Manage encryption, access control and audit logs.</p>
    </div>

    <div class="grid grid-cols-1 gap-6">
      <div 
        v-for="item in settings" 
        :key="item.id" 
        class="p-8 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm flex items-center justify-between hover:shadow-md transition-shadow"
      >
        <div class="flex items-center space-x-6">
          <div class="p-4 bg-[#F9F8F6] rounded-xl">
            <component :is="item.icon" class="w-8 h-8 text-black" />
          </div>
          <div>
            <h3 class="text-base font-bold">{{ item.name }}</h3>
            <p class="text-sm text-[#666] max-w-sm mt-1">{{ item.description }}</p>
          </div>
        </div>
        <div class="flex items-center space-x-4">
          <span :class="['text-xs font-bold uppercase py-1 px-3 rounded-full', item.status ? 'bg-green-100 text-green-700' : 'bg-[#E5E1DD] text-[#666]']">
            {{ item.status ? 'Enabled' : 'Disabled' }}
          </span>
          <div 
            class="w-12 h-6 rounded-full p-1 cursor-pointer transition-colors duration-200" 
            :class="item.status ? 'bg-black' : 'bg-[#E5E1DD]'"
            @click="item.status = !item.status"
          >
            <div 
              class="w-4 h-4 bg-white rounded-full transition-transform duration-200" 
              :style="{ transform: item.status ? 'translateX(24px)' : 'translateX(0)' }"
            ></div>
          </div>
        </div>
      </div>

      <div class="p-8 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm">
        <h3 class="text-lg font-bold mb-6 flex items-center">
          <Key class="w-5 h-5 mr-3" /> API Credentials
        </h3>
        <div class="space-y-4">
          <div class="p-4 bg-[#F9F8F6] rounded-xl flex items-center justify-between border border-[#E5E1DD]">
            <div>
              <p class="text-xs font-semibold text-[#999] uppercase tracking-wider">Master Key ID</p>
              <code class="text-sm font-mono mt-1 block">CV-8829-XLK-990-221</code>
            </div>
            <button class="text-xs font-bold underline hover:no-underline">Rotate Key</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
