export default defineNuxtRouteMiddleware((to, from) => {
    const authStore = useAuthStore();
    if (!authStore.isLoggedIn) {
        return navigateTo('/login'); // Перенаправление на страницу логина, если не авторизован
    }
});