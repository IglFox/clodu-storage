<script setup>
import { Folder, File, Plus, Upload, MoreVertical } from "lucide-vue-next";

defineProps({
    files: Array,
});
</script>

<template>
    <div class="animate-in fade-in slide-in-from-bottom-4 duration-500">
        <div class="flex justify-between items-center mb-6">
            <h2 class="text-2xl font-bold">My Storage</h2>
            <div class="flex space-x-3">
                <button
                    class="px-4 py-2 border border-[#E5E1DD] rounded-lg inline-flex items-center space-x-2 text-sm font-medium hover:bg-[#F9F8F6]"
                >
                    <Plus class="w-4 h-4" />
                    <span>New Folder</span>
                </button>
                <div class="relative inline-block">
                    <input
                        type="file"
                        class="hidden"
                        @change="onFileSelected"
                    />
                    <button
                        class="px-4 py-2 bg-black text-white rounded-lg inline-flex items-center space-x-2 text-sm font-medium hover:opacity-90"
                        @click="$el.previousElementSibling.click()"
                    >
                        <Upload class="w-4 h-4" />
                        <span>Upload File</span>
                    </button>
                </div>
            </div>
        </div>

        <div
            class="bg-white border border-[#E5E1DD] rounded-2xl overflow-hidden shadow-sm"
        >
            <table class="w-full text-left">
                <thead class="bg-[#FDFCFB] border-bottom border-[#E5E1DD]">
                    <tr>
                        <th
                            class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]"
                        >
                            Name
                        </th>
                        <th
                            class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]"
                        >
                            Size
                        </th>
                        <th
                            class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]"
                        >
                            Type
                        </th>
                        <th
                            class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]"
                        >
                            Modified
                        </th>
                        <th
                            class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]"
                        ></th>
                    </tr>
                </thead>
                <tbody class="divide-y divide-[#E5E1DD]">
                    <tr
                        v-for="file in files"
                        :key="file.id"
                        class="hover:bg-[#F9F8F6] group transition-colors"
                    >
                        <td class="px-6 py-4">
                            <div class="flex items-center space-x-3">
                                <Folder
                                    v-if="file.type === 'folder'"
                                    class="w-5 h-5 text-amber-400 fill-amber-100"
                                />
                                <File
                                    v-else
                                    class="w-5 h-5 text-blue-400 fill-blue-50"
                                />
                                <span class="font-medium text-sm">{{
                                    file.name
                                }}</span>
                            </div>
                        </td>
                        <td class="px-6 py-4 text-sm text-[#666]">
                            {{ file.size }}
                        </td>
                        <td class="px-6 py-4 text-sm text-[#666]">
                            <span
                                v-if="file.type === 'folder'"
                                class="bg-[#F2F0ED] px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-tighter"
                                >{{ file.items }} items</span
                            >
                            <span
                                v-else
                                class="text-[10px] font-bold uppercase tracking-tighter"
                                >{{ file.ext }}</span
                            >
                        </td>
                        <td class="px-6 py-4 text-sm text-[#666]">
                            {{ file.date }}
                        </td>
                        <td class="px-6 py-4 text-right">
                            <button
                                class="opacity-0 group-hover:opacity-100 p-1 hover:bg-[#E5E1DD] rounded transition-all"
                            >
                                <MoreVertical class="w-4 h-4 text-[#666]" />
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</template>
