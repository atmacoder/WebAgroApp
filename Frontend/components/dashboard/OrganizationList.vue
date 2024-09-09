<template>
  <div class="container mx-auto p-4">
    <h2 class="text-2xl font-bold mb-4">Список организаций</h2>

    <div class="mb-4">
      <input
          type="text"
          v-model="searchQuery"
          placeholder="Поиск по названию или району"
          class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
      />
    </div>

    <div v-if="isLoading">
      <p>Загрузка...</p>
    </div>

    <div v-else-if="error">
      <p class="text-red-500">Ошибка: {{ error.message }}</p>
    </div>

    <ul v-else>
      <li   v-for="organization in filteredOrganizations"
            :key="organization.id"
            class="mb-2 cursor-pointer"
            @click="addOrganizationToCurrent(organization)">
        {{ organization.name }}
      </li>
    </ul>
  </div>

</template>

<script setup>
import {ref, onMounted, computed, watch} from 'vue';
import {useFetch, useRuntimeConfig} from '#imports';
import {useOrganizationsStore} from '../stores/organizations';
import {useAuthStore} from '~/stores/auth';

const organizationsStore = useOrganizationsStore();
const authStore = useAuthStore();

const name = ref('');
const deliveryType = ref('Стандартная');
const logoPath = ref('');
const courierCount = ref(0);
const deliveryArea = ref('');

const {public: {apiBase}} = useRuntimeConfig();

const createPolygon = async () => {
  try {
    await polygonsStore.createPolygon({
      // ... (другие данные)
      user_id: authStore.user.id,
      organization_id: organizationsStore.currentOrganization.id // ID текущей организации
    });

    // ...
  } catch (error) {
    // ...
  }
};

watch(
    () => organizationsStore.organizations,
    (newOrganizations) => {
      organizations.value = newOrganizations;
    }
);
const isLoading = ref(true);
const organizations = ref([]);
const error = ref(null);
const searchQuery = ref(''); // Добавлено для поиска

const filteredOrganizations = computed(() => {
  if (!searchQuery.value) {
    return organizations.value;
  }

  const lowercaseQuery = searchQuery.value.toLowerCase();
  return organizations.value.filter(org => {
    return org.name.toLowerCase().includes(lowercaseQuery);
  });
});

onMounted(async () => {
  try {
    await organizationsStore.fetchOrganizations();
    organizations.value = organizationsStore.organizations;
    isLoading.value = false;
  } catch (error) {
    console.error('Ошибка при получении организаций:', error);
    error.value = error;
    isLoading.value = false;
  }
});

const addOrganizationToCurrent = (organization) => {
  // Добавьте логику добавления организации в текущее состояние
  // Например:
  organizationsStore.addOrganizationToCurrent(organization);
};

</script>