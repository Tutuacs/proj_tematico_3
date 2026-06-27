<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { Button } from '@/components/ui/button'
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from '@/components/ui/sidebar'
import { House, Leaf, ListTodo, Plus, Settings, Sprout, User2 } from 'lucide-vue-next'
import { computed } from 'vue'

const authStore = useAuthStore()
const route = useRoute()
const router = useRouter()

const menuItems = [
  { title: 'Página inicial', to: '/', icon: House },
  { title: 'Horta', to: '/horta', icon: Sprout },
  { title: 'Plantas', to: '/plantas', icon: Leaf },
  { title: 'Tarefas', to: '/tarefas', icon: ListTodo },
  { title: 'Configurações', to: '/configuracoes', icon: Settings },
]

const userDisplayName = computed(() => authStore.user?.name || authStore.user?.email || 'Usuário')
const isHortaRoute = computed(() => route.path === '/horta')
const isPlantasRoute = computed(() => route.path === '/plantas')
const isTarefasRoute = computed(() => route.path === '/tarefas')

function isMenuItemActive(path: string) {
  if (path === '/horta') {
    return route.path === '/horta' || route.path.startsWith('/horta/')
  }

  if (path === '/tarefas') {
    return route.path === '/tarefas' || route.path.startsWith('/tarefas/')
  }

  return route.path === path
}

function handleLogout() {
  authStore.logout()
  router.push('/login')
}
</script>

<template>
  <Sidebar>
    <SidebarHeader class="border-b p-4">
      <div class="flex items-center justify-between gap-2 px-2 py-1">
        <div class="flex min-w-0 items-center gap-2 text-sm font-medium">
          <User2 class="size-4 text-muted-foreground" />
          <span class="truncate">{{ userDisplayName }}</span>
        </div>

        <Button
          variant="outline"
          size="sm"
          class="transition-colors hover:border-red-300 hover:bg-red-50 hover:text-red-600"
          @click="handleLogout"
        >
          Sair
        </Button>
      </div>
    </SidebarHeader>

    <SidebarContent>
      <SidebarGroup>
        <SidebarGroupContent>
          <SidebarMenu>
            <SidebarMenuItem v-for="item in menuItems" :key="item.title">
              <SidebarMenuButton
                as-child
                :is-active="isMenuItemActive(item.to)"
                :tooltip="item.title"
                class="data-[active=true]:bg-primary/15 data-[active=true]:text-primary"
              >
                <RouterLink :to="item.to">
                  <component :is="item.icon" />
                  <span>{{ item.title }}</span>
                </RouterLink>
              </SidebarMenuButton>
            </SidebarMenuItem>
          </SidebarMenu>
        </SidebarGroupContent>
      </SidebarGroup>
    </SidebarContent>

    <SidebarFooter class="border-t p-4">
      <Button v-if="isHortaRoute" as-child variant="default" size="sm" class="w-full">
        <RouterLink to="/horta/nova" class="flex items-center gap-2">
          <Plus class="size-4" />
          Cadastrar horta
        </RouterLink>
      </Button>

      <Button v-if="isPlantasRoute" as-child variant="default" size="sm" class="w-full">
        <RouterLink to="/plantas/nova-especie" class="flex items-center gap-2">
          <Plus class="size-4" />
          Nova espécie
        </RouterLink>
      </Button>

      <Button v-if="isTarefasRoute" as-child variant="default" size="sm" class="w-full">
        <RouterLink to="/tarefas/nova" class="flex items-center gap-2">
          <Plus class="size-4" />
          Nova tarefa
        </RouterLink>
      </Button>
    </SidebarFooter>
  </Sidebar>
</template>
