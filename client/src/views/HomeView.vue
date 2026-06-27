<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { RouterLink } from "vue-router";
import { ArrowRight, CalendarDays, CheckCircle2, ClipboardList, Leaf, Plus, Sprout, Users2, Flower2 } from "lucide-vue-next";

import { useAuthStore } from "@/stores/auth";
import { getHortas, getPlantiosByHorta } from "@/services/Horta/horta.service";
import { getMembrosByHorta } from "@/services/Membro/membro.service";
import { getTarefas } from "@/services/Tarefa/tarefa.service";
import { statusClasses, statusLabel } from "@/services/Tarefa/tarefaLabels";
import type { Task } from "@/types/tarefa";

const authStore = useAuthStore();

const hortas = ref<any[]>([]);
const tarefas = ref<Task[]>([]);

const userName = computed(() => authStore.user?.name || authStore.user?.email || "visitante");

const totalPlantios = computed(() => 
  hortas.value.reduce((sum, h) => sum + (h.plantios?.length || 0), 0)
);

const totalQuantidade = computed(() =>
  hortas.value.reduce((sum, horta) => sum + (horta.plantios?.reduce((s: any, p: any) => s + (p.quantidade || 0), 0) || 0), 0)
);

const totalMembros = computed(() =>
  hortas.value.reduce((sum, h) => sum + (h.membros?.length || 0), 0)
);

const totalTarefas = computed(() => tarefas.value.length);

const resumoCards = computed(() => [
  {
    titulo: "Plantios",
    valor: totalPlantios.value,
    descricao: "registros ativos",
    icon: Sprout,
  },
  {
    titulo: "Mudas",
    valor: totalQuantidade.value,
    descricao: "unidades cultivadas",
    icon: Leaf,
  },
  {
    titulo: "Membros",
    valor: totalMembros.value,
    descricao: "membros das hortas",
    icon: Users2,
  },
  {
    titulo: "Tarefas",
    valor: totalTarefas.value,
    descricao: "atividades planejadas",
    icon: ClipboardList,
  },
]);

function extractData<T>(response: any): T | null {
  if (!response) return null
  if (response.data && typeof response.data === 'object' && 'data' in response.data) {
    return response.data.data ?? null
  }
  return response.data ?? null
}

function formatDate(date: string) {
  if (!date) return "-";

  const d = new Date(date);

  // Evita problemas de fuso horário para datas no formato YYYY-MM-DD
  if (isNaN(d.getTime())) {
    const [year, month, day] = date.split("-");
    return `${day}/${month}/${year}`;
  }

  return d.toLocaleDateString("pt-BR");
}

