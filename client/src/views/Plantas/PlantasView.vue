<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import PlantaCard from "@/components/PlantaCard.vue";
import { getAllEspecies } from "@/services/Especie/especie.service";

interface Planta {
  id: number;
  nome: string;
  imageLink: string;
  diasParaRegar: number;
  diasParaColher: number;
  mesesPlantio: string;
  mesesColheita: string;
  descricao: string;
}

const plantas = ref<Planta[]>([]);
const isLoading = ref(true);
const error = ref<string | null>(null);

const totalPlantas = computed(() => plantas.value.length);

const totalMesesPlantio = computed(() =>
  plantas.value.reduce((acc, planta) => acc + (planta.mesesPlantio ? 1 : 0), 0),
);

const totalComImagem = computed(() =>
  plantas.value.reduce((acc, planta) => acc + (planta.imageLink ? 1 : 0), 0),
);

const mediaDiasRega = computed(() => {
  if (!plantas.value.length) {
    return 0;
  }

  const totalDias = plantas.value.reduce((acc, planta) => acc + (planta.diasParaRegar || 0), 0);
  return Math.round(totalDias / plantas.value.length);
});

const mediaDiasColheita = computed(() => {
  if (!plantas.value.length) {
    return 0;
  }

  const totalDias = plantas.value.reduce((acc, planta) => acc + (planta.diasParaColher || 0), 0);
  return Math.round(totalDias / plantas.value.length);
});

const plantaMaisRapida = computed(() => {
  if (!plantas.value.length) {
    return null;
  }

  return [...plantas.value].sort((a, b) => a.diasParaColher - b.diasParaColher)[0];
});

onMounted(async () => {
  try {
    const res = await getAllEspecies();
    plantas.value = res.data;
  } catch (err) {
    console.error(err);
    error.value = "Erro ao carregar as plantas";
  } finally {
    isLoading.value = false;
  }
});
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-screen-2xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <div>
        <h1 class="text-3xl font-bold">Plantas e Espécies</h1>
        <p class="text-muted-foreground mt-2 max-w-3xl">
          Compare espécies, entenda o ciclo de rega e colheita e escolha o que faz mais sentido para a sua horta.
        </p>
      </div>

      <div v-if="!isLoading && !error && plantas.length > 0" class="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Total de espécies</p>
          <p class="mt-2 text-2xl font-bold text-gray-900">{{ totalPlantas }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Média de rega</p>
          <p class="mt-2 text-2xl font-bold text-green-700">{{ mediaDiasRega }} dias</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Com imagem</p>
          <p class="mt-2 text-2xl font-bold text-blue-700">{{ totalComImagem }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Com mês de plantio</p>
          <p class="mt-2 text-2xl font-bold text-amber-700">{{ totalMesesPlantio }}</p>
        </div>
      </div>

      <!-- Loading State -->
      <div v-if="isLoading" class="flex items-center justify-center py-12">
        <div class="text-center">
          <div class="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-green-600"></div>
          <p class="mt-4 text-muted-foreground">Carregando plantas...</p>
        </div>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="rounded-lg border border-red-200 bg-red-50 p-4">
        <p class="text-red-800">{{ error }}</p>
      </div>

      <!-- Plants Content -->
      <div v-else>
        <div v-if="plantas.length > 0" class="grid gap-6 2xl:grid-cols-[minmax(0,1fr)_320px]">
          <div class="space-y-4 pb-2">
            <PlantaCard
              v-for="planta in plantas"
              :key="planta.id"
              :planta="planta"
            />
          </div>

          <aside class="h-fit rounded-xl border bg-white p-4 shadow-sm xl:sticky xl:top-6">
            <h2 class="text-base font-semibold text-gray-800">Resumo da Safra</h2>
            <p class="mt-1 text-sm text-gray-600">Visão rápida para comparar ciclo, rega e período ideal.</p>

            <div class="mt-4 space-y-3">
              <div class="rounded-lg bg-gray-50 p-3">
                <p class="text-xs font-medium text-gray-500">Total de espécies</p>
                <p class="text-lg font-bold text-gray-800">{{ totalPlantas }}</p>
              </div>

              <div class="rounded-lg bg-gray-50 p-3">
                <p class="text-xs font-medium text-gray-500">Média de dias para regar</p>
                <p class="text-lg font-bold text-gray-800">{{ mediaDiasRega }} dias</p>
              </div>

              <div class="rounded-lg bg-gray-50 p-3">
                <p class="text-xs font-medium text-gray-500">Média até a colheita</p>
                <p class="text-lg font-bold text-gray-800">{{ mediaDiasColheita }} dias</p>
              </div>

              <div class="rounded-lg bg-gray-50 p-3">
                <p class="text-xs font-medium text-gray-500">Colheita mais rápida</p>
                <p class="text-sm font-semibold text-gray-800">
                  {{ plantaMaisRapida?.nome || "-" }}
                </p>
                <p class="text-xs text-gray-600">
                  {{ plantaMaisRapida ? `${plantaMaisRapida.diasParaColher} dias` : "-" }}
                </p>
              </div>
            </div>

            <div class="mt-4 rounded-lg border border-amber-200 bg-amber-50 p-3">
              <p class="text-xs font-semibold text-amber-800">Dica de uso</p>
              <p class="mt-1 text-xs text-amber-700">
                Abra cada card para ver meses de plantio, colheita e frequência de rega sem perder o contexto da lista.
              </p>
            </div>
          </aside>
        </div>

        <p v-else class="py-12 text-center text-muted-foreground">
          Nenhuma planta disponível no momento.
        </p>
      </div>
    </div>
  </main>
</template>
