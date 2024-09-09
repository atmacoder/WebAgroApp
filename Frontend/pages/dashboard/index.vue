<template>
  <div class="container mx-auto p-4">
    <p v-if="authStore.isLoggedIn">
      Привет, <span class="font-medium">{{ authStore.user.name }}</span>!
    </p>

    <p v-else>
      Вы не авторизованны <NuxtLink to="/login">Войдите</NuxtLink> чтобы получить доступ.
    </p>

<!--    <OrganizationAddForm v-if="authStore.isLoggedIn" />-->
  </div>
</template>

<script setup>
import { useAuthStore } from '~/stores/auth' 
import OrganizationAddForm from '~/components/dashboard/formOrganizationAdd.vue';
import formPaligonAdd from '~/components/dashboard/formPaligonAdd.vue';

const authStore = useAuthStore()
</script>