<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter, RouterLink } from 'vue-router'
import { ArrowLeft, Search, UserPlus } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { addMembro, getUsuariosDisponiveisParaHorta } from '@/services/Membro/membro.service'
import { getApiErrorMessage } from '@/services/api'

type Usuario = {
  id: string
  name: string
  email: string
}

const route = useRoute()
const router = useRouter()
const hortaId = Number(route.params.id)

const usuarios = ref<Usuario[]>([])
const isLoading = ref(true)
const loadError = ref<string | null>(null)
const searchTerm = ref('')

const selectedPerfilId = ref('')
const selectedRole = ref<'Membro' | 'Admin'>('Membro')
const isSubmitting = ref(false)
const submitError = ref<string | null>(null)

function extractData<T>(response: any): T | null {
  if (!response) return null
  if (response.data && typeof response.data === 'object' && 'data' in response.data) {
    return response.data.data ?? null
  }
  return response.data ?? null
}

async function loadUsuarios() {
  try {
    isLoading.value = true
    loadError.value = null

    const response = await getUsuariosDisponiveisParaHorta(hortaId)
    usuarios.value = extractData<Usuario[]>(response) ?? []
  } catch {
    loadError.value = 'Nao foi possivel carregar os usuarios disponiveis.'
  } finally {
    isLoading.value = false
  }
}

const usuariosFiltrados = computed(() => {
  const term = searchTerm.value.trim().toLowerCase()
  if (!term) return usuarios.value

  return usuarios.value.filter(
    (u) => u.name?.toLowerCase().includes(term) || u.email?.toLowerCase().includes(term),
  )
})

function selecionar(perfilId: string) {
  selectedPerfilId.value = perfilId
}

async function confirmarAdicao() {
  if (!selectedPerfilId.value) return

  try {
    isSubmitting.value = true
    submitError.value = null

    await addMembro({
      hortaId,
      perfilId: selectedPerfilId.value,
      role: selectedRole.value,
    })

    router.push(`/horta/${hortaId}`)
  } catch (error) {
    submitError.value = getApiErrorMessage(error, 'Nao foi possivel adicionar este membro.')
  } finally {
    isSubmitting.value = false
  }
}

onMounted(loadUsuarios)
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-3xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <Button variant="outline" size="sm" class="w-fit" @click="router.back()">
        <ArrowLeft class="h-4 w-4" />
        Voltar
      </Button>

      <section class="rounded-xl border bg-white p-6 shadow-sm">
        <div class="flex items-start gap-4">
          <div class="flex h-11 w-11 shrink-0 items-center justify-center rounded-lg bg-primary/10 text-primary">
            <UserPlus class="h-5 w-5" />
          </div>

          <div>
            <h1 class="text-2xl font-bold text-gray-900">Adicionar membro</h1>
            <p class="mt-2 text-muted-foreground">
              Selecione um usuario que ainda nao faz parte desta horta.
            </p>
          </div>
        </div>
      </section>

      <section class="rounded-xl border bg-white p-5 shadow-sm">
        <label class="relative block">
          <Search class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
          <Input v-model="searchTerm" class="pl-9" placeholder="Buscar por nome ou email" />
        </label>

        <div v-if="loadError" class="mt-4 rounded-lg border border-amber-200 bg-amber-50 p-3 text-sm text-amber-800">
          {{ loadError }}
        </div>

        <div v-if="isLoading" class="mt-4 py-8 text-center text-sm text-gray-600">
          Carregando usuarios...
        </div>

        <div v-else-if="usuariosFiltrados.length === 0" class="mt-4 rounded-lg border border-dashed p-8 text-center text-sm text-gray-600">
          Nenhum usuario disponivel para adicionar (todos ja sao membros desta horta, ou nenhum corresponde a busca).
        </div>

        <div v-else class="mt-4 max-h-80 space-y-2 overflow-auto">
          <button
            v-for="usuario in usuariosFiltrados"
            :key="usuario.id"
            type="button"
            class="flex w-full items-center justify-between rounded-lg border p-3 text-left transition hover:border-primary/40"
            :class="selectedPerfilId === usuario.id ? 'border-primary bg-primary/5' : 'border-gray-200 bg-white'"
            @click="selecionar(usuario.id)"
          >
            <div class="min-w-0">
              <p class="truncate text-sm font-medium text-gray-900">{{ usuario.name }}</p>
              <p class="truncate text-xs text-gray-500">{{ usuario.email }}</p>
            </div>
            <span
              v-if="selectedPerfilId === usuario.id"
              class="ml-3 shrink-0 rounded-full bg-primary px-2 py-0.5 text-xs font-medium text-white"
            >
              Selecionado
            </span>
          </button>
        </div>
      </section>

      <section v-if="selectedPerfilId" class="rounded-xl border bg-white p-5 shadow-sm">
        <p class="text-sm font-medium text-gray-700">Funcao do novo membro</p>
        <div class="mt-3 flex gap-2">
          <button
            type="button"
            class="flex-1 rounded-lg border px-3 py-2 text-sm font-medium transition"
            :class="selectedRole === 'Membro' ? 'border-primary bg-primary/10 text-primary' : 'border-gray-200 text-gray-600'"
            @click="selectedRole = 'Membro'"
          >
            Membro
          </button>
          <button
            type="button"
            class="flex-1 rounded-lg border px-3 py-2 text-sm font-medium transition"
            :class="selectedRole === 'Admin' ? 'border-primary bg-primary/10 text-primary' : 'border-gray-200 text-gray-600'"
            @click="selectedRole = 'Admin'"
          >
            Admin
          </button>
        </div>

        <p v-if="submitError" class="mt-3 rounded-lg border border-red-200 bg-red-50 p-3 text-sm text-red-700">
          {{ submitError }}
        </p>

        <Button class="mt-4 w-full" :disabled="isSubmitting" @click="confirmarAdicao">
          {{ isSubmitting ? 'Adicionando...' : 'Adicionar a horta' }}
        </Button>
      </section>
    </div>
  </main>
</template>
