<template>
  <div class="container mx-auto p-4">
    <h1 class="text-3xl font-bold mb-4">Добавить полигон</h1>

    <div v-if="organizationsStore.currentOrganization">
      <label>Вы добавляете полигон для, {{ organizationsStore.currentOrganization.name }}</label>

      <form @submit.prevent="createPolygon">
        <div class="mb-4">
          <label for="coordinates" class="block text-gray-700 font-bold mb-2">Координаты (JSON):</label>
          <textarea
              id="coordinates"
              v-model="coordinates"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          ></textarea>
        </div>
        <div class="mb-4">
          <label for="strokeColor" class="block text-gray-700 font-bold mb-2">Цвет обводки:</label>
          <input
              type="color"
              id="strokeColor"
              v-model="strokeColor"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          />
        </div>
        <div class="mb-4">
          <label for="fillColor" class="block text-gray-700 font-bold mb-2">Цвет заливки:</label>
          <input
              type="color"
              id="fillColor"
              v-model="fillColor"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          />
        </div>

        <div class="mb-4">
          <label for="opacity" class="block text-gray-700 font-bold mb-2">Прозрачность:</label>
          <input
              type="text"
              id="opacity"
              v-model.number="opacity"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          />
        </div>

        <div class="mb-4">
          <label for="strokeWidth" class="block text-gray-700 font-bold mb-2">Ширина обводки:</label>
          <input
              type="number"
              id="strokeWidth"
              v-model.number="strokeWidth"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          />
        </div>

        <div class="mb-4">
          <label for="strokeStyle" class="block text-gray-700 font-bold mb-2">Стиль обводки:</label>
          <select
              id="strokeStyle"
              v-model="strokeStyle"
              class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          >
            <option value="solid">Сплошная</option>
            <option value="dashed">Пунктирная</option>
            <option value="dotted">Точечная</option>
            <option value="shortdash">Короткие штрихи</option>
            <option value="longdash">Длинные штрихи</option>
          </select>
        </div>

        <button type="submit" class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
          Добавить полигон
        </button>
      </form>
    </div>

    <div v-else>
      <p>Выберите организацию для добавления полигона.</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useFetch, useRuntimeConfig } from '#imports';
import { usePolygonsStore } from '../stores/polygonsStore'; // Импортируем store для полигонов
import { useAuthStore } from '~/stores/auth';
import { useOrganizationsStore } from '../stores/organizations';

const polygonsStore = usePolygonsStore();
const authStore = useAuthStore();
const organizationsStore = useOrganizationsStore();

// Инициализируем поля формы
const coordinates = ref('[[[55.75, 37.80], [55.80, 37.90], [55.75, 38.00], [55.70, 38.00], [55.70, 37.80]], [[55.75, 37.82], [55.75, 37.98], [55.65, 37.90]]]');
const fillColor = ref('#00FF00');
const strokeColor = ref('#0000FF');
const opacity = ref(0.5);
const strokeWidth = ref(5);
const strokeStyle = ref('shortdash');

const { public: { apiBase } } = useRuntimeConfig();

const createPolygon = async () => {
  try {
    await polygonsStore.createPolygon({
      coordinates: coordinates.value,
      fill_color: fillColor.value,
      stroke_color: strokeColor.value,
      opacity: opacity.value,
      stroke_width: strokeWidth.value,
      stroke_style: strokeStyle.value,
      user_id: authStore.user.id, // ID текущего пользователя
      organization_id: organizationsStore.currentOrganization.id // ID текущей организации
    });

    // Очистить поля формы
    coordinates.value = '[[[37.85, 55.75], [37.95, 55.8], [37.9, 55.85], [37.8, 55.8], [37.85, 55.75]]]';
    fillColor.value = '#00FF00';
    strokeColor.value = '#0000FF';
    opacity.value = 0.5;
    strokeWidth.value = 5;
    strokeStyle.value = 'shortdash';

    // Обновить список организаций (возможно, не нужно, если вы обновляете только текущую)
    await organizationsStore.fetchOrganizations();
  } catch (error) {
    console.error('Ошибка при создании полигона:', error);
  }
};

onMounted(async () => {
  // Получение организаций и отрисовка полигонов
  await organizationsStore.fetchOrganizations();
});

// Следим за изменениями в organizationsStore.currentOrganization
watch(
    () => organizationsStore.currentOrganization,
    (newOrganization) => {
      // Обновляем значения в форме, если есть данные в polygons[0]
      if (newOrganization?.polygons?.[0]) {
        coordinates.value = newOrganization.polygons[0].coordinates;
        fillColor.value = newOrganization.polygons[0].fill_color ?? '#00FF00';
        strokeColor.value = newOrganization.polygons[0].stroke_color ?? '#0000FF';
        opacity.value = newOrganization.polygons[0].opacity ?? 0.5;
        strokeWidth.value = newOrganization.polygons[0].stroke_width ?? 5;
        strokeStyle.value = newOrganization.polygons[0].stroke_style ?? 'shortdash';
      } else {
        // Сбрасываем значения в форму по умолчанию
        coordinates.value = '[[[37.85, 55.75], [37.95, 55.8], [37.9, 55.85], [37.8, 55.8], [37.85, 55.75]]]';
        fillColor.value = '#00FF00';
        strokeColor.value = '#0000FF';
        opacity.value = 0.5;
        strokeWidth.value = 5;
        strokeStyle.value = 'shortdash';
      }
    },
    {immediate: true} // Сразу вызываем watch при монтировании
);

</script>