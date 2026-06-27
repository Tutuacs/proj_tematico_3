<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { useRoute, RouterLink } from 'vue-router'
import { UserPlus } from 'lucide-vue-next'
import HortaCard from '@/components/HortaCard.vue'
import { Button } from '@/components/ui/button'
import { getHortaById, getPlantiosByHorta } from '@/services/Horta/horta.service'
import { getMembrosByHorta } from '@/services/Membro/membro.service'
import { useAuthStore } from '@/stores/auth'

type HortaResumo = {
  id: number
  nome: string
  local: string
  descricao?: string
  countMembros: number
  countPlantios: number
}

type PlantioItem = {
  id: number
  quantidade: number
  status?: string
  especie?: {
    nome?: string
    imageLink?: string
    imagemLink?: string
    descricao?: string
  }
}

type MembroItem = {
  id: number
  role?: string
  perfilId?: string
  profile?: {
    name?: string
    email?: string
  }
}

const route = useRoute()
const authStore = useAuthStore()

const hortaId = computed(() => Number(route.params.id))
const horta = ref<HortaResumo | null>(null)
const plantios = ref<PlantioItem[]>([])
const membros = ref<MembroItem[]>([])
const isLoading = ref(true)
const errorMessage = ref<string | null>(null)

// Usuario logado e Admin desta horta especifica (nao de outra)
const isAdminDestaHorta = computed(() => {
  const perfilId = authStore.user?.id
  if (!perfilId) return false
  return membros.value.some((m) => m.perfilId === perfilId && (m.role === 'Admin' || m.role === 0 as any))
})

const membrosSorted = computed(() => {
  return [...membros.value].sort((a, b) => {
    if (a.role === 'Admin' && b.role !== 'Admin') return -1
    if (a.role !== 'Admin' && b.role === 'Admin') return 1
    return 0
  })
})

async function loadHortaData() {
  if (!Number.isFinite(hortaId.value) || hortaId.value <= 0) {
    errorMessage.value = 'ID de horta inválido.'
    isLoading.value = false
    plantios.value = []
    return
  }

  isLoading.value = true
  errorMessage.value = null

  try {
    const [hortaRes, plantiosRes, membrosRes] = await Promise.all([
      getHortaById(hortaId.value),
      getPlantiosByHorta(hortaId.value),
      getMembrosByHorta(hortaId.value),
    ])

    horta.value = hortaRes?.data ?? null
    plantios.value = plantiosRes?.data ?? []
    membros.value = membrosRes?.data?.data ?? []
  } catch {
    errorMessage.value = 'Não foi possível carregar os dados da horta.'
    horta.value = null
    plantios.value = []
  } finally {
    isLoading.value = false
  }
}

onMounted(loadHortaData)

watch(() => route.params.id, loadHortaData)
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-screen-2xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <div>
        <h1 class="text-3xl font-bold">{{ horta?.nome || `Horta #${hortaId}` }}</h1>
        <p class="mt-2 text-muted-foreground">{{ horta?.local || 'Visão geral da horta.' }}</p>

        <div v-if="horta" class="mt-3 flex flex-wrap gap-2 text-sm">
          <span class="rounded-full bg-blue-50 px-3 py-1 font-medium text-blue-700">
            Membros: {{ horta.countMembros }}
          </span>
          <span class="rounded-full bg-green-50 px-3 py-1 font-medium text-green-700">
            Plantios: {{ horta.countPlantios }}
          </span>
        </div>
      </div>

      <div v-if="isLoading" class="flex items-center justify-center py-12">
        <div class="text-center">
          <div class="inline-block h-12 w-12 animate-spin rounded-full border-b-2 border-green-600"></div>
          <p class="mt-4 text-muted-foreground">Carregando horta...</p>
        </div>
      </div>

      <div v-else-if="errorMessage" class="rounded-lg border border-red-200 bg-red-50 p-4">
        <p class="text-red-800">{{ errorMessage }}</p>
      </div>

      <div v-else>
        <section class="mb-8">
          <div class="mb-4 flex items-center justify-between gap-3">
            <h2 class="text-lg font-semibold">Membros</h2>
            <Button v-if="isAdminDestaHorta" as-child size="sm">
              <RouterLink :to="`/horta/${hortaId}/membros/adicionar`">
                <UserPlus class="h-4 w-4" />
                Adicionar membro
              </RouterLink>
            </Button>
          </div>
          <div v-if="membros.length === 0" class="text-muted-foreground py-4">Nenhum membro encontrado.</div>
          <div v-else class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
            <RouterLink v-for="(mem, idx) in membrosSorted" :key="mem.id ?? idx" :to="`/membro/${mem.id ?? idx}`" class="block">
              <div class="bg-white border rounded-lg p-4 hover:shadow-md hover:border-blue-300 transition-all h-full">
                <div class="flex items-start justify-between">
                  <div class="flex-1">
                    <div class="font-medium text-gray-900 truncate">{{ mem.profile?.name || mem.perfilId }}</div>
                    <div class="text-sm text-gray-500 truncate">{{ mem.profile?.email }}</div>
                  </div>
                  <span :class="[
                    'text-xs px-2 py-1 rounded ml-2 whitespace-nowrap',
                    mem.role === 'Admin' 
                      ? 'bg-purple-100 text-purple-700'
                      : 'bg-blue-100 text-blue-700'
                  ]">
                    {{ mem.role }}
                  </span>
                </div>
              </div>
            </RouterLink>
          </div>
        </section>

        <section v-if="plantios.length === 0" class="rounded-lg border border-dashed p-8 text-center text-muted-foreground">
          Nenhum plantio encontrado para esta horta.
        </section>

        <section v-else>
          <h2 class="text-lg font-semibold mb-4">Plantios</h2>
          <div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
            <RouterLink v-for="(item, index) in plantios" :key="item.id || index" :to="`/plantio/${item.id}`" class="block">
              <HortaCard
                :nome="item.especie?.nome || 'Espécie sem nome'"
                :descricao="item.especie?.descricao || `Quantidade: ${item.quantidade}`"
                :progresso="Math.min(item.quantidade || 0, 100)"
                :status="item.status || (item.quantidade && item.quantidade > 0 ? 'Ativo' : 'Vazio')"
                :imagem="item.especie?.imageLink || item.especie?.imagemLink"
              />
            </RouterLink>
          </div>
        </section>
      </div>
    </div>
  </main>
</template>
