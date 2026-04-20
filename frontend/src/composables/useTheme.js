import { ref, watch, onMounted } from 'vue';

export function useTheme() {
  const isLight = ref(false);

  const applyTheme = () => {
    if (isLight.value) {
      document.documentElement.classList.add('light');
    } else {
      document.documentElement.classList.remove('light');
    }
    localStorage.setItem('theme', isLight.value ? 'light' : 'dark');
  };

  const toggleTheme = () => {
    isLight.value = !isLight.value;
    applyTheme();
  };

  onMounted(() => {
    const saved = localStorage.getItem('theme');
    if (saved === 'light') {
      isLight.value = true;
    } else if (saved === 'dark') {
      isLight.value = false;
    } else {
      isLight.value = window.matchMedia('(prefers-color-scheme: light)').matches;
    }
    applyTheme();
  });

  return { isLight, toggleTheme };
}
