<template>
   
      <h2 class="text-2xl font-bold text-center mb-6">Авторизация</h2>
      <form @submit.prevent="login" class="max-w-sm mx-auto">
        <div class="mb-5">
          <label for="email" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Ваш Email</label>
          <input
            type="email"
            id="email"
            v-model="credentials.email"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            placeholder="name@mail.ru"
            required
          />
        </div>
        <div class="mb-5">
          <label for="password" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Ваш пароль</label>
          <input
            type="password"
            id="password"
            v-model="credentials.password"
            class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            required
          />
        </div>
        <button
          type="submit"
          class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
        >
          Отправить
        </button>
        <div v-if="error" class="text-red-500 mt-2">{{ error }}</div>
      </form>
 
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import { useAuthStore } from '~/stores/auth' 
  import { useRouter } from 'nuxt/app' 
  
  const router = useRouter() 
  const authStore = useAuthStore()
  
  const credentials = ref({
    email: '',
    password: '',
  })
  
  const error = ref(null)
  
  const login = async () => {
    const success = await authStore.login(credentials.value)
  
    if (success) {
      // Перенаправление на главную страницу после успешного входа
      router.push('/dashboard/') 
    } else {
      // Обработка ошибки входа
      error.value = 'Неверный логин или пароль' 
    }
  }
  </script>