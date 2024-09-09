<template>
  <section class="bg-gray-50 dark:bg-gray-900 mt-4">
    <h2 class="text-2xl font-bold text-center mb-6">Регистрация</h2>
    <form @submit.prevent="register" class="max-w-sm mx-auto">
      <div class="mb-5">
        <label for="name" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Ваше Имя</label>
        <input 
          type="text"
          id="name"
          v-model="credentials.name"
          class="shadow-sm bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 dark:shadow-sm-light"
          placeholder="Введите ваше имя"
          required
        />
        <div v-if="errors.name" class="text-red-500 mt-2">{{ errors.name[0] }}</div> 
      </div>
      <div class="mb-5">
        <label for="email" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Ваш email</label>
        <input
          type="email"
          id="email"
          v-model="credentials.email"
          class="shadow-sm bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 dark:shadow-sm-light"
          placeholder="name@flowbite.com"
          required
        />
        <div v-if="errors.email" class="text-red-500 mt-2">{{ errors.email[0] }}</div>
      </div>
      <div class="mb-5">
        <label for="password" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Ваш пароль</label>
        <input
          type="password"
          id="password"
          v-model="credentials.password"
          class="shadow-sm bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 dark:shadow-sm-light"
          required
        />
        <div v-if="errors.password" class="text-red-500 mt-2">{{ errors.password[0] }}</div>
      </div>
      <div class="mb-5">
        <label
          for="repeat-password"
          class="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
          >Повтор пароля</label
        >
        <input
          type="password"
          id="repeat-password"
          v-model="credentials.password_confirmation"
          class="shadow-sm bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 dark:shadow-sm-light"
          required
        />
        <div v-if="errors.password_confirmation" class="text-red-500 mt-2">{{ errors.password_confirmation[0] }}</div>
      </div>
      <div class="flex items-start mb-5">
        <div class="flex items-center h-5">
          <input
            id="terms"
            type="checkbox"
            v-model="agreedToTerms"
            class="w-4 h-4 border border-gray-300 rounded bg-gray-50 focus:ring-3 focus:ring-blue-300 dark:bg-gray-700 dark:border-gray-600 dark:focus:ring-blue-600 dark:ring-offset-gray-800 dark:focus:ring-offset-gray-800"
            required
          />
        </div>
        <label for="terms" class="ms-2 text-sm font-medium text-gray-900 dark:text-gray-300">Я согласен с
          <a href="#" class="text-blue-600 hover:underline dark:text-blue-500">правилами</a>
        </label>
      </div>
      <button
        type="submit"
        :disabled="!agreedToTerms"
        class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:opacity-50 disabled:cursor-not-allowed" 
      >
        Создать новый аккаунт
      </button>
    </form>
  </section>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '~/stores/auth'
import { useRouter } from 'nuxt/app'
import { useError } from '#imports' 

const router = useRouter()
const authStore = useAuthStore()
const errors = ref({})

const credentials = ref({
  name: '',
  email: '',
  password: '',
  password_confirmation: ''
})
const agreedToTerms = ref(false)

const register = async () => {
  if (credentials.value.password !== credentials.value.password_confirmation) {
    errors.value = { password_confirmation: 'Passwords do not match' };
    return
  }

  if (!agreedToTerms.value) {
    errors.value = { terms: 'You must agree to the terms and conditions' };
    return
  }

  try {
    const success = await authStore.register(credentials.value)

    if (success) {
      router.push('/dashboard/')
      credentials.value = {
        name: '',
        email: '',
        password: '',
        password_confirmation: ''
      }
      agreedToTerms.value = false 
      errors.value = {}
    } 
  } catch (error) {
    if (error.message && error.message.startsWith('{')) {
      errors.value = JSON.parse(error.message) 
    } else {
      useError(error) 
      errors.value = { message: 'An error occurred' }
    }
  }
}
</script>