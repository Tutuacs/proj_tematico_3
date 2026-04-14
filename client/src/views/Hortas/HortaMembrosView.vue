<script setup lang="ts">
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-screen-2xl px-4 py-6 sm:px-6 lg:px-8">
      <h1 class="text-2xl font-bold">Membros da Horta #{{ route.params.id }}</h1>
    </div>
  </main>
</template>
