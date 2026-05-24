<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { CryptoService } from '../utils/crypto';
import { AuthService } from '../utils/authService';

const router = useRouter();
const step = ref('login'); // 'login', 'register'
const authForm = ref({ email: '', password: '' });
const errorMsg = ref('');
const showSettings = ref(false);
const customAuthApiUrl = ref(AuthService.getApiUrl());

const saveSettings = () => {
  AuthService.setApiUrl(customAuthApiUrl.value);
};

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

  if (isLoggedIn) {
    router.push('/dashboard');
  } else {
    step.value = 'login';
  }
});

const handleAuth = async () => {
  if (!authForm.value.email.includes('@')) {
    showError('Please enter a valid email address.');
    return;
  }
  if (authForm.value.password.length < 6) {
    showError('Password must be at least 6 characters long.');
    return;
  }

  const microserviceUrl = customAuthApiUrl.value.trim();
  if (microserviceUrl) {
    try {
      const resData = await AuthService.authenticate(
        microserviceUrl,
        step.value,
        authForm.value.email,
        authForm.value.password
      );

      localStorage.setItem('currentUser', JSON.stringify({
        email: authForm.value.email,
        password: authForm.value.password,
        token: resData.token || 'microservice-simulated-jwt-token'
      }));
      localStorage.setItem('isLoggedIn', 'true');

      // Sync master key scoped to user email
      const idbKey = await CryptoService.getKey('master_key_' + authForm.value.email);
      if (idbKey) {
        localStorage.setItem('masterKey', idbKey.value);
      } else {
        localStorage.removeItem('masterKey');
      }

      router.push('/dashboard');
      return;
    } catch (err) {
      showError(`Authorization Microservice Failed: ${err.message}`);
      return;
    }
  }

  if (step.value === 'register') {
    // Mock Registration
    // Fresh Registration: clear any old vault data for this browser
    localStorage.removeItem('masterKey');
    
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
      
      // If user has a master key inside secure enclave, load it silently
      const idbKey = await CryptoService.getKey('master_key_' + user.email);
      if (idbKey) {
        localStorage.setItem('masterKey', idbKey.value);
      } else {
        localStorage.removeItem('masterKey');
      }
      router.push('/dashboard');
    } else {
      showError('Invalid email or password.');
    }
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

        <hr style="border: none; border-top: 1px dashed #ccc; margin: 25px 0 20px 0;" />
        
        <div style="text-align: center;">
          <a href="#" @click.prevent="showSettings = !showSettings" style="font-size: 0.85rem; color: #555; text-decoration: underline;">
            {{ showSettings ? 'Hide Microservice Settings' : 'Configure Authorization Microservice' }}
          </a>
        </div>

        <div v-if="showSettings" style="margin-top: 15px; padding: 15px; border: 1px solid #ddd; background: #fafafa; border-radius: 4px; text-align: left;">
          <h4 style="margin: 0 0 10px 0; font-size: 0.95rem; color: #222;">Authorization Microservice</h4>
          <p style="font-size: 0.8rem; color: #666; margin: 0 0 12px 0; line-height: 1.4;">
            Specify an external service URL (e.g., <code>https://api.myauth.com</code>) to authenticate users. If left blank, offline zero-knowledge virtual accounts are used.
          </p>
          <div>
            <label style="font-size: 0.75rem; font-weight: bold; color: #444; letter-spacing: 0.05em;">API ENDPOINT URL</label><br />
            <input 
              type="url" 
              v-model="customAuthApiUrl" 
              placeholder="e.g., http://localhost:8080 or https://auth.api" 
              style="width: 100%; box-sizing: border-box; padding: 6px; font-family: monospace; font-size: 0.85rem; margin-top: 4px; border: 1px solid #ccc; border-radius: 3px;"
              @input="saveSettings"
            />
          </div>
          <div style="margin-top: 8px; font-size: 0.75rem; color: #22863a; font-weight: bold;" v-if="customAuthApiUrl.trim()">
            ● Active Service Endpoint: Send POST `/register` & `/login` requests
          </div>
          <div style="margin-top: 8px; font-size: 0.75rem; color: #777;" v-else>
            ○ Standalone Mode: Zero-Knowledge browser sandbox
          </div>
        </div>
      </div>
    </main>
  </div>
</template>
