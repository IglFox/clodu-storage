<script setup>
import { ref } from 'vue';
import { useFileStore } from '../stores/files';
import { deriveKey, encryptData } from '../utils/crypto';
import { 
  X, 
  FileUp, 
  Lock, 
  Unlock, 
  ShieldCheck, 
  Cpu 
} from '@lucide/vue';

const props = defineProps({
  isOpen: {
    type: Boolean,
    required: true
  }
});

const emit = defineEmits(['close']);

const fileStore = useFileStore();

const selectedFile = ref(null);
const isEncrypted = ref(false);
const passphrase = ref('');
const showPassword = ref(false);
const errorMessage = ref('');

function handleFileChange(event) {
  selectedFile.value = event.target.files[0];
}

function handleUpload() {
  errorMessage.value = '';
  
  if (!selectedFile.value) {
    errorMessage.value = 'Please select a file to upload.';
    return;
  }
  
  if (isEncrypted.value) {
    if (!passphrase.value) {
      errorMessage.value = 'Encryption requires a secure Master passphrase.';
      return;
    }
    
    // Key derivation and encryption (mock)
    const key = deriveKey(passphrase.value);
    const encryptedResult = encryptData(selectedFile.value.name, key.raw);
    
    fileStore.uploadFile(selectedFile.value);
  } else {
    fileStore.uploadFile(selectedFile.value);
  }
  
  // Reset fields & close
  selectedFile.value = null;
  passphrase.value = '';
  isEncrypted.value = false;
  emit('close');
}
</script>

<template>
  <div 
    v-if="isOpen"
    class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-folder/20 backdrop-blur-[2px] select-none"
    @click.self="emit('close')"
  >
    <div 
      class="w-full max-w-md bg-surface border border-border rounded-xl p-8 shadow-2xl relative animate-fade-fast"
    >
      <!-- Close button -->
      <button 
        @click="emit('close')"
        class="absolute top-4 right-4 p-1.5 rounded-lg hover:bg-canvas text-text-secondary transition-colors cursor-pointer"
      >
        <X class="w-4 h-4" />
      </button>

      <!-- Header -->
      <div class="flex flex-col items-center mb-8">
        <div class="w-10 h-10 bg-folder rounded-lg flex items-center justify-center mb-4">
          <FileUp class="w-5 h-5 text-white opacity-90" />
        </div>
        <h3 class="text-lg font-medium text-text-primary tracking-tight">Upload to Network</h3>
        <p class="text-xs text-text-secondary mt-1">Select a local object to distribute</p>
      </div>

      <!-- Error block -->
      <div 
        v-if="errorMessage"
        class="mb-6 p-3 rounded-lg bg-red-50 border border-red-100 text-[11px] text-red-600 font-medium text-center"
      >
        {{ errorMessage }}
      </div>

      <!-- Form -->
      <div class="space-y-6">
        <!-- File Input -->
        <div class="space-y-2">
          <label class="text-[11px] font-bold uppercase tracking-widest text-text-secondary ml-1">Object Source</label>
          <input 
            type="file" 
            @change="handleFileChange"
            class="w-full text-[12px] px-3 py-2 bg-canvas border border-border rounded-lg text-text-primary cursor-pointer file:mr-4 file:py-1 file:px-2 file:rounded file:border-0 file:text-[10px] file:font-semibold file:bg-folder file:text-white hover:file:opacity-90"
          />
        </div>

        <!-- Encryption -->
        <div class="p-4 rounded-xl bg-canvas border border-border space-y-4">
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-2">
              <Lock v-if="isEncrypted" class="w-3.5 h-3.5 text-text-primary" />
              <Unlock v-else class="w-3.5 h-3.5 text-text-secondary opacity-40" />
              <span class="text-[12px] font-medium text-text-primary">
                Zero-Knowledge Privacy
              </span>
            </div>
            <label class="relative inline-flex items-center cursor-pointer">
              <input v-model="isEncrypted" type="checkbox" class="sr-only peer" />
              <div class="w-8 h-4.5 bg-border rounded-full peer peer-checked:after:translate-x-full after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-surface after:rounded-full after:h-3.5 after:w-3.5 after:transition-all peer-checked:bg-folder"></div>
            </label>
          </div>

          <!-- Passphrase -->
          <div v-if="isEncrypted" class="space-y-2.5 animate-fade-fast">
            <input 
              v-model="passphrase"
              :type="showPassword ? 'text' : 'password'" 
              placeholder="Private master key..."
              class="input-neutral font-mono"
              @keydown.enter="handleUpload"
            />
            <p class="text-[10px] text-text-secondary leading-tight opacity-70">
              Your key is derived locally and never leaves this device. 
            </p>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div class="mt-8 flex gap-3">
        <button 
          @click="emit('close')"
          class="flex-1 btn-neutral h-10"
        >
          Cancel
        </button>
        <button 
          @click="handleUpload"
          class="flex-1 h-10 bg-folder text-white text-[13px] font-medium rounded-lg hover:opacity-95 active:scale-[0.98] transition-all"
        >
          {{ isEncrypted ? 'Secure & Upload' : 'Upload Object' }}
        </button>
      </div>
    </div>
  </div>
</template>
