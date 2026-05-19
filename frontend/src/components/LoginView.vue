<script setup>
import { ref } from 'vue';
import { HardDrive, Mail, Lock, ChevronRight, Shield, Globe } from 'lucide-vue-next';

const emit = defineEmits(['login']);
const loginForm = ref({ email: '', password: '' });

const handleLogin = () => {
  if (loginForm.value.email && loginForm.value.password) {
    emit('login', { ...loginForm.value });
  }
};
</script>

<template>
  <div class="min-h-screen flex items-center justify-center p-6 bg-[#FDFCFB]">
    <div class="max-w-md w-full bg-white border border-[#E5E1DD] rounded-3xl p-10 shadow-xl shadow-black/5 animate-in fade-in slide-in-from-bottom-8 duration-700">
      <div class="text-center mb-10">
        <div class="w-16 h-16 bg-black rounded-2xl flex items-center justify-center mx-auto mb-6 rotate-3">
          <HardDrive class="text-white w-8 h-8" />
        </div>
        <h1 class="text-3xl font-bold tracking-tight italic serif mb-2">CloudVault</h1>
        <p class="text-[#666] text-sm">Secure enterprise storage microservice</p>
      </div>

      <form @submit.prevent="handleLogin" class="space-y-6">
        <div class="space-y-2">
          <label class="text-xs font-bold uppercase tracking-wider text-[#999] ml-1">Email Intelligence Access</label>
          <div class="relative">
            <Mail class="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[#999]" />
            <input 
              v-model="loginForm.email"
              type="email" 
              placeholder="administrator@cloudvault.io"
              class="w-full pl-12 pr-4 py-4 bg-[#F9F8F6] border border-[#E5E1DD] rounded-2xl text-sm focus:outline-none focus:border-black focus:bg-white transition-all"
              required
            />
          </div>
        </div>

        <div class="space-y-2">
          <label class="text-xs font-bold uppercase tracking-wider text-[#999] ml-1">Vault Key (Password)</label>
          <div class="relative">
            <Lock class="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-[#999]" />
            <input 
              v-model="loginForm.password"
              type="password" 
              placeholder="••••••••••••"
              class="w-full pl-12 pr-4 py-4 bg-[#F9F8F6] border border-[#E5E1DD] rounded-2xl text-sm focus:outline-none focus:border-black focus:bg-white transition-all"
              required
            />
          </div>
        </div>

        <div class="flex items-center justify-between py-2 text-xs font-medium">
          <label class="flex items-center space-x-2 cursor-pointer">
            <input type="checkbox" class="w-4 h-4 rounded border-[#E5E1DD] text-black focus:ring-0" />
            <span class="text-[#666]">Trust this explorer</span>
          </label>
          <a href="#" class="text-black underline underline-offset-4 hover:no-underline">Lost Key?</a>
        </div>

        <button 
          type="submit"
          class="w-full py-4 bg-black text-white rounded-2xl font-bold text-sm shadow-lg shadow-black/20 hover:opacity-90 active:scale-[0.98] transition-all flex items-center justify-center space-x-2"
        >
          <span>Decrypt & Access</span>
          <ChevronRight class="w-4 h-4" />
        </button>
      </form>

      <div class="mt-10 pt-8 border-t border-[#F2F0ED] flex items-center justify-center space-x-6">
        <div class="flex items-center space-x-2 text-[10px] text-[#999] uppercase font-bold tracking-widest">
          <Shield class="w-3 h-3" />
          <span>Encrypted</span>
        </div>
        <div class="w-1 h-1 rounded-full bg-[#E5E1DD]"></div>
        <div class="flex items-center space-x-2 text-[10px] text-[#999] uppercase font-bold tracking-widest">
          <Globe class="w-3 h-3" />
          <span>Nodes: 24</span>
        </div>
      </div>
    </div>
  </div>
</template>
