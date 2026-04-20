import { createRouter, createWebHistory } from 'vue-router';
import FilesView from '../views/FilesView.vue';
import InsightsView from '../views/InsightsView.vue';

const routes = [
  { path: '/', redirect: '/files' },
  { path: '/files', component: FilesView },
  { path: '/insights', component: InsightsView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
