<script setup>
import { Camera, Mail, ShieldCheck, LogOut } from "lucide-vue-next";

defineProps({
    user: Object,
});

const emit = defineEmits(["logout", "update"]);
</script>

<template>
    <div class="animate-in fade-in slide-in-from-bottom-4 duration-500">
        <div class="mb-8">
            <h2 class="text-2xl font-bold">User Profile</h2>
            <p class="text-[#666] mt-1">
                Manage your personal information and account preferences.
            </p>
        </div>
    </div>
    <div class="grid grid-cols-3 gap-8">
        <div class="col-span-1 space-y-6">
            <div
                class="bg-white border border-[#E5E1DD] rounded-2xl p-8 text-center shadow-sm"
            >
                <div class="relative w-32 h-32 mx-auto mb-6">
                    <div
                        class="w-full h-full rounded-2xl bg-[#F2F0ED] flex items-center justify-center text-4xl font-bold italic serif"
                    >
                        {{
                            user.name
                                .split(" ")
                                .map((n) => n[0])
                                .join("")
                        }}
                    </div>
                    <button
                        class="absolute -bottom-2 -right-2 w-10 h-10 bg-black text-white rounded-xl flex items-center justify-center shadow-lg border-4 border-white hover:scale-105 transition-transform"
                    >
                        <Camera class="w-4 h-4" />
                    </button>
                </div>
                <h3 class="text-xl font-bold">{{ user.name }}</h3>
                <p class="text-sm text-[#999] mb-6">{{ user.role }}</p>

                <div class="pt-6 border-t border-[#F2F0ED] space-y-4 text-left">
                    <div class="flex items-center space-x-3 text-sm">
                        <Mail class="w-4 h-4 text-[#999]" />
                        <span class="text-[#666]">{{ user.email }}</span>
                    </div>
                    <div class="flex items-center space-x-3 text-sm">
                        <ShieldCheck class="w-4 h-4 text-[#999]" />
                        <span class="text-green-600 font-medium"
                            >Verified Account</span
                        >
                    </div>
                </div>

                <button
                    @click="emit('logout')"
                    class="w-full mt-8 py-3 bg-[#FFF0F0] text-[#FF4444] rounded-xl text-sm font-bold flex items-center justify-center space-x-2 hover:bg-[#FFE5E5] transition-colors"
                >
                    <LogOut class="w-4 h-4" />
                    <span>Sign Out</span>
                </button>
            </div>
        </div>

        <div class="col-span-2 space-y-6">
            <div
                class="bg-white border border-[#E5E1DD] rounded-2xl p-8 shadow-sm"
            >
                <h3 class="text-lg font-bold mb-6">Account Settings</h3>
                <div class="space-y-6">
                    <div class="grid grid-cols-2 gap-6">
                        <div class="space-y-2">
                            <label
                                class="text-xs font-bold uppercase tracking-wider text-[#999]"
                                >Full Name</label
                            >
                            <input
                                type="text"
                                v-model="user.name"
                                class="w-full px-4 py-2.5 bg-white border border-[#E5E1DD] rounded-lg text-sm focus:outline-none focus:border-black"
                            />
                        </div>
                        <div class="space-y-2">
                            <label
                                class="text-xs font-bold uppercase tracking-wider text-[#999]"
                                >Email Address</label
                            >
                            <input
                                type="email"
                                v-model="user.email"
                                class="w-full px-4 py-2.5 bg-white border border-[#E5E1DD] rounded-lg text-sm focus:outline-none focus:border-black"
                            />
                        </div>
                    </div>
                    <div class="pt-6 border-t border-[#F2F0ED]"></div>

                    <div
                        class="mt-8 pt-6 border-t border-[#F2F0ED] flex justify-end"
                    >
                        <button
                            class="px-6 py-2.5 bg-black text-white rounded-lg text-sm font-bold shadow-lg shadow-black/10 hover:opacity-90 transition-opacity"
                        >
                            Save Changes
                        </button>
                    </div>
                </div>

                <div
                    class="bg-black text-white border border-black rounded-2xl p-8 shadow-sm"
                >
                    <div class="flex items-start justify-between">
                        <div>
                            <h3 class="text-lg font-bold">
                                Two-Factor Authentication
                            </h3>
                            <p class="text-white/60 text-sm mt-1 max-w-sm">
                                Secure your account with an extra layer of
                                protection during login.
                            </p>
                        </div>
                        <span
                            class="bg-green-400/20 text-green-400 px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider"
                            >Active</span
                        >
                    </div>
                    <button
                        class="mt-6 text-sm font-bold border-b border-white pb-1 hover:border-white/60 transition-colors"
                    >
                        Manage 2FA Settings
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>