onMounted(async () => {
  try {
    const res = await getHortas();
    const hortasData = res.data || [];
    
    // Enriquecer cada horta com dados de plantios e membros
    const enrichedHortas = await Promise.all(
      hortasData.map(async (horta: any) => {
        try {
          const [plantiosRes, membrosRes] = await Promise.all([
            getPlantiosByHorta(horta.id),
            getMembrosByHorta(horta.id),
          ]);
          return {
            ...horta,
            plantios: plantiosRes.data || [],
            membros: membrosRes.data?.data || [],
          };
        } catch {
          return { ...horta, plantios: [], membros: [] };
        }
      })
    );
    
    hortas.value = enrichedHortas;
  } catch (error) {
    console.error(error);
  }

  try {
    const tarefasRes = await getTarefas();
    const tarefasData = extractData<Task[]>(tarefasRes) ?? [];
    tarefas.value = tarefasData.slice(0, 3);
  } catch (error) {
    console.error(error);
  }
});
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-7xl flex-col gap-6 px-4 py-6 sm:px-6 lg:px-8">
      <section class="grid items-start gap-5 lg:grid-cols-[minmax(0,1fr)_360px]">
        <div class="rounded-lg border bg-card p-5 shadow-sm sm:p-6">
          <div class="flex h-full flex-col gap-4">
            <div class="space-y-3">
              <span class="inline-flex w-fit items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
                <Leaf class="h-4 w-4" />
                Painel da horta
              </span>

              <div class="max-w-3xl space-y-3">
                <h1 class="text-2xl font-bold tracking-normal text-foreground sm:text-3xl">
                  Pagina Inicial
                </h1>

                <p v-if="authStore.isLoggedIn" class="text-sm text-muted-foreground sm:text-base">
                  Bem-vindo, <strong class="text-foreground">{{ userName }}</strong>. Acompanhe os plantios, especies e atividades da sua horta em um so lugar.
                </p>
              </div>
            </div>

            <div class="flex flex-col gap-3 sm:flex-row">
              <RouterLink
                to="/horta"
                class="inline-flex items-center justify-center gap-2 rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground transition hover:bg-primary/90"
              >
                Ver hortas
                <ArrowRight class="h-4 w-4" />
              </RouterLink>

              <RouterLink
                to="/plantas/nova-especie"
                class="inline-flex items-center justify-center gap-2 rounded-md border bg-background px-4 py-2 text-sm font-medium transition hover:bg-accent hover:text-accent-foreground"
              >
                <Plus class="h-4 w-4" />
                Nova especie
              </RouterLink>
            </div>
          </div>
        </div>

        <div class="grid gap-3 sm:grid-cols-2">
          <article
            v-for="card in resumoCards"
            :key="card.titulo"
            class="rounded-lg border bg-card p-4 shadow-sm"
          >
            <div class="flex items-center justify-between gap-3">
              <div class="min-w-0">
                <p class="text-xs font-medium text-muted-foreground">{{ card.titulo }}</p>
                <p class="mt-1 text-2xl font-bold leading-none">{{ card.valor }}</p>
                <p class="mt-1 truncate text-xs text-muted-foreground">{{ card.descricao }}</p>
              </div>

              <div class="flex h-9 w-9 shrink-0 items-center justify-center rounded-md bg-primary/10 text-primary">
                <component :is="card.icon" class="h-4 w-4" />
              </div>
            </div>
          </article>
        </div>
      </section>

      <div class="grid gap-6 xl:grid-cols-[minmax(0,0.95fr)_minmax(0,1.05fr)]">
        <section class="rounded-lg border bg-card p-5 shadow-sm sm:p-6">
          <div class="mb-5 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <h2 class="text-xl font-semibold">Tarefas Recentes</h2>
              <p class="mt-1 text-sm text-muted-foreground">
                Atividades planejadas para manter a rotina organizada.
              </p>
            </div>

            <RouterLink
              to="/tarefas"
              class="inline-flex items-center justify-center gap-2 rounded-md border bg-background px-3 py-2 text-sm font-medium transition hover:bg-accent hover:text-accent-foreground"
            >
              Ver tarefas
              <ArrowRight class="h-4 w-4" />
            </RouterLink>
          </div>

          <div class="grid gap-4 md:grid-cols-2 xl:grid-cols-1 2xl:grid-cols-2">
            <RouterLink
              v-for="tarefa in tarefas"
              :key="tarefa.id"
              :to="`/tarefas/${tarefa.id}`"
              class="rounded-lg border bg-card p-4 shadow-sm transition hover:border-primary/40 hover:shadow-md"
            >
              <div class="mb-4 flex items-start justify-between gap-3">
                <div class="flex h-10 w-10 items-center justify-center rounded-md bg-primary/10 text-primary">
                  <ClipboardList class="h-5 w-5" />
                </div>

                <span
                  class="rounded-full border px-2.5 py-1 text-xs font-medium"
                  :class="statusClasses(tarefa.status)"
                >
                  {{ statusLabel(tarefa.status) }}
                </span>
              </div>

              <h3 class="line-clamp-2 font-semibold text-foreground">{{ tarefa.descricao || `Tarefa #${tarefa.id}` }}</h3>

              <div class="mt-4 flex items-center gap-2 text-sm text-muted-foreground">
                <CalendarDays class="h-4 w-4 text-green-600" />
                <span>{{ formatDate(tarefa.dataLimite) }}</span>
              </div>

              <p class="mt-3 flex items-center gap-2 text-xs text-muted-foreground">
                <CheckCircle2 class="h-3.5 w-3.5" />
                Clique para acompanhar
              </p>
            </RouterLink>

            <p v-if="tarefas.length === 0" class="text-sm text-muted-foreground">
              Nenhuma tarefa cadastrada ainda.
            </p>
          </div>
        </section>

        <section class="rounded-lg border bg-card p-5 shadow-sm sm:p-6">
          <div class="mb-5 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
            <div>
              <h2 class="text-xl font-semibold">Hortas Ativas</h2>
              <p class="mt-1 text-sm text-muted-foreground">
                Plantios vinculados a sua horta principal.
              </p>
            </div>

            <RouterLink
              to="/plantas"
              class="inline-flex items-center justify-center gap-2 rounded-md border bg-background px-3 py-2 text-sm font-medium transition hover:bg-accent hover:text-accent-foreground"
            >
              Ver especies
              <ArrowRight class="h-4 w-4" />
            </RouterLink>
          </div>

          <div
            v-if="hortas.length > 0"
            class="grid gap-4 sm:grid-cols-2"
          >
            <RouterLink
              v-for="horta in hortas"
              :key="horta.id"
              :to="`/horta/${horta.id}`"
              class="rounded-lg border bg-card p-4 shadow-sm transition-shadow hover:shadow-md"
            >
              <h3 class="font-semibold text-foreground">{{ horta.nome }}</h3>
              <div class="mt-4 space-y-2">
                <div class="flex items-center gap-2 text-sm text-muted-foreground">
                  <Flower2 class="h-4 w-4 text-green-600" />
                  <span>{{ horta.plantios?.length || 0 }} plantio{{ (horta.plantios?.length || 0) !== 1 ? 's' : '' }}</span>
                </div>
                <div class="flex items-center gap-2 text-sm text-muted-foreground">
                  <Users2 class="h-4 w-4 text-blue-600" />
                  <span>{{ horta.membros?.length || 0 }} membro{{ (horta.membros?.length || 0) !== 1 ? 's' : '' }}</span>
                </div>
              </div>
              <p class="mt-3 text-xs text-muted-foreground">
                Clique para gerenciar
              </p>
            </RouterLink>
          </div>

          <div
            v-else
            class="flex min-h-56 flex-col items-center justify-center rounded-lg border border-dashed bg-muted/30 px-6 py-10 text-center"
          >
            <div class="mb-4 flex h-12 w-12 items-center justify-center rounded-md bg-primary/10 text-primary">
              <Sprout class="h-6 w-6" />
            </div>
            <h3 class="text-lg font-semibold">Nenhuma horta cadastrada ainda</h3>
            <p class="mt-2 max-w-md text-sm text-muted-foreground">
              Cadastre uma especie ou acesse a area de hortas para comecar a organizar seus plantios.
            </p>
          </div>
        </section>
      </div>
    </div>
  </main>
</template>