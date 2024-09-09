import { defineStore } from 'pinia';
import { useAuthStore } from '~/stores/auth'; // Импортируйте useAuthStore

export const useOrganizationsStore = defineStore('organizations', {
  state: () => ({
    organizations: [],
    currentOrganization: null, // Добавьте состояние для текущей организации
  }),
  actions: {
    async fetchOrganizations() {
		const { public: { apiBase } } = useRuntimeConfig();
      try {
        const response = await fetch(`${apiBase}/api/organizations`); // Убираем Authorization
        if (response.ok) {
          this.organizations = await response.json();
        }
      } catch (error) {
        console.error('Ошибка при получении списка организаций:', error);
      }
    },
    addOrganizationToCurrent(organization) {
      this.currentOrganization = organization;
    },
    async createOrganization(organizationData) {
		const { public: { apiBase } } = useRuntimeConfig();
      try {
        const authStore = useAuthStore(); 
        const response = await fetch(`${apiBase}/api/organizations`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${authStore.token}`
          },
          body: JSON.stringify(organizationData),
        });

        if (response.ok) {
          await this.fetchOrganizations(); // Обновление списка организаций
          return true;
        } else {
          throw new Error(`Ошибка при создании организации: ${response.status}`);
        }
      } catch (error) {
        console.error('Ошибка при создании организации:', error);
        throw error; // Передача ошибки наверх
      }
    },
  },
});