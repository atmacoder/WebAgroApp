<template>
  <div class="container mx-auto p-4">
    <p v-if="authStore.isLoggedIn">
      <formPaligonAdd v-if="organizations.currentOrganization" />
      <OrganizationList  />
    </p>

    <p v-else>
      You are not logged in. Please <NuxtLink to="/login">log in</NuxtLink> to access the dashboard.
    </p>


  </div>
</template>

<script setup>
import { useAuthStore } from '~/stores/auth'
import { useOrganizationsStore } from '~/stores/organizations'
import OrganizationList from '~/components/dashboard/OrganizationList.vue';
import formPaligonAdd from '~/components/dashboard/formPaligonAdd.vue';

const authStore = useAuthStore();
const organizations = useOrganizationsStore();
</script>