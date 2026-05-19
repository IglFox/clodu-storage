<script setup>
import { ref, computed } from 'vue';
import { 
  Folder, 
  File, 
  Upload, 
  Plus, 
  ShieldCheck, 
  Settings, 
  Search, 
  HardDrive, 
  Lock, 
  Globe, 
  Bell, 
  Database,
  ChevronRight,
  MoreVertical,
  Activity,
  Key,
  Shield,
  Zap,
  CreditCard,
  Layers,
  User,
  Mail,
  Camera,
  LogOut
} from 'lucide-vue-next';

// View Management
const isLoggedIn = ref(false);
const currentView = ref('dashboard');
const breadcrumbs = ref(['Dashboard']);
const loginForm = ref({ email: '', password: '' });

// Mock Data
const files = ref([
  { id: 1, name: 'Project Assets', type: 'folder', size: '-', date: '2026-05-18', items: 24 },
  { id: 2, name: 'Security_Policy.pdf', type: 'file', ext: 'PDF', size: '1.2 MB', date: '2026-05-19' },
  { id: 3, name: 'Backup_v2.zip', type: 'file', ext: 'ZIP', size: '4.5 GB', date: '2026-05-15' },
  { id: 4, name: 'Architecture_Diagram.svg', type: 'file', ext: 'SVG', size: '450 KB', date: '2026-05-17' },
]);

const storageStats = {
  used: 156.4,
  total: 512,
  percent: 30.5
};

const securitySettings = ref([
  { id: 'encryption', name: 'Encryption at Rest', description: 'AES-256 military-grade encryption for all files.', status: true, icon: Lock },
  { id: 'mfa', name: 'Multi-Factor Auth', description: 'Require 2FA for all account actions.', status: true, icon: ShieldCheck },
  { id: 'logs', name: 'Audit Logging', description: 'Track every file access and modification.', status: false, icon: Activity },
]);

const configSettings = ref([
  { id: 'region', name: 'Storage Region', value: 'Europe (London)', description: 'Select the primary data center location.', icon: Globe },
  { id: 'billing', name: 'Billing Method', value: 'Corporate Card', description: 'Manage payment and subscriptions.', icon: CreditCard },
  { id: 'perf', name: 'Transfer Acceleration', value: 'Enabled', description: 'Optimized routing for faster uploads.', icon: Zap },
]);

const userProfile = ref({
  name: 'Jane Doe',
  email: 'jane.doe@cloudvault.io',
  role: 'System Administrator',
  avatar: null,
  joinedDate: 'January 2024',
  twoFactorEnabled: true,
  notifications: {
    email: true,
    browser: true,
    security: true
  }
});

// Navigation & Auth Logic
const setView = (view) => {
  currentView.value = view;
  if (view === 'dashboard') breadcrumbs.value = ['Dashboard'];
  else if (view === 'files') breadcrumbs.value = ['My Storage'];
  else if (view === 'security') breadcrumbs.value = ['Security Center'];
  else if (view === 'configuration') breadcrumbs.value = ['System Configuration'];
  else if (view === 'profile') breadcrumbs.value = ['User Profile'];
};

const handleLogin = () => {
  // Simple mock login validation
  if (loginForm.value.email && loginForm.value.password) {
    isLoggedIn.value = true;
    setView('dashboard');
  }
};

const handleLogout = () => {
  isLoggedIn.value = false;
  loginForm.value = { email: '', password: '' };
};

</script>

