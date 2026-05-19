import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import './index.css';

// Reset session unlock state on page refresh/initial load for secure zero-knowledge architecture
sessionStorage.removeItem('isVaultUnlocked');

const app = createApp(App);
app.use(router);
app.mount('#root');
