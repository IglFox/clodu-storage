<script setup>
import { Globe } from "lucide-vue-next";

defineProps({
    settings: Array,
});
</script>

<template>
    <div class="animate-in fade-in slide-in-from-bottom-4 duration-500">
        <div class="mb-8 flex justify-between items-end">
            <div>
                <h2 class="text-2xl font-bold">System Configuration</h2>
                <p class="text-[#666] mt-1">
                    Infrastructure settings and service parameters.
                </p>
            </div>
            <button
                class="px-6 py-2 bg-black text-white rounded-lg text-sm font-bold shadow-lg shadow-black/10 hover:opacity-90"
            >
                Apply Changes
            </button>
        </div>

        <div class="grid grid-cols-2 gap-6">
            <div
                v-for="item in settings"
                :key="item.id"
                class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm"
            >
                <div class="flex items-center space-x-4 mb-4">
                    <div class="p-2 bg-[#F2F0ED] rounded-lg">
                        <component :is="item.icon" class="w-5 h-5 text-black" />
                    </div>
                    <h3 class="font-bold text-sm">{{ item.name }}</h3>
                </div>
                <p class="text-2xl font-bold mb-2">{{ item.value }}</p>
                <p class="text-xs text-[#999] leading-relaxed">
                    {{ item.description }}
                </p>
                <button
                    class="mt-4 text-xs font-bold text-black border-b border-black pb-0.5"
                >
                    Manage Details
                </button>
            </div>

            <div
                class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm flex flex-col justify-between"
            >
                <div>
                    <div class="flex items-center space-x-4 mb-4">
                        <div class="p-2 bg-[#F2F0ED] rounded-lg">
                            <Globe class="w-5 h-5 text-black" />
                        </div>
                        <h3 class="font-bold text-sm">Webhooks</h3>
                    </div>
                    <p class="text-xs text-[#999] mb-4">
                        Trigger external services when files are modified.
                    </p>
                </div>
                <div class="flex space-x-2">
                    <div class="flex-1 h-1 bg-[#F2F0ED] rounded-full"></div>
                    <div class="flex-1 h-1 bg-[#F2F0ED] rounded-full"></div>
                    <div class="flex-1 h-1 bg-black rounded-full"></div>
                </div>
                <button
                    class="mt-4 text-xs font-bold text-black border-b border-black pb-0.5 w-max"
                >
                    Configure Hooks
                </button>
            </div>
        </div>
    </div>
</template>
