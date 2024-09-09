<template>
  <div class="container mx-auto p-4">
    <h1 class="text-3xl font-bold mb-4">Добавить организацию</h1>

     <form @submit.prevent="createOrganization">
      <div class="mb-4">
        <label for="name" class="block text-gray-700 font-bold mb-2">Название:</label>
        <input
          type="text"
          id="name"
          v-model="name"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>

      <div class="mb-4">
        <label for="delivery-type" class="block text-gray-700 font-bold mb-2">Тип доставки:</label>
        <select
          id="delivery-type"
          v-model="deliveryType"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        >
          <option value="Срочная">Срочная</option>
          <option value="Стандартная">Стандартная</option>
          <option value="Медленная">Медленная</option>
        </select>
      </div>

      <div class="mb-4">
        <label for="logo-path" class="block text-gray-700 font-bold mb-2">Путь к логотипу:</label>
        <input
          type="text"
          id="logo-path"
          v-model="logoPath"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>

      <div class="mb-4">
        <label for="courier-count" class="block text-gray-700 font-bold mb-2">Количество курьеров:</label>
        <input
          type="number"
          id="courier-count"
          v-model="courierCount"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>

      <div class="mb-4">
        <label for="delivery-area" class="block text-gray-700 font-bold mb-2">Район доставки:</label>
        <input
          type="text"
          id="delivery-area"
          v-model="deliveryArea"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        />
      </div>

      <button type="submit" class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
        Добавить
      </button>
    </form>

  </div>
</template>

      
<script setup>
import { ref } from 'vue';
import { useFetch, useRuntimeConfig } from '#imports';
import { useOrganizationsStore } from '../stores/organizations';
import { useAuthStore } from '~/stores/auth';

const organizationsStore = useOrganizationsStore();
const authStore = useAuthStore(); 

const name = ref('');
const deliveryType = ref('Стандартная');
const logoPath = ref('');
const courierCount = ref(0);
const deliveryArea = ref('');

// Получаем apiBase из nuxt.config.js
const { public: { apiBase } } = useRuntimeConfig(); 

const { data: organizations, pending, error } = useFetch(`${apiBase}/api/organizations`);

const createOrganization = async () => {
  try {
    await organizationsStore.createOrganization({
      name: name.value,
      delivery_type: deliveryType.value,
      logo_path: logoPath.value,
      courier_count: courierCount.value,
      delivery_area: deliveryArea.value,
    });

    // Очистить поля формы
    name.value = '';
    deliveryType.value = 'Стандартная';
    logoPath.value = '';
    courierCount.value = 0;
    deliveryArea.value = '';
  } catch (error) {
    console.error('Ошибка при создании организации:', error);
  }
};

</script>

    