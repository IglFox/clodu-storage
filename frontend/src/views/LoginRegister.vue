<script setup>
import { ref } from "vue";
import { useAuthStore } from "../stores/auth";
import { useRouter } from "vue-router";
import { Cloud, Eye, EyeOff } from "@lucide/vue";

const authStore = useAuthStore();
const router = useRouter();
const isLoginMode = ref(true);
const email = ref("");
const password = ref("");
const name = ref("");
const showPassword = ref(false);

async function handleSubmit() {
    let success = false;
    if (isLoginMode.value) {
        success = await authStore.login(email.value, password.value);
    } else {
        success = await authStore.register(
            name.value,
            email.value,
            password.value,
        );
    }

    if (success && authStore.currentUser) {
        router.push("/");
    }
}
</script>

<template>
    <div
        class="min-h-screen w-full flex items-center justify-center bg-canvas font-sans selection:bg-folder selection:text-white"
    >
        <div class="w-full max-w-[360px] animate-fade-fast">
            <div class="flex flex-col items-center mb-10">
                <div
                    class="w-12 h-12 bg-folder rounded-xl flex items-center justify-center mb-6"
                >
                    <Cloud class="w-6 h-6 text-white opacity-90" />
                </div>
                <h1
                    class="text-xl font-normal text-text-primary tracking-tight"
                >
                    Enterprise Storage
                </h1>
                <p class="text-xs text-text-secondary mt-1.5 font-normal">
                    Sign in to access your secure space
                </p>
            </div>

            <div
                class="bg-surface border border-border rounded-xl p-8 shadow-sm"
            >
                <form @submit.prevent="handleSubmit" class="space-y-5">
                    <div v-if="!isLoginMode" class="space-y-1.5">
                        <label
                            class="text-[11px] font-medium text-text-secondary uppercase tracking-widest ml-1"
                            >Full Name</label
                        >
                        <input
                            v-model="name"
                            type="text"
                            placeholder="John Doe"
                            class="input-neutral"
                        />
                    </div>

                    <div class="space-y-1.5">
                        <label
                            class="text-[11px] font-medium text-text-secondary uppercase tracking-widest ml-1"
                            >Email address</label
                        >
                        <input
                            v-model="email"
                            type="email"
                            placeholder="name@company.com"
                            class="input-neutral"
                        />
                    </div>

                    <div class="space-y-1.5">
                        <label
                            class="text-[11px] font-medium text-text-secondary uppercase tracking-widest ml-1"
                            >Password</label
                        >
                        <div class="relative">
                            <input
                                v-model="password"
                                :type="showPassword ? 'text' : 'password'"
                                placeholder="••••••••"
                                class="input-neutral font-mono"
                            />
                            <button
                                type="button"
                                @click="showPassword = !showPassword"
                                class="absolute inset-y-0 right-0 pr-3 flex items-center text-text-secondary hover:text-text-primary transition-colors"
                            >
                                <Eye
                                    v-if="!showPassword"
                                    class="w-3.5 h-3.5 opacity-50"
                                />
                                <EyeOff v-else class="w-3.5 h-3.5 opacity-50" />
                            </button>
                        </div>
                    </div>

                    <button
                        type="submit"
                        :disabled="authStore.isLoading"
                        class="w-full h-10 bg-folder text-white text-[13px] font-medium rounded-lg hover:opacity-90 active:scale-[0.98] transition-all disabled:opacity-50 mt-2"
                    >
                        {{
                            authStore.isLoading
                                ? "Processing..."
                                : isLoginMode
                                  ? "Sign In"
                                  : "Create Account"
                        }}
                    </button>
                </form>

                <div class="mt-6 pt-6 border-t border-border text-center">
                    <button
                        @click="isLoginMode = !isLoginMode"
                        class="text-[12px] text-text-secondary hover:text-text-primary font-normal transition-colors"
                    >
                        {{
                            isLoginMode
                                ? "Don't have an account? Sign up"
                                : "Already have an account? Sign in"
                        }}
                    </button>
                </div>
            </div>

            <div
                v-if="authStore.authError"
                class="mt-4 p-3 bg-red-50 border border-red-100 rounded-lg text-red-600 text-[11px] text-center font-medium animate-fade-fast"
            >
                {{ authStore.authError }}
            </div>
        </div>
    </div>
</template>