<template>
  <div class="min-h-screen bg-[#FDFCFB] text-[#1A1A1A] font-sans selection:bg-[#FFDAB9]">
    
    <!-- Login Screen -->
    <div v-if="!isLoggedIn" class="min-h-screen flex items-center justify-center p-6 bg-[#FDFCFB]">
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

    <!-- Main App UI -->
    <template v-else>
      <!-- Sidebar -->
    <aside class="fixed left-0 top-0 h-full w-64 border-r border-[#E5E1DD] bg-white z-50 transition-all duration-300">
      <div class="p-6 flex items-center space-x-3">
        <div class="w-8 h-8 bg-black rounded-lg flex items-center justify-center">
          <HardDrive class="text-white w-5 h-5" />
        </div>
        <span class="font-bold text-xl tracking-tight italic serif">CloudVault</span>
      </div>

      <nav class="mt-4 px-4 space-y-1">
        <button 
          @click="setView('dashboard')" 
          :class="['w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors', 
          currentView === 'dashboard' ? 'bg-[#F2F0ED] text-black' : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black']"
        >
          <Layers class="w-4 h-4" />
          <span>Dashboard</span>
        </button>
        <button 
          @click="setView('files')" 
          :class="['w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors', 
          currentView === 'files' ? 'bg-[#F2F0ED] text-black' : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black']"
        >
          <Folder class="w-4 h-4" />
          <span>My Storage</span>
        </button>
        <button 
          @click="setView('security')" 
          :class="['w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors', 
          currentView === 'security' ? 'bg-[#F2F0ED] text-black' : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black']"
        >
          <Shield class="w-4 h-4" />
          <span>Security</span>
        </button>
        <button 
          @click="setView('configuration')" 
          :class="['w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors', 
          currentView === 'configuration' ? 'bg-[#F2F0ED] text-black' : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black']"
        >
          <Settings class="w-4 h-4" />
          <span>Configuration</span>
        </button>
        <button 
          @click="setView('profile')" 
          :class="['w-full flex items-center space-x-3 px-4 py-2.5 rounded-lg text-sm font-medium transition-colors', 
          currentView === 'profile' ? 'bg-[#F2F0ED] text-black' : 'text-[#666] hover:bg-[#F9F8F6] hover:text-black']"
        >
          <User class="w-4 h-4" />
          <span>Profile</span>
        </button>
      </nav>

      <!-- Storage Info -->
      <div class="absolute bottom-6 left-6 right-6 p-4 bg-[#F9F8F6] rounded-xl border border-[#E5E1DD]">
        <div class="flex justify-between text-xs font-semibold uppercase tracking-wider text-[#999] mb-2">
          <span>Used Space</span>
          <span>{{ storageStats.percent }}%</span>
        </div>
        <div class="h-1.5 w-full bg-[#E5E1DD] rounded-full overflow-hidden">
          <div class="h-full bg-black rounded-full" :style="{ width: storageStats.percent + '%' }"></div>
        </div>
        <div class="mt-2 text-xs text-[#666]">
          {{ storageStats.used }}GB of {{ storageStats.total }}GB used
        </div>
      </div>
    </aside>

    <!-- Main Content -->
    <main class="ml-64 p-8 min-h-screen">
      <!-- Header -->
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
            @click="setView('profile')"
            class="w-10 h-10 rounded-full bg-black text-white flex items-center justify-center text-sm font-bold cursor-pointer hover:opacity-80 transition-opacity"
          >
            JD
          </button>
        </div>
      </header>

      <!-- View Wrapper -->
      <div class="max-w-5xl mx-auto">
        
        <!-- Dashboard View -->
        <div v-if="currentView === 'dashboard'" class="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
          <div class="grid grid-cols-3 gap-6">
            <div class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm">
              <div class="flex items-center justify-between mb-4">
                <div class="p-2 bg-[#F2F0ED] rounded-lg"><Database class="w-5 h-5" /></div>
                <MoreVertical class="w-4 h-4 text-[#999]" />
              </div>
              <h3 class="text-sm font-medium text-[#666]">Total Storage</h3>
              <p class="text-3xl font-bold mt-1">512 GB</p>
            </div>
            <div class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm">
              <div class="flex items-center justify-between mb-4">
                <div class="p-2 bg-[#F2F0ED] rounded-lg"><Activity class="w-5 h-5" /></div>
                <MoreVertical class="w-4 h-4 text-[#999]" />
              </div>
              <h3 class="text-sm font-medium text-[#666]">Avg Daily Upload</h3>
              <p class="text-3xl font-bold mt-1">1.2 GB</p>
            </div>
            <div class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm text-white bg-black">
              <div class="flex items-center justify-between mb-4">
                <div class="p-2 bg-white/10 rounded-lg"><Shield class="w-5 h-5" /></div>
                <MoreVertical class="w-4 h-4 text-white/40" />
              </div>
              <h3 class="text-sm font-medium text-white/60">System Status</h3>
              <p class="text-3xl font-bold mt-1 uppercase text-sm tracking-widest flex items-center">
                Secure <span class="ml-2 w-2 h-2 bg-green-400 rounded-full animate-pulse"></span>
              </p>
            </div>
          </div>

          <div class="bg-white border border-[#E5E1DD] rounded-2xl p-8">
            <h2 class="text-xl font-bold mb-6">Recent Activity</h2>
            <div class="space-y-6">
              <div v-for="i in 3" :key="i" class="flex items-start space-x-4">
                <div class="mt-1 w-2 h-2 rounded-full bg-black"></div>
                <div class="flex-1">
                  <p class="text-sm font-medium">Backup completed successfully</p>
                  <p class="text-xs text-[#999] mt-1">Automatic snapshot created in Europe-West-2</p>
                </div>
                <span class="text-xs text-[#999]">{{ i*2 }}h ago</span>
              </div>
            </div>
          </div>
        </div>

        <!-- File Browser View -->
        <div v-if="currentView === 'files'" class="animate-in fade-in slide-in-from-bottom-4 duration-500">
          <div class="flex justify-between items-center mb-6">
            <h2 class="text-2xl font-bold">My Storage</h2>
            <div class="flex space-x-3">
              <button class="px-4 py-2 border border-[#E5E1DD] rounded-lg inline-flex items-center space-x-2 text-sm font-medium hover:bg-[#F9F8F6]">
                <Plus class="w-4 h-4" />
                <span>New Folder</span>
              </button>
              <button class="px-4 py-2 bg-black text-white rounded-lg inline-flex items-center space-x-2 text-sm font-medium hover:opacity-90">
                <Upload class="w-4 h-4" />
                <span>Upload File</span>
              </button>
            </div>
          </div>

          <div class="bg-white border border-[#E5E1DD] rounded-2xl overflow-hidden shadow-sm">
            <table class="w-full text-left">
              <thead class="bg-[#FDFCFB] border-bottom border-[#E5E1DD]">
                <tr>
                  <th class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]">Name</th>
                  <th class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]">Size</th>
                  <th class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]">Type</th>
                  <th class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]">Modified</th>
                  <th class="px-6 py-4 text-xs font-semibold uppercase tracking-wider text-[#999]"></th>
                </tr>
              </thead>
              <tbody class="divide-y divide-[#E5E1DD]">
                <tr v-for="file in files" :key="file.id" class="hover:bg-[#F9F8F6] group transition-colors">
                  <td class="px-6 py-4">
                    <div class="flex items-center space-x-3">
                      <Folder v-if="file.type === 'folder'" class="w-5 h-5 text-amber-400 fill-amber-100" />
                      <File v-else class="w-5 h-5 text-blue-400 fill-blue-50" />
                      <span class="font-medium text-sm">{{ file.name }}</span>
                    </div>
                  </td>
                  <td class="px-6 py-4 text-sm text-[#666]">{{ file.size }}</td>
                  <td class="px-6 py-4 text-sm text-[#666]">
                    <span v-if="file.type === 'folder'" class="bg-[#F2F0ED] px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-tighter">{{ file.items }} items</span>
                    <span v-else class="text-[10px] font-bold uppercase tracking-tighter">{{ file.ext }}</span>
                  </td>
                  <td class="px-6 py-4 text-sm text-[#666]">{{ file.date }}</td>
                  <td class="px-6 py-4 text-right">
                    <button class="opacity-0 group-hover:opacity-100 p-1 hover:bg-[#E5E1DD] rounded transition-all">
                      <MoreVertical class="w-4 h-4 text-[#666]" />
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <!-- Security View -->
        <div v-if="currentView === 'security'" class="animate-in fade-in slide-in-from-bottom-4 duration-500">
          <div class="mb-8">
            <h2 class="text-2xl font-bold">Security Center</h2>
            <p class="text-[#666] mt-1">Manage encryption, access control and audit logs.</p>
          </div>

          <div class="grid grid-cols-1 gap-6">
            <div 
              v-for="item in securitySettings" 
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

            <!-- Detailed Security Card -->
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

        <!-- Configuration View -->
        <div v-if="currentView === 'configuration'" class="animate-in fade-in slide-in-from-bottom-4 duration-500">
          <div class="mb-8 flex justify-between items-end">
            <div>
              <h2 class="text-2xl font-bold">System Configuration</h2>
              <p class="text-[#666] mt-1">Infrastructure settings and service parameters.</p>
            </div>
            <button class="px-6 py-2 bg-black text-white rounded-lg text-sm font-bold shadow-lg shadow-black/10 hover:opacity-90">
              Apply Changes
            </button>
          </div>

          <div class="grid grid-cols-2 gap-6">
            <div 
              v-for="item in configSettings" 
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
              <p class="text-xs text-[#999] leading-relaxed">{{ item.description }}</p>
              <button class="mt-4 text-xs font-bold text-black border-b border-black pb-0.5">Manage Details</button>
            </div>

            <!-- Specialized Config: Webhooks -->
            <div class="p-6 bg-white border border-[#E5E1DD] rounded-2xl shadow-sm flex flex-col justify-between">
              <div>
                 <div class="flex items-center space-x-4 mb-4">
                  <div class="p-2 bg-[#F2F0ED] rounded-lg">
                    <Globe class="w-5 h-5 text-black" />
                  </div>
                  <h3 class="font-bold text-sm">Webhooks</h3>
                </div>
                <p class="text-xs text-[#999] mb-4">Trigger external services when files are modified.</p>
              </div>
              <div class="flex space-x-2">
                <div class="flex-1 h-1 bg-[#F2F0ED] rounded-full"></div>
                <div class="flex-1 h-1 bg-[#F2F0ED] rounded-full"></div>
                <div class="flex-1 h-1 bg-black rounded-full"></div>
              </div>
              <button class="mt-4 text-xs font-bold text-black border-b border-black pb-0.5 w-max">Configure Hooks</button>
            </div>
          </div>
        </div>

        <!-- Profile View -->
        <div v-if="currentView === 'profile'" class="animate-in fade-in slide-in-from-bottom-4 duration-500">
          <div class="mb-8">
            <h2 class="text-2xl font-bold">User Profile</h2>
            <p class="text-[#666] mt-1">Manage your personal information and account preferences.</p>
          </div>

          <div class="grid grid-cols-3 gap-8">
            <!-- Left Column: Avatar & Basic Info -->
            <div class="col-span-1 space-y-6">
              <div class="bg-white border border-[#E5E1DD] rounded-2xl p-8 text-center shadow-sm">
                <div class="relative w-32 h-32 mx-auto mb-6">
                  <div class="w-full h-full rounded-2xl bg-[#F2F0ED] flex items-center justify-center text-4xl font-bold italic serif">
                    {{ userProfile.name.split(' ').map(n => n[0]).join('') }}
                  </div>
                  <button class="absolute -bottom-2 -right-2 w-10 h-10 bg-black text-white rounded-xl flex items-center justify-center shadow-lg border-4 border-white hover:scale-105 transition-transform">
                    <Camera class="w-4 h-4" />
                  </button>
                </div>
                <h3 class="text-xl font-bold">{{ userProfile.name }}</h3>
                <p class="text-sm text-[#999] mb-6">{{ userProfile.role }}</p>
                
                <div class="pt-6 border-t border-[#F2F0ED] space-y-4 text-left">
                  <div class="flex items-center space-x-3 text-sm">
                    <Mail class="w-4 h-4 text-[#999]" />
                    <span class="text-[#666]">{{ userProfile.email }}</span>
                  </div>
                  <div class="flex items-center space-x-3 text-sm">
                    <ShieldCheck class="w-4 h-4 text-[#999]" />
                    <span class="text-green-600 font-medium">Verified Account</span>
                  </div>
                </div>

                <button 
                  @click="handleLogout"
                  class="w-full mt-8 py-3 bg-[#FFF0F0] text-[#FF4444] rounded-xl text-sm font-bold flex items-center justify-center space-x-2 hover:bg-[#FFE5E5] transition-colors"
                >
                  <LogOut class="w-4 h-4" />
                  <span>Sign Out</span>
                </button>
              </div>
            </div>

            <!-- Right Column: Settings & Preferences -->
            <div class="col-span-2 space-y-6">
              <!-- Account Settings -->
              <div class="bg-white border border-[#E5E1DD] rounded-2xl p-8 shadow-sm">
                <h3 class="text-lg font-bold mb-6">Account Settings</h3>
                <div class="space-y-6">
                  <div class="grid grid-cols-2 gap-6">
                    <div class="space-y-2">
                       <label class="text-xs font-bold uppercase tracking-wider text-[#999]">Full Name</label>
                       <input type="text" v-model="userProfile.name" class="w-full px-4 py-2.5 bg-white border border-[#E5E1DD] rounded-lg text-sm focus:outline-none focus:border-black" />
                    </div>
                    <div class="space-y-2">
                       <label class="text-xs font-bold uppercase tracking-wider text-[#999]">Email Address</label>
                       <input type="email" v-model="userProfile.email" class="w-full px-4 py-2.5 bg-white border border-[#E5E1DD] rounded-lg text-sm focus:outline-none focus:border-black" />
                    </div>
                  </div>
                  
                  <div class="pt-6 border-t border-[#F2F0ED]">
                    <h4 class="text-sm font-bold mb-4">Notification Preferences</h4>
                    <div class="space-y-4">
                      <div v-for="(val, key) in userProfile.notifications" :key="key" class="flex items-center justify-between">
                        <span class="text-sm text-[#666] capitalize">{{ key }} Notifications</span>
                        <div 
                          class="w-10 h-5 rounded-full p-1 cursor-pointer transition-colors" 
                          :class="val ? 'bg-black' : 'bg-[#E5E1DD]'"
                          @click="userProfile.notifications[key] = !userProfile.notifications[key]"
                        >
                          <div class="w-3 h-3 bg-white rounded-full transition-transform" :style="{ transform: val ? 'translateX(20px)' : 'translateX(0)' }"></div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                
                <div class="mt-8 pt-6 border-t border-[#F2F0ED] flex justify-end">
                  <button class="px-6 py-2.5 bg-black text-white rounded-lg text-sm font-bold shadow-lg shadow-black/10 hover:opacity-90 transition-opacity">
                    Save Changes
                  </button>
                </div>
              </div>

              <!-- Security Brief -->
              <div class="bg-black text-white border border-black rounded-2xl p-8 shadow-sm">
                <div class="flex items-start justify-between">
                  <div>
                    <h3 class="text-lg font-bold">Two-Factor Authentication</h3>
                    <p class="text-white/60 text-sm mt-1 max-w-sm">Secure your account with an extra layer of protection during login.</p>
                  </div>
                  <span class="bg-green-400/20 text-green-400 px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider">Active</span>
                </div>
                <button class="mt-6 text-sm font-bold border-b border-white pb-1 hover:border-white/60 transition-colors">Manage 2FA Settings</button>
              </div>
            </div>
          </div>
        </div>

      </div>
    </main>
    </template>
  </div>
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&family=Playfair+Display:ital,wght@0,700;1,700&display=swap');

.serif {
  font-family: 'Playfair Display', serif;
}

body {
  overflow-x: hidden;
}

/* Custom shadow for cards */
.shadow-sm {
  box-shadow: 0 1px 3px rgba(0,0,0,0.05), 0 1px 2px rgba(0,0,0,0.03);
}

.shadow-md {
  box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05), 0 2px 4px -1px rgba(0,0,0,0.03);
}

/* Animations */
.animate-in {
  animation-duration: 400ms;
  animation-timing-function: cubic-bezier(0.16, 1, 0.3, 1);
  animation-fill-mode: forwards;
}

.fade-in {
  animation-name: fadeIn;
}

.slide-in-from-bottom-4 {
  animation-name: slideInFromBottom4;
}

.slide-in-from-bottom-8 {
  animation-name: slideInFromBottom8;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideInFromBottom4 {
  from { transform: translateY(1rem); }
  to { transform: translateY(0); }
}

@keyframes slideInFromBottom8 {
  from { transform: translateY(2rem); }
  to { transform: translateY(0); }
}
</style>
