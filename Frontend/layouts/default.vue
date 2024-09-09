<script setup>
import { onMounted } from 'vue';
import { useFlowbite } from '~/composables/useFlowbite';
import { 
    initAccordions, 
    initCarousels, 
    initCollapses, 
    initDials, 
    initDismisses, 
    initDrawers, 
    initDropdowns, 
    initModals, 
    initPopovers, 
    initTabs, 
    initTooltips 
} from 'flowbite';

// Импорт компонентов Header, Footer, MainContent
import Header from '~/components/Header.vue'; 
import Footer from '~/components/Footer.vue'; 
import MainContent from '~/components/MainContent.vue'; 

onMounted(() => {
    if (process.client) { // Проверка на клиентскую сторону
        useFlowbite(() => {
            initAccordions();
            initCarousels();
            initCollapses();
            initDials();
            initDismisses();
            initDrawers();
            initDropdowns();
            initModals();
            initPopovers();
            initTabs();
            initTooltips();
        });
    }
});
</script>

<template>
  <div>
    <Header /> 
<!--	<Sidebar/>-->
    <MainContent>
      <slot /> 
    </MainContent>
    <Footer /> 
  </div>
</template>