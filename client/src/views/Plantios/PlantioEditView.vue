<script setup lang="ts">
import { onMounted, ref, computed } from "vue";
import { useRoute, useRouter } from "vue-router";
import {
  getPlantioById,
  updatePlantio,
} from "@/services/Plantio/plantio.service";
import { getApiErrorMessage } from "@/services/api";

const route = useRoute();
const router = useRouter();
const plantioId = Number(route.params.id);

const plantio = ref<any>(null);
const isLoading = ref(false);
const isSaving = ref(false);
const error = ref<string | null>(null);

const statusOptions = [
  {
    value: "Ativo",
    label: "Ativo",
    description: "Plantio em acompanhamento normal.",
    className: "border-green-200 bg-green-50 text-green-800",
    dotClass: "bg-green-500",
  },
  {
    value: "Colhido",
    label: "Colhido",
    description: "Ciclo finalizado com colheita realizada.",
    className: "border-blue-200 bg-blue-50 text-blue-800",
    dotClass: "bg-blue-500",
  },
  {
    value: "Perdido",
    label: "Perdido",
    description: "Plantio que não evoluiu como esperado.",
    className: "border-red-200 bg-red-50 text-red-800",
    dotClass: "bg-red-500",
  },
] as const;

function formatDateOnly(value?: string) {
  if (!value) return "-";

  const normalized = value.includes("T") ? value : `${value}T00:00:00`;
  const parsed = new Date(normalized);

  if (Number.isNaN(parsed.getTime())) return value;

  return parsed.toLocaleDateString("pt-BR");
}

async function load() {
  isLoading.value = true;
  try {
    const res = await getPlantioById(plantioId);
    plantio.value = res.data?.data || res.data;
  } catch (err) {
    error.value = getApiErrorMessage(err);
  } finally {
    isLoading.value = false;
  }
}

async function save() {
  try {
    isSaving.value = true;
    const payload: any = {
      quantidade: plantio.value.quantidade,
      status: plantio.value.status,
    };
    if (plantio.value.dataPlantio)
      payload.dataPlantio = plantio.value.dataPlantio;

    await updatePlantio(plantioId, payload);
    router.back();
  } catch (err) {
    error.value = getApiErrorMessage(err);
  } finally {
    isSaving.value = false;
  }
}

onMounted(load);

const remainingDays = computed<number | null>(() => {
  const dias = plantio.value?.especie?.diasParaColher;
  const dateStr =
    plantio.value?.dataPlantio ||
    plantio.value?.dataplantio ||
    plantio.value?.DataPlantio;
  if (dias == null || !dateStr) return null;

  const normalized = dateStr.includes("T") ? dateStr : `${dateStr}T00:00:00`;
  const planted = new Date(normalized);
  if (Number.isNaN(planted.getTime())) return null;

  const now = new Date();
  const elapsed = Math.floor(
    (now.getTime() - planted.getTime()) / (1000 * 60 * 60 * 24),
  );
  return dias - elapsed;
});

const remainingText = computed(() => {
  const r = remainingDays.value;
  if (r == null) return null;
  if (r > 1) return `Faltam ${r} dias para colheita`;
  if (r === 1) return "Falta 1 dia para colheita";
  if (r === 0) return "Colheita prevista para hoje";
  return `Passaram ${Math.abs(r)} dias da data prevista`;
});

const remainingBadgeClass = computed(() => {
  const r = remainingDays.value;
  if (r == null) return "bg-gray-100 text-gray-700";
  if (r > 7) return "bg-green-100 text-green-800";
  if (r > 0) return "bg-yellow-100 text-yellow-800";
  if (r === 0) return "bg-blue-100 text-blue-800";
  return "bg-red-100 text-red-800";
});
</script>

