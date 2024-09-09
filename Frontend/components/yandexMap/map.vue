<template>
  <div class="container-fluid mx-auto p-4">

    <yandex-map
        ref="mapRef"
        v-model="map"
        :settings="{
        location: {
          center: mapCenter,
          zoom: mapZoom,
        },
      }"
        width="100%"
        height="600px"
    >
      <yandex-map-default-scheme-layer />
      <yandex-map-default-features-layer />
      <yandex-map-hint hint-property="hint">
        <template #default="{ content }">
            <div
                class="hint-window"
                v-html="content"
            />
        </template>
      </yandex-map-hint>

      <template v-for="(organization, index) in organizations" :key="index">
        <yandex-map-feature
            v-if="organization.polygons && organization.polygons.length"
            :key="organization.id"
            :settings="getPolygonSettings(organization)"
            @click="showOrganizationInfo(organization, $event)"
        />
      </template>
    </yandex-map>

    <Modal v-model:visible="showModal" @close="showModal = false" />

  </div>
</template>

<script setup lang="ts">
import { shallowRef, ref, onMounted, computed, watch } from 'vue';
import type { YMap, IGeoObject, IMapEvent, YMapFeatureProps } from '@yandex/ymaps3-types';
import { YandexMap, YandexMapDefaultSchemeLayer, YandexMapFeature, YandexMapDefaultFeaturesLayer, YandexMapHint, YandexMapDefaultMarker, YandexMapControls, YandexMapZoomControl } from 'vue-yandex-maps';
import { useOrganizationsStore } from '../stores/organizations';
import Modal from '../components/Modal.vue';

const organizationsStore = useOrganizationsStore();
const mapRef = shallowRef<null | YMap>(null);
const mapCenter = ref([37.617644, 55.755819]); // Изначальный центр карты
const mapZoom = ref(9); // Изначальный зум карты
const showModal = ref(false);

const organizations = computed(() => {
  return organizationsStore.organizations;
});

const showOrganizationInfo = (organization: any, event: IMapEvent) => {
  console.log('Нажата организация:', organization);
  const feature = event.get('target') as any;
  console.log('Название организации:', feature.properties.get('balloonContent'));
  organizationsStore.currentOrganization = organization; // Установка текущей организации
  // showModal.value = true; // Открытие модального окна

  // Получить ссылку на Yandex.Map
  const mapInstance = mapRef.value as any;

  // Открыть балун
  mapInstance.openBalloon(feature, {
    content: organization.name, // Содержимое балуна
    // ... other balloon options
  });
};

onMounted(async () => {
  // Получение организаций и отрисовка полигонов
  await organizationsStore.fetchOrganizations();
  organizationsStore.currentOrganization = null;
});

const getPolygonSettings = (organization: any): YMapFeatureProps => {
  return {
    geometry: {
      type: 'Polygon',
      coordinates: JSON.parse(organization.polygons[0].coordinates),
      fillRule: 'nonZero'
    },
    style: {
      cursor: 'pointer',
      stroke: [{
        color: organization.polygons[0].stroke_color || '#0000FF',
        width: organization.polygons[0].stroke_width,
      }],
       fill: `rgba(${organization.polygons[0].stroke_color.slice(1)}, ${parseFloat(organization.polygons[0].opacity)})`,
	   fillOpacity: parseFloat(organization.polygons[0].opacity),
    },
    properties: {
      hint: organization.name, // Добавляем свойство hint
      balloonContent: organization.name, // Добавляем balloonContent
      balloonPanelMaxMapArea: 0.5 // Ограничиваем балун
    },
    onClick: (e: MouseEvent) => {
      organizationsStore.currentOrganization = organization; // Установка текущей организации
    }
  };
};

watch(
    () => organizationsStore.currentOrganization,
    (newOrganization) => {
      if (newOrganization && newOrganization.polygons && newOrganization.polygons.length) {
        const coordinates = JSON.parse(newOrganization.polygons[0].coordinates)[0][0];
        mapCenter.value = coordinates;
        mapZoom.value = 10;
        showModal.value = true; // Открытие модального окна
      }
    },
    { immediate: true }
);
</script>