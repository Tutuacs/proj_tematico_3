<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { RouterLink } from 'vue-router'
import { ArrowRight, Leaf, MapPin, Plus, Sprout, Users } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { getHortas } from '@/services/Horta/horta.service'
import { getMembrosByPerfil } from '@/services/Membro/membro.service'
import { useAuthStore } from '@/stores/auth'

type HortaResumo = {
  id: number
  nome: string
  local: string
  descricao?: string
  countMembros: number
  countPlantios: number
}

const authStore = useAuthStore()

const hortas = ref<HortaResumo[]>([])
const isLoading = ref(true)
const errorMessage = ref<string | null>(null)
const isAdminEmAlgumaHorta = ref(false)

const totalHortas = computed(() => hortas.value.length)
const totalMembros = computed(() =>
  hortas.value.reduce((total, horta) => total + Number(horta.countMembros || 0), 0),
)
const totalPlantios = computed(() =>
  hortas.value.reduce((total, horta) => total + Number(horta.countPlantios || 0), 0),
)
const hortaMaisAtiva = computed(() => {
  if (!hortas.value.length) {
    return null
  }

  return [...hortas.value].sort((a, b) => b.countPlantios - a.countPlantios)[0]
})

onMounted(async () => {
  try {
    const res = await getHortas()
    hortas.value = res?.data ?? []
  } catch {
    errorMessage.value = 'Nao foi possivel carregar as hortas.'
  } finally {
    isLoading.value = false
  }

  try {
    const perfilId = authStore.user?.id
    if (perfilId) {
      const res = await getMembrosByPerfil(perfilId)
      const memberships = res?.data?.data ?? res?.data ?? []
      isAdminEmAlgumaHorta.value = memberships.some((m: any) => m.role === 'Admin' || m.role === 0)
    }
  } catch {
    isAdminEmAlgumaHorta.value = false
  }
})
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-screen-2xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <section class="rounded-xl border bg-white p-6 shadow-sm">
        <div class="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
          <div class="max-w-3xl">
            <span class="inline-flex items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
              <Sprout class="h-4 w-4" />
              Gestao de hortas
            </span>
            <h1 class="mt-4 text-3xl font-bold text-gray-900">Hortas</h1>
            <p class="mt-2 text-muted-foreground">
              Organize suas areas de cultivo, acompanhe membros vinculados e acesse os plantios de cada horta.
            </p>

            <Button v-if="isAdminEmAlgumaHorta" as-child class="mt-4 w-fit">
              <RouterLink to="/horta/nova">
                <Plus class="h-4 w-4" />
                Nova horta
              </RouterLink>
            </Button>
          </div>

          <div class="grid gap-3 sm:grid-cols-3 lg:w-[520px]">
            <div class="rounded-lg border bg-gray-50 p-4">
              <p class="text-xs font-medium text-gray-500">Total de hortas</p>
              <p class="mt-2 text-2xl font-bold text-gray-900">{{ totalHortas }}</p>
            </div>

            <div class="rounded-lg border bg-gray-50 p-4">
              <p class="text-xs font-medium text-gray-500">Membros</p>
              <p class="mt-2 text-2xl font-bold text-gray-900">{{ totalMembros }}</p>
            </div>

            <div class="rounded-lg border bg-gray-50 p-4">
              <p class="text-xs font-medium text-gray-500">Plantios</p>
              <p class="mt-2 text-2xl font-bold text-gray-900">{{ totalPlantios }}</p>
            </div>
          </div>
        </div>
      </section>

      <div v-if="isLoading" class="flex items-center justify-center rounded-xl border bg-white py-16 shadow-sm">
        <div class="text-center">
          <div class="inline-block h-12 w-12 animate-spin rounded-full border-b-2 border-green-600"></div>
          <p class="mt-4 text-muted-foreground">Carregando hortas...</p>
        </div>
      </div>

      <div v-else-if="errorMessage" class="rounded-lg border border-red-200 bg-red-50 p-4">
        <p class="text-red-800">{{ errorMessage }}</p>
      </div>

      <div v-else-if="hortas.length === 0" class="rounded-xl border border-dashed bg-white p-10 text-center shadow-sm">
        <div class="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10 text-primary">
          <Leaf class="h-6 w-6" />
        </div>
        <h2 class="text-lg font-semibold text-gray-900">Nenhuma horta encontrada</h2>
        <p class="mx-auto mt-2 max-w-md text-sm text-muted-foreground">
          Quando uma horta for cadastrada por um administrador, ela aparecera aqui para consulta.
        </p>
      </div>

      <div v-else class="grid gap-6 xl:grid-cols-[minmax(0,1fr)_320px]">
        <section class="space-y-4">
          <div class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            <RouterLink
              v-for="horta in hortas"
              :key="horta.id"
              :to="`/horta/${horta.id}`"
              class="group flex min-h-52 flex-col justify-between rounded-xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:border-primary/40 hover:shadow-md"
            >
              <div>
                <div class="mb-4 flex items-start justify-between gap-3">
                  <div class="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10 text-primary">
                    <Sprout class="h-5 w-5" />
                  </div>

                  <ArrowRight class="h-5 w-5 text-gray-400 transition group-hover:translate-x-1 group-hover:text-primary" />
                </div>

                <h2 class="text-lg font-semibold text-gray-900">{{ horta.nome }}</h2>
                <p class="mt-2 flex items-center gap-2 text-sm text-gray-600">
                  <MapPin class="h-4 w-4 shrink-0 text-gray-400" />
                  <span class="truncate">{{ horta.local }}</span>
                </p>
                <p v-if="horta.descricao" class="mt-3 line-clamp-2 text-sm text-gray-500">
                  {{ horta.descricao }}
                </p>
              </div>

              <div class="mt-5 flex flex-wrap gap-2 text-xs">
                <span class="inline-flex items-center gap-1 rounded-full bg-blue-50 px-2.5 py-1 font-medium text-blue-700">
                  <Users class="h-3.5 w-3.5" />
                  {{ horta.countMembros }} membros
                </span>
                <span class="inline-flex items-center gap-1 rounded-full bg-green-50 px-2.5 py-1 font-medium text-green-700">
                  <Leaf class="h-3.5 w-3.5" />
                  {{ horta.countPlantios }} plantios
                </span>
              </div>
            </RouterLink>
            </div>
        </section>

        <aside class="h-fit rounded-xl border bg-white p-5 shadow-sm xl:sticky xl:top-6">
          <h2 class="text-base font-semibold text-gray-900">Resumo geral</h2>
          <p class="mt-1 text-sm text-gray-600">Indicadores rapidos das hortas cadastradas.</p>

          <div class="mt-5 space-y-3">
            <div class="rounded-lg bg-gray-50 p-3">
              <p class="text-xs font-medium text-gray-500">Horta mais ativa</p>
              <p class="mt-1 text-sm font-semibold text-gray-900">
                {{ hortaMaisAtiva?.nome || '-' }}
              </p>
              <p class="text-xs text-gray-600">
                {{ hortaMaisAtiva ? `${hortaMaisAtiva.countPlantios} plantios` : '-' }}
              </p>
            </div>

            <div class="rounded-lg border border-green-200 bg-green-50 p-3">
              <p class="text-xs font-semibold text-green-800">Organizacao</p>
              <p class="mt-1 text-xs text-green-700">
                Clique em uma horta para acessar detalhes, membros e plantios relacionados.
              </p>
            </div>
          </div>
        </aside>
      </div>
    </div>
  </main>
</template>
