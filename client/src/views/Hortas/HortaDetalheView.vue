<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue'
import { useRoute } from 'vue-router'
import HortaCard from '@/components/HortaCard.vue'
import { getHortaById, getPlantiosByHorta } from '@/services/Horta/horta.service'

type HortaResumo = {
  id: number
  nome: string
  local: string
  descricao?: string
  countMembros: number
  countPlantios: number
}

type PlantioItem = {
  quantidade: number
  especie?: {
    nome?: string
    imageLink?: string
    imagemLink?: string
    descricao?: string
  }
}

const route = useRoute()

const hortaId = computed(() => Number(route.params.id))
const horta = ref<HortaResumo | null>(null)
const plantios = ref<PlantioItem[]>([])
const isLoading = ref(true)
const errorMessage = ref<string | null>(null)

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
    const [hortaRes, plantiosRes] = await Promise.all([
      getHortaById(hortaId.value),
      getPlantiosByHorta(hortaId.value),
    ])

    horta.value = hortaRes?.data ?? null
    plantios.value = plantiosRes?.data ?? []
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

      <div v-else-if="plantios.length === 0" class="rounded-lg border border-dashed p-8 text-center text-muted-foreground">
        Nenhum plantio encontrado para esta horta.
      </div>

      <div v-else class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
        <HortaCard
          v-for="(item, index) in plantios"
          :key="index"
          :nome="item.especie?.nome || 'Espécie sem nome'"
          :descricao="item.especie?.descricao || `Quantidade: ${item.quantidade}`"
          :progresso="Math.min(item.quantidade, 100)"
          :status="item.quantidade > 0 ? 'Ativo' : 'Vazio'"
          :imagem="item.especie?.imageLink || item.especie?.imagemLink"
        />
      </div>
    </div>
  </main>
</template>
