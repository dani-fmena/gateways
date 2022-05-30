import { RouteRecordRaw } from 'vue-router';
import GatewayPage from 'pages/GatewayPage.vue'
import AboutPage from 'pages/AboutPage.vue'
import DevicePage from 'pages/DevicePage.vue'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    // skip the lazy loading
    children: [
      { path: '', component: GatewayPage },
      { path: '/about', component: AboutPage },
      { path: '/gateway/:id', name: 'devices', component: DevicePage },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
