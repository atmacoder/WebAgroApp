<template>
  <div
      v-if="showModal"
      class="fixed inset-0 z-50 overflow-y-auto"
      aria-labelledby="modal-title"
      role="dialog"
      aria-modal="true" data-modal-toggle="modal"
  >
    <div class="flex items-end justify-center min-h-screen px-4 pt-4 pb-20 text-center sm:block sm:p-0">
      <div class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" aria-hidden="true"></div>

      <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">​</span>
      <div
          class="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full"
          role="dialog"
          aria-modal="true"
          aria-labelledby="modal-title"
          data-modal-toggle="modal"
      >
        <div class="bg-gray-50 px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
          <div class="sm:flex sm:items-start">
            <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                <h3 class="text-xl font-semibold text-gray-900 dark:text-white" id="modal-title">
                  {{title}}
                </h3>
                <div class="mt-2">
                  <p class="text-sm text-gray-500">
				  
	<img class="w-60 h-60 max-w-lg rounded-lg" :src="'https://api.mimap.ru/storage/'+content.logo_path" alt="image description">
			
			<div v-if="content.courier_count<10" class="p-4 mb-4 text-sm text-blue-800 rounded-lg bg-blue-50 dark:bg-gray-800 dark:text-blue-400" role="alert">
			  <span class="font-medium">(!) </span> Возможны проблемы с доставкой
			</div>

					<ul class="max-w-md space-y-1 text-gray-500 list-disc list-inside dark:text-gray-400">
					<li>
						id: {{ content.id }}
					</li>					
					<li>
						Тип доставки: {{ content.delivery_type }}
					</li>				
					<li>
						Количество курьеров: {{ content.courier_count }}
					</li>
		
				</ul>
                  </p>
                </div>
            </div>
            <div class="ml-auto"> <button type="button" class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white" @click="showModal = false">
                  <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                      <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"/>
                  </svg>
                  <span class="sr-only">Close modal</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import { useOrganizationsStore } from '../stores/organizations';

const organizationsStore = useOrganizationsStore();

const showModal = ref(false);
const title = computed(() => organizationsStore.currentOrganization?.name || '');
const content = computed(() => organizationsStore.currentOrganization || '');

//  Добавляем watch  внутри  Modal.vue
watch(
    () => organizationsStore.currentOrganization,
    (newOrganization) => {
      showModal.value = !!newOrganization; // Устанавливаем  showModal  в  true, если  newOrganization  существует
    },
    { immediate: true } // Запускаем watch  сразу при монтировании
);
</script>