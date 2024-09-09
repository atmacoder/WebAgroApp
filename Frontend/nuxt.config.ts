export default defineNuxtConfig({
  modules: [
    '@pinia/nuxt',
    '@nuxtjs/tailwindcss',
    '@pinia-plugin-persistedstate/nuxt',
    'vue-yandex-maps/nuxt',
  ],
  yandexMaps: {
    apikey: '8d013790-c451-4991-a7bc-e9065121d902',
  },
  runtimeConfig: {
    public: {
      apiBase: process.env.API_BASE_URL, // Use environment variable
          baseUrl: 'localhost:7067'
    },
  },
	devServer: {
		host: 'localhost',
	  },
  router: {
    middleware: ['auth'], // Apply auth middleware to all routes
  },
  // Use .env files to manage your environment variables (development, staging, production)
  // Set your API base URL in your environment files (e.g., .env.development, .env.staging, .env.production)
  // Example:
  // API_BASE_URL=http://localhost:8000 (for development)
  // API_BASE_URL=https://your-api.com (for production)
  pinia: {
    // Automatically persist state to local storage
    autoHydrate: true, 
    // Add this configuration to save your Pinia state on localStorage
    // Make sure you have installed @pinia/plugin-persistedstate:
    // npm install @pinia/plugin-persistedstate
    // yarn add @pinia/plugin-persistedstate
    // This line imports the @pinia/plugin-persistedstate:
    // import piniaPluginPersistedstate from '@pinia/plugin-persistedstate'
    // This line registers the plugin:
    // const pinia = createPinia()
    // pinia.use(piniaPluginPersistedstate)
    // Then you can use it on the Pinia config:
    // pinia: {
    //   persistedstate: {
    //     storage: localStorage,
    //   },
    // },
    // If you don't have the plugin installed, use a custom hydration function:
    // hydrate: (store) => {
    //   store.auth.loadUserFromLocalStorage(); // Call your store's action
    // }
  },
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
    build: {
    transpile: [
      // ... 
      'vue-yandex-maps', // Добавьте, если это необходимо
    ],
  }
});