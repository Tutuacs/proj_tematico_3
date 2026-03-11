<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()
</script>

<template>
  <main class="mx-auto max-w-3xl space-y-4 p-6">
    <h1 class="text-2xl font-semibold">Home</h1>
    <p v-if="authStore.isLoggedIn">
      Você está logado como <strong>{{ authStore.user?.email }}</strong>.
    </p>
    <p v-else>Você ainda não está autenticado.</p>
    <RouterLink class="underline" to="/login">Ir para login</RouterLink>
  </main>
</template>
