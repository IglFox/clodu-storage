<script setup>
import { Database, Activity, Shield, MoreVertical } from "lucide-vue-next";

defineProps({
    stats: Object,
});
</script>

<template>
    <div
        class="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500"
    >
        <div class="grid grid-cols-3 gap-6">
            <div
                class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm"
            >
                <div class="flex items-center justify-between mb-4">
                    <div class="p-2 bg-[#F2F0ED] rounded-lg">
                        <Database class="w-5 h-5" />
                    </div>
                    <MoreVertical class="w-4 h-4 text-[#999]" />
                </div>
                <h3 class="text-sm font-medium text-[#666]">Total Storage</h3>
                <p class="text-3xl font-bold mt-1">{{ stats.total }} GB</p>
            </div>
            <div
                class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm"
            >
                <div class="flex items-center justify-between mb-4">
                    <div class="p-2 bg-[#F2F0ED] rounded-lg">
                        <Activity class="w-5 h-5" />
                    </div>
                    <MoreVertical class="w-4 h-4 text-[#999]" />
                </div>
                <h3 class="text-sm font-medium text-[#666]">
                    Avg Daily Upload
                </h3>
                <p class="text-3xl font-bold mt-1">1.2 GB</p>
            </div>
            <div
                class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm"
            >
                <div class="flex items-center justify-between mb-4">
                    <div class="p-2 bg-white/10 rounded-lg">
                        <Shield class="w-5 h-5" />
                    </div>
                    <MoreVertical class="w-4 h-4 text-white/40" />
                </div>
                <h3 class="text-sm font-medium text-[#666]">System Status</h3>
                <p
                    class="text-3xl font-bold mt-1 uppercase tracking-widest flex items-center"
                >
                    Secure
                    <span
                        class="ml-2 w-2 h-2 bg-green-400 rounded-full animate-pulse"
                    ></span>
                </p>
            </div>
        </div>

        <div class="bg-white border border-[#E5E1DD] rounded-2xl p-8">
            <h2 class="text-xl font-bold mb-6">Recent Activity</h2>
            <div class="space-y-6">
                <div v-for="i in 3" :key="i" class="flex items-start space-x-4">
                    <div class="mt-1 w-2 h-2 rounded-full bg-black"></div>
                    <div class="flex-1">
                        <p class="text-sm font-medium">
                            Backup completed successfully
                        </p>
                        <p class="text-xs text-[#999] mt-1">
                            Automatic snapshot created in Europe-West-2
                        </p>
                    </div>
                    <span class="text-xs text-[#999]">{{ i * 2 }}h ago</span>
                </div>
            </div>
        </div>
    </div>
</template>
