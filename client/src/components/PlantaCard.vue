<script setup lang="ts">
import { ref } from "vue";

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

const props = defineProps<{
  planta: Planta;
}>();

const isExpanded = ref(false);

const mesesNomes = {
  "1": "Jan",
  "2": "Fev",
  "3": "Mar",
  "4": "Abr",
  "5": "Mai",
  "6": "Jun",
  "7": "Jul",
  "8": "Ago",
  "9": "Set",
  "10": "Out",
  "11": "Nov",
  "12": "Dez",
};

function formatarMesesTexto(mesesStr?: string | null): string {
  if (!mesesStr) {
    return "Não informado";
  }

  return mesesStr
    .split(",")
    .map((mes) => mesesNomes[mes.trim() as keyof typeof mesesNomes] || mes.trim())
    .join(" • ");
}

function toggleExpand() {
  isExpanded.value = !isExpanded.value;
}
</script>

<template>
  <div class="w-full">
    <div class="flex w-full items-stretch gap-3">
      <div class="relative h-36 min-w-0 flex-1 overflow-hidden rounded-xl border bg-white shadow-sm transition-all duration-300 hover:shadow-md">
        <div class="flex h-full">
          <div class="w-32 shrink-0 overflow-hidden rounded-l-xl">
          <img
            v-if="planta.imageLink"
            :src="planta.imageLink"
            :alt="planta.nome"
            class="h-full w-full object-contain bg-gray-50 p-2"
          />
          <div v-else class="h-full w-full bg-gray-100 flex items-center justify-center">
            <span class="text-4xl">🌱</span>
          </div>
          </div>

          <div class="min-w-0 flex-1 p-4 pr-12">
            <h3 class="truncate text-lg font-bold text-gray-800">{{ planta.nome }}</h3>
            <p class="mt-1 line-clamp-2 text-xs text-gray-600">{{ planta.descricao }}</p>

            <div class="mt-4 grid grid-cols-[130px_1fr] items-center gap-x-3 gap-y-2 text-sm">
              <span class="font-semibold text-gray-700">Rega:</span>
              <span class="font-bold text-blue-700">A cada {{ planta.diasParaRegar }} dias</span>

              <span class="font-semibold text-gray-700">Colheita:</span>
              <span class="font-bold text-amber-700">{{ planta.diasParaColher }} dias</span>
            </div>
          </div>

          <div class="absolute right-2 top-1/2 -translate-y-1/2">
          <button
            @click="toggleExpand"
            class="p-2 hover:bg-gray-100 rounded-lg transition-colors"
            :title="isExpanded ? 'Recolher' : 'Expandir'"
          >
            <svg
              class="w-6 h-6 text-gray-600 transition-transform duration-300"
              :style="{ transform: isExpanded ? 'rotate(180deg)' : 'rotate(0deg)' }"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
            </svg>
          </button>
          </div>
        </div>
      </div>

      <div
        v-if="isExpanded"
        class="h-36 w-72 shrink-0 rounded-xl border bg-gray-50 p-3"
      >
        <div class="grid h-full grid-rows-3 gap-2 text-sm">
          <div class="grid grid-cols-[90px_1fr] items-center gap-x-2">
            <span class="font-semibold text-gray-700">Plantio:</span>
            <span class="truncate text-green-700">{{ formatarMesesTexto(planta.mesesPlantio) }}</span>
          </div>

          <div class="grid grid-cols-[90px_1fr] items-center gap-x-2">
            <span class="font-semibold text-gray-700">Colheita:</span>
            <span class="truncate text-orange-700">{{ formatarMesesTexto(planta.mesesColheita) }}</span>
          </div>

          <div class="grid grid-cols-[90px_1fr] items-center gap-x-2">
            <span class="font-semibold text-gray-700">Rega:</span>
            <span class="font-medium text-blue-700">A cada {{ planta.diasParaRegar }} dias</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
