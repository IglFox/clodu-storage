<script setup>
import {
    HardDrive,
    Layers,
    Folder,
    Shield,
    Settings,
    User,
} from "lucide-vue-next";

defineProps({
    currentView: String,
    storagePercent: Number,
    storageUsed: Number,
    storageTotal: Number,
});

const emit = defineEmits(["navigate"]);
</script>

<template>
    <aside
        class="fixed left-0 top-0 h-full w-64 border-r border-[#E5E1DD] bg-white z-50 transition-all duration-300"
    >
        <div class="p-6 flex items-center space-x-3">
            <div
                class="w-8 h-8 bg-black rounded-lg flex items-center justify-center"
            >
                <HardDrive class="text-white w-5 h-5" />
            </div>
            <span class="font-bold text-xl tracking-tight italic serif"
                >CloudVault</span
            >
        </div>

        <nav class="mt-4 px-4 space-y-1">
            <button
                @click="emit('navigate', 'dashboard')"
                :class="[
                    'w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors',
                    currentView === 'dashboard'
                        ? 'bg-[#F2F0ED] text-black'
                        : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black',
                ]"
            >
                <Layers class="w-4 h-4" />
                <span>Dashboard</span>
            </button>
            <button
                @click="emit('navigate', 'files')"
                :class="[
                    'w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors',
                    currentView === 'files'
                        ? 'bg-[#F2F0ED] text-black'
                        : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black',
                ]"
            >
                <Folder class="w-4 h-4" />
                <span>My Storage</span>
            </button>
            <button
                @click="emit('navigate', 'security')"
                :class="[
                    'w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors',
                    currentView === 'security'
                        ? 'bg-[#F2F0ED] text-black'
                        : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black',
                ]"
            >
                <Shield class="w-4 h-4" />
                <span>Security</span>
            </button>
            <button
                @click="emit('navigate', 'configuration')"
                :class="[
                    'w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors',
                    currentView === 'configuration'
                        ? 'bg-[#F2F0ED] text-black'
                        : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black',
                ]"
            >
                <Settings class="w-4 h-4" />
                <span>Configuration</span>
            </button>
            <button
                @click="emit('navigate', 'profile')"
                :class="[
                    'w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors',
                    currentView === 'profile'
                        ? 'bg-[#F2F0ED] text-black'
                        : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black',
                ]"
            >
                <User class="w-4 h-4" />
                <span>Profile</span>
            </button>
        </nav>

        <div
            class="absolute bottom-6 left-6 right-6 p-4 bg-[#F9F8F6] rounded-xl border border-[#E5E1DD]"
        >
            <div
                class="flex justify-between text-xs font-semibold uppercase tracking-wider text-[#999] mb-2"
            >
                <span>Used Space</span>
                <span>{{ storagePercent }}%</span>
            </div>
            <div class="h-1.5 w-full bg-[#E5E1DD] rounded-full overflow-hidden">
                <div
                    class="h-full bg-black rounded-full"
                    :style="{ width: storagePercent + '%' }"
                ></div>
            </div>
            <div class="mt-2 text-xs text-[#666]">
                {{ storageUsed }}GB of {{ storageTotal }}GB used
            </div>
        </div>
    </aside>
</template>
