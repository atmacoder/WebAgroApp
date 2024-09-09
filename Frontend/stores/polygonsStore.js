import { defineStore } from 'pinia';
import { useAuthStore } from '~/stores/auth';
import { useRuntimeConfig } from '#imports'; // Импортируем useRuntimeConfig

export const usePolygonsStore = defineStore('polygons', {
  state: () => ({
    // Возможно, вам понадобится хранить список полигонов, если вы их будете показывать
  }),
  actions: {
    async createPolygon(polygonData) {
      const { public: { apiBase } } = useRuntimeConfig(); // Получаем apiBase
      try {
        const authStore = useAuthStore();
        const response = await fetch(`${apiBase}/api/polygons`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${authStore.token}`
          },
          body: JSON.stringify(polygonData),
        });

        if (response.ok) {
          // Обновите список полигонов, если это необходимо
          // ...
          return true;
        } else {
          throw new Error(`Ошибка при создании полигона: ${response.status}`);
        }
      } catch (error) {
        console.error('Ошибка при создании полигона:', error);
        throw error;
      }
    },
  },
});