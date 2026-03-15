<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { Button } from '@/components/ui/button'
import { Sonner } from '@/components/ui/sonner'
import { computed } from 'vue'

const authStore = useAuthStore()
const router = useRouter()
const route = useRoute()
const showLogin = computed(() => route.path !== '/login' && !authStore.isLoggedIn)

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
</script>

<template>
  <header class="border-b">
    <div class="mx-auto flex max-w-5xl items-center justify-between p-4">
      <nav class="flex items-center gap-4">
        <RouterLink to="/">Home</RouterLink>
        <RouterLink v-if="showLogin" to="/login">Login</RouterLink>
      </nav>

      <div class="flex items-center gap-3">
        <span v-if="authStore.isLoggedIn" class="text-sm text-muted-foreground">
          {{ authStore.user?.email }}
        </span>
        <Button v-if="authStore.isLoggedIn" variant="outline" size="sm" @click="handleLogout">
          Sair
        </Button>
      </div>
    </div>
  </header>

  <RouterView />
  <Sonner />
</template>

<style scoped>
</style>
