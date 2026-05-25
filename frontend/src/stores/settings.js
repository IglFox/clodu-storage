import { defineStore } from 'pinia';
import { ref, watch } from 'vue';

export const useSettingsStore = defineStore('settings', () => {
  const isDarkMode = ref(localStorage.getItem('dcs_dark_mode') === 'true');

  const toggleDarkMode = () => {
    isDarkMode.value = !isDarkMode.value;
  };

  watch(isDarkMode, (val) => {
    localStorage.setItem('dcs_dark_mode', val);
    if (val) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }, { immediate: true });

  return {
    isDarkMode,
    toggleDarkMode
  };
});
