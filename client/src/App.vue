<script setup lang="ts">
import { RouterView } from 'vue-router'
import { useRoute } from 'vue-router'
import { Sonner } from '@/components/ui/sonner'
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from '@/components/ui/sidebar'
import AppSidebar from '@/components/layout/AppSidebar.vue'

const route = useRoute()
</script>

<template>
  <RouterView v-if="route.path === '/login'" />

  <SidebarProvider v-else>
    <AppSidebar />

    <SidebarInset>
      <header class="p-4">
        <SidebarTrigger />
      </header>
      <RouterView />
    </SidebarInset>
  </SidebarProvider>
  <Sonner />
</template>

<style scoped>
</style>
