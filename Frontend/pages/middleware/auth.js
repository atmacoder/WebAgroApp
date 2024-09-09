import { defineNuxtRouteMiddleware } from '#imports';
import { useAuthStore } from '~/stores/auth';

export default defineNuxtRouteMiddleware((to, from) => {
  const authStore = useAuthStore();

  if (!authStore.isLoggedIn && !localStorage.getItem('token')) {
    return navigateTo('/login');
  } 
});