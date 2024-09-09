import { defineStore } from 'pinia';
import { useCookie } from '#app';

export const useAuthStore = defineStore('auth', {
    state: () => ({
        user: null,
        token: null,
        refreshToken: null
    }),
    persist: true,
    getters: {
        isLoggedIn: (state) => !!state.user,
    },
    actions: {
        async login(credentials) {
            const { public: { apiBase } } = useRuntimeConfig();
            try {
                const response = await fetch(`${apiBase}/api/auth/login`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(credentials)
                });
                if (response.status === 200) {
                    const data = await response.json();
                    this.token = data.token;
                    this.refreshToken = data.refreshToken;

                    // Используем useCookie
                    const cookieToken = useCookie('token');
                    const cookieRefreshToken = useCookie('refreshToken');

                    cookieToken.value = this.token;
                    cookieRefreshToken.value = this.refreshToken;

                    // Получаем данные пользователя после успешного логина
                    await this.fetchUser();

                    return true;
                } else if (response.status === 401) {
                    throw new Error('Неверный логин или пароль');
                } else {
                    throw new Error(`Ошибка входа: ${response.status}`);
                }
            } catch (error) {
                console.error('Ошибка входа:', error);
                return false;
            }
        },

        async register(credentials) {
            const { public: { apiBase } } = useRuntimeConfig();
            try {
                const response = await fetch(`${apiBase}/api/Auth/register`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(credentials)
                });

                if (response.status === 200) {
                    // Обработываем успешную регистрацию
                    const data = await response.json();
                    this.user = data.user;
                    this.token = data.token;
                    this.refreshToken = data.refreshToken;

                    // Используйем useCookie
                    const cookieToken = useCookie('token');
                    const cookieRefreshToken = useCookie('refreshToken');

                    cookieToken.value = this.token;
                    cookieRefreshToken.value = this.refreshToken;
                    // Получения данных пользователя
                    await this.fetchUser(); 
                    return true;
                } else if (response.status === 422) { // ошибки валидации
                    const errorData = await response.json();
                    throw new Error(JSON.stringify(errorData.errors));
                } else {
                    throw new Error(`Ошибка регистрации: ${response.status}`);
                }
            } catch (error) {
                console.error('Ошибка регистрации:', error);
                return false;
            }
        },

        async logout() {
            const { public: { apiBase } } = useRuntimeConfig();
            try {
                const response = await fetch(`${apiBase}/api/logout`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${this.token}`
                    }
                });

                if (response.status === 200) {
                    this.user = null;
                    this.token = null;
                    this.refreshToken = null;
                    // Используйте useCookie
                    const cookieToken = useCookie('token');
                    const cookieRefreshToken = useCookie('refreshToken');

                    cookieToken.value = null;
                    cookieRefreshToken.value = null;
                } else {
                    throw new Error(`Ошибка выхода: ${response.status}`);
                }
            } catch (error) {
                console.error('Ошибка выхода:', error);
            }
        },

        async fetchUser() {
            const { public: { apiBase } } = useRuntimeConfig();
            try {
                const response = await fetch(`${apiBase}/api/user`, {
                    method: 'GET',
                    headers: {
                        Authorization: `Bearer ${this.token}`
                    }
                });

                if (response.status === 200) {
                    this.user = await response.json();
                    return true;
                } else {
                    throw new Error(`Ошибка получения пользователя: ${response.status}`);
                }
            } catch (error) {
                console.error('Ошибка получения пользователя:', error);
                // Ошибки, например, очистите токены
                this.logout();
                throw new Error('Ошибка получения пользователя.');
            }
        },

        async refreshAccessToken() {
            const { public: { apiBase } } = useRuntimeConfig();
            try {
                const response = await fetch(`${apiBase}/api/refresh`, {
                    method: 'POST',
                    headers: {
                        Authorization: `Bearer ${this.refreshToken}`
                    }
                });
                if (response.status === 200) {
                    const data = await response.json();
                    this.token = data.token; // Обновляем JWT-токен
                    // Используйте useCookie
                    const cookieToken = useCookie('token');

                    cookieToken.value = this.token; // Обновляем cookie 
                    return true;
                } else {
                    // Ошибки, например, очистите токены
                    this.logout();
                    throw new Error('Ошибка обновления токена.');
                }
            } catch (error) {
                console.error('Ошибка обновления токена:', error);
                return false;
            }
        }
    },
});