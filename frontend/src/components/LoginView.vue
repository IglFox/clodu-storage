<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { CryptoService } from '../utils/crypto';

const router = useRouter();
const step = ref('login'); // 'login', 'register'
const authForm = ref({ email: '', password: '' });
const errorMsg = ref('');

const showError = (msg) => {
  errorMsg.value = msg;
  setTimeout(() => {
    errorMsg.value = '';
  }, 4000);
};

onMounted(async () => {
  const currentUserStr = localStorage.getItem('currentUser');
  if (!currentUserStr) {
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('masterKey');
    sessionStorage.removeItem('isVaultUnlocked');
    step.value = 'register';
    return;
  }

  const currentUser = JSON.parse(currentUserStr);
  const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
  
  // Clean up any stale masterKey in localStorage and sync with this user's IndexedDB
  const idbKey = await CryptoService.getKey('master_key_' + currentUser.email);
  if (idbKey) {
    localStorage.setItem('masterKey', idbKey.value);
  } else {
    localStorage.removeItem('masterKey');
  }

  const hasMasterKey = !!localStorage.getItem('masterKey');
  const isVaultUnlocked = sessionStorage.getItem('isVaultUnlocked') === 'true';

  if (isLoggedIn) {
    if (hasMasterKey && !isVaultUnlocked) {
      step.value = 'vault_unlock';
    } else {
      router.push('/dashboard');
    }
  } else {
    step.value = 'login';
  }
});

const handleAuth = async () => {
  if (step.value === 'register') {
    if (!authForm.value.email.includes('@')) {
      showError('Please enter a valid email address.');
      return;
    }
    if (authForm.value.password.length < 6) {
      showError('Password must be at least 6 characters long.');
      return;
    }
    
    // Mock Registration
    // Fresh Registration: clear any old vault data for this browser
    localStorage.removeItem('masterKey');
    sessionStorage.removeItem('isVaultUnlocked');
    
    localStorage.setItem('currentUser', JSON.stringify({ email: authForm.value.email, password: authForm.value.password }));
    localStorage.setItem('isLoggedIn', 'true');
    router.push('/dashboard');
  } else {
    const stored = localStorage.getItem('currentUser');
    if (!stored) {
      showError('No account found. Please register first.');
      step.value = 'register';
      return;
    }
    
    const user = JSON.parse(stored);
    if (user.email === authForm.value.email && user.password === authForm.value.password) {
      localStorage.setItem('isLoggedIn', 'true');
      
      // If user has a master key inside secure enclave, load and request to unlock
      const idbKey = await CryptoService.getKey('master_key_' + user.email);
      if (idbKey) {
        localStorage.setItem('masterKey', idbKey.value);
        step.value = 'vault_unlock';
      } else {
        localStorage.removeItem('masterKey');
        router.push('/dashboard');
      }
    } else {
      showError('Invalid email or password.');
    }
  }
};

const masterKey = ref('');

const unlockVault = () => {
  const storedKey = localStorage.getItem('masterKey');
  if (masterKey.value === storedKey) {
    sessionStorage.setItem('isVaultUnlocked', 'true');
    router.push('/dashboard');
  } else {
    showError('Access Denied: Invalid Master Key.');
  }
};
</script>

<template>
  <div>
    
    <!-- ERROR POPUP -->
    <div v-if="errorMsg" style="border: 1px solid red; color: red; padding: 10px; margin-bottom: 20px;">
      {{ errorMsg }}
    </div>

    <header>
      <h1>CloudVault</h1>
      <p>Secure Intelligence Node</p>
    </header>

    <main>
      <div v-if="step === 'login' || step === 'register'">
        <h3>{{ step === 'login' ? 'Welcome Back' : 'Create Account' }}</h3>
        <br />
        <form @submit.prevent="handleAuth">
          <div>
            <label>EMAIL ADDRESS</label><br />
            <input type="email" v-model="authForm.email" required />
          </div>
          <br />
          <div>
            <label>PASSWORD</label><br />
            <input type="password" v-model="authForm.password" required />
          </div>
          <br />
          <button type="submit">
            {{ step === 'login' ? 'Sign In' : 'Register' }}
          </button>
        </form>
        <p>
          <a href="#" @click.prevent="step = step === 'login' ? 'register' : 'login'">
            {{ step === 'login' ? "Don't have an account? Register" : "Already have an account? Login" }}
          </a>
        </p>
      </div>

      <!-- PHASE 2: VAULT UNLOCK (only if Master Key exists) -->
      <div v-if="step === 'vault_unlock'">
        <h3>Unlock Vault</h3>
        <p>Your vault is encrypted. Enter Master Key to proceed.</p>
        <form @submit.prevent="unlockVault">
          <div>
            <label>MASTER KEY</label><br />
            <input type="password" v-model="masterKey" required />
          </div>
          <br />
          <button type="submit">Unlock & Enter</button>
        </form>
      </div>
    </main>
  </div>
</template>