<template>
  <main class="container mx-auto p-4">
    <div v-if="isLoading" class="text-center py-12">Carregando...</div>

    <div v-else-if="error" class="text-red-600 bg-red-50 p-4 rounded">
      {{ error }}
    </div>

    <div v-else-if="plantio" class="space-y-6">
      <!-- (status selection moved into details card) -->

      <!-- Espécie Info Card -->
      <div class="bg-white border rounded-lg p-6">
        <h2 class="text-lg font-semibold mb-4">Informações da Espécie</h2>
        <div class="grid gap-6 sm:grid-cols-2">
          <div>
            <img
              v-if="plantio.especie?.imageLink"
              :src="plantio.especie.imageLink"
              :alt="plantio.especie.nome"
              class="w-full h-48 object-cover rounded-lg"
            />
            <div
              v-else
              class="w-full h-48 bg-gray-200 rounded-lg flex items-center justify-center"
            >
              <span class="text-4xl">🌱</span>
            </div>
          </div>
          <div>
            <div class="mb-4">
              <label class="block text-xs font-semibold text-gray-500 uppercase"
                >Nome da Espécie</label
              >
              <p class="mt-1 text-xl font-bold text-gray-900">
                {{ plantio.especie?.nome }}
              </p>
            </div>
            <div>
              <label class="block text-xs font-semibold text-gray-500 uppercase"
                >Descrição</label
              >
              <p class="mt-1 text-sm text-gray-700">
                {{ plantio.especie?.descricao || "Sem descrição" }}
              </p>
            </div>
          </div>
        </div>

        <div class="mt-6 grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          <div class="rounded-lg bg-gray-50 p-4 border">
            <p class="text-xs font-semibold uppercase text-gray-500">
              Dias para regar
            </p>
            <p class="mt-1 text-lg font-bold text-gray-900">
              {{ plantio.especie?.diasParaRegar ?? "-" }}
            </p>
          </div>
          <div class="rounded-lg bg-gray-50 p-4 border">
            <p class="text-xs font-semibold uppercase text-gray-500">
              Dias para colher
            </p>
            <p class="mt-1 text-lg font-bold text-gray-900">
              {{ plantio.especie?.diasParaColher ?? "-" }}
            </p>
          </div>
          <div class="rounded-lg bg-gray-50 p-4 border">
            <p class="text-xs font-semibold uppercase text-gray-500">
              Meses de plantio
            </p>
            <p class="mt-1 text-sm font-medium text-gray-900">
              {{ plantio.especie?.mesesPlantio || "-" }}
            </p>
          </div>
          <div class="rounded-lg bg-gray-50 p-4 border">
            <p class="text-xs font-semibold uppercase text-gray-500">
              Meses de colheita
            </p>
            <p class="mt-1 text-sm font-medium text-gray-900">
              {{ plantio.especie?.mesesColheita || "-" }}
            </p>
          </div>
        </div>
      </div>

      <!-- Plantio Details Card -->
      <div class="bg-white border rounded-lg p-6">
        <h2 class="text-lg font-semibold mb-4">Detalhes do Plantio</h2>
        <div class="grid gap-6 sm:grid-cols-2">
          <div>
            <label
              class="block text-xs font-semibold text-gray-500 uppercase mb-2"
              >Data de Plantio</label
            >
            <div class="flex items-center gap-4">
              <div class="flex-1">
                <label class="sr-only">Data de plantio</label>
                <div v-if="plantio.status === 'Colhido'">
                  <input
                    type="date"
                    v-model="plantio.dataPlantio"
                    class="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                  />
                </div>
                <div
                  v-else
                  class="text-sm text-gray-700 p-3 bg-gray-50 rounded"
                >
                  {{
                    formatDateOnly(
                      plantio.dataPlantio ||
                        plantio.dataplantio ||
                        plantio.DataPlantio,
                    )
                  }}
                </div>
              </div>

              <div
                v-if="remainingText"
                :class="['px-4 py-2 rounded-lg text-sm font-semibold', remainingBadgeClass]"
              >
                {{ remainingText }}
              </div>
            </div>
          </div>
          <div>
            <label
              class="block text-xs font-semibold text-gray-500 uppercase mb-2"
              >Quantidade</label
            >
            <input
              type="number"
              v-model.number="plantio.quantidade"
              placeholder="0"
              class="w-full px-3 py-2 border rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
            />
          </div>
          <div>
            <label
              class="block text-xs font-semibold text-gray-500 uppercase mb-2"
              >Status</label
            >
            <div class="grid gap-3">
              <button
                v-for="option in statusOptions"
                :key="option.value"
                type="button"
                @click="plantio.status = option.value"
                class="rounded-xl border p-3 text-left transition-all"
                :class="
                  plantio.status === option.value
                    ? option.className
                    : 'border-gray-200 bg-white text-gray-700 hover:border-gray-300 hover:bg-gray-50'
                "
              >
                <div class="flex items-center gap-3">
                  <span
                    class="h-3 w-3 rounded-full"
                    :class="option.dotClass"
                  ></span>
                  <div class="min-w-0">
                    <div class="flex items-center justify-between gap-3">
                      <p class="font-semibold text-sm">{{ option.label }}</p>
                      <span
                        v-if="plantio.status === option.value"
                        class="text-xs font-medium uppercase"
                        >Selecionado</span
                      >
                    </div>
                    <p class="mt-1 text-xs leading-5 opacity-80">
                      {{ option.description }}
                    </p>
                  </div>
                </div>
              </button>
            </div>
          </div>
          <div>
            <label
              class="block text-xs font-semibold text-gray-500 uppercase mb-2"
              >ID do plantio</label
            >
            <div class="text-sm text-gray-700 p-3 bg-gray-50 rounded">
              #{{ plantio.id }}
            </div>
          </div>
          <div>
            <label
              class="block text-xs font-semibold text-gray-500 uppercase mb-2"
              >Horta</label
            >
            <div class="text-sm text-gray-700 p-3 bg-gray-50 rounded">
              #{{ plantio.hortaId }}
            </div>
          </div>
        </div>
      </div>

      <!-- Actions -->
      <div class="flex gap-3 sticky bottom-0 bg-white p-4 rounded-lg border">
        <button
          @click="save"
          :disabled="isSaving"
          class="flex-1 px-4 py-2 bg-green-600 hover:bg-green-700 disabled:bg-gray-400 text-white font-medium rounded-lg transition-colors"
        >
          {{ isSaving ? "Salvando..." : "Salvar Alterações" }}
        </button>
        <button
          @click="$router.back()"
          :disabled="isSaving"
          class="flex-1 px-4 py-2 border border-gray-300 text-gray-700 font-medium rounded-lg hover:bg-gray-50 disabled:bg-gray-50 transition-colors"
        >
          Cancelar
        </button>
      </div>
    </div>

    <div v-else class="text-gray-600">Plantio não encontrado.</div>
  </main>
</template>
