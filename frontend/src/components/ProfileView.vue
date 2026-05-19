<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { CryptoService } from '../utils/crypto';

const router = useRouter();
const user = ref({ email: 'Unknown', role: 'Vault User' });
const masterKey = ref('');
const showSetup = ref(false);
const hasMasterKeyStatus = ref(!!localStorage.getItem('masterKey'));
const successMsg = ref('');

const hasMasterKey = computed(() => hasMasterKeyStatus.value);

onMounted(async () => {
  const storedUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
  if (storedUser.email) {
    user.value = { 
      email: storedUser.email,
      initials: storedUser.email.substring(0, 2).toUpperCase(),
      role: 'Administrator' 
    };

    // Double check persistent storage for key status (scoped by user email)
    const idbKey = await CryptoService.getKey('master_key_' + storedUser.email);
    if (idbKey) {
      localStorage.setItem('masterKey', idbKey.value);
      hasMasterKeyStatus.value = true;
    } else {
      localStorage.removeItem('masterKey');
      hasMasterKeyStatus.value = false;
    }
  } else {
    hasMasterKeyStatus.value = false;
  }
});

const generateKey = () => {
  const charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+";
  let retVal = "";
  for (let i = 0; i < 32; ++i) {
    retVal += charset.charAt(Math.floor(Math.random() * charset.length));
  }
  masterKey.value = retVal;
};

const saveKey = async () => {
  if (masterKey.value.length < 8) {
    alert('Key too short');
    return;
  }
  
  localStorage.setItem('masterKey', masterKey.value);
  await CryptoService.saveKey('master_key_' + user.value.email, { 
    type: 'master', 
    value: masterKey.value,
    timestamp: Date.now() 
  });

  sessionStorage.setItem('isVaultUnlocked', 'true');
  hasMasterKeyStatus.value = true;
  showSetup.value = false;
  successMsg.value = 'Vault Key Synchronized to Secure Storage';
  setTimeout(() => { successMsg.value = ''; }, 3000);
};

const downloadKey = () => {
  const element = document.createElement("a");
  const file = new Blob([`CLOUDVAULT MASTER KEY:\n\n${masterKey.value}`], {type: 'text/plain'});
  element.href = URL.createObjectURL(file);
  element.download = "vault_master_key.txt";
  document.body.appendChild(element);
  element.click();
};

const logout = () => {
  localStorage.removeItem('isLoggedIn');
  router.push('/login');
};
</script>

<template>
  <div>
    <nav>
      <button @click="$router.push('/')">← Back</button>
      <h2>User Profile</h2>
    </nav>
    <hr />

    <main>
      <div v-if="successMsg" style="background: #e6ffed; color: #22863a; padding: 10px; border: 1px solid #34d058; margin-bottom: 20px; text-align: center;">
        {{ successMsg }}
      </div>

      <div style="border: 1px solid #ccc; padding: 20px; margin-bottom: 20px;">
        <div style="font-size: 2rem; background: #eee; width: 60px; height: 60px; line-height: 60px; text-align: center; border-radius: 50%; margin-bottom: 10px;">
          {{ user.initials || '??' }}
        </div>
        <h3>{{ user.email }}</h3>
        <p>{{ user.role }}</p>
      </div>

      <section style="border: 1px solid #ccc; padding: 20px; margin-bottom: 20px;">
        <h3>Encryption Settings</h3>
        <p>Master Key is required for <strong>CRYPT-MAX</strong> mode.</p>
        
        <div v-if="!showSetup && hasMasterKey">
          <p style="color: green;">✓ Master Key configured and active.</p>
          <button @click="showSetup = true">Change Key</button>
        </div>
        
        <div v-else-if="!showSetup">
          <p style="color: red;">⚠ No Master Key configured.</p>
          <button @click="showSetup = true">Setup Master Key</button>
        </div>

        <div v-if="showSetup" style="margin-top: 15px; border-top: 1px solid #eee; padding-top: 15px;">
          <label>MASTER KEY</label><br />
          <input type="text" v-model="masterKey" style="width: 100%; margin: 10px 0;" /><br />
          <button @click="generateKey">Generate Random</button>
          <button @click="downloadKey" :disabled="!masterKey">Download .txt</button>
          <br /><br />
          <button @click="saveKey" style="background: black; color: white;">Save & Activate</button>
          <button @click="showSetup = false">Cancel</button>
        </div>
      </section>

      <button @click="logout" style="color: red;">
        Sign Out
      </button>
    </main>
  </div>
</template>
