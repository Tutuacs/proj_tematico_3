<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { RouterLink } from "vue-router";
import { ArrowRight, Leaf, Plus, Sprout, Users } from "lucide-vue-next";

import HortaCard from "@/components/HortaCard.vue";
import { useAuthStore } from "@/stores/auth";
import { getPlantiosByHorta } from "@/services/Horta/horta.service";

const authStore = useAuthStore();

const hortas = ref<any[]>([]);

const userName = computed(() => authStore.user?.name || authStore.user?.email || "visitante");

const totalPlantios = computed(() => hortas.value.length);

const totalQuantidade = computed(() =>
  hortas.value.reduce((total, horta) => total + Number(horta.quantidade || 0), 0),
);

const especiesAtivas = computed(
  () => new Set(hortas.value.map((horta) => horta.especie?.nome).filter(Boolean)).size,
);

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
    titulo: "Especies",
    valor: especiesAtivas.value,
    descricao: "tipos cadastrados",
    icon: Users,
  },
]);

onMounted(async () => {
  try {
    const res = await getPlantiosByHorta(1);

    hortas.value = res.data;
  } catch (error) {
    console.error(error);
  }
});
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto flex min-h-[calc(100vh-5rem)] w-full max-w-7xl flex-col justify-center gap-8 px-4 py-8 sm:px-6 lg:px-8">
      <section class="grid items-stretch gap-6 lg:grid-cols-[1.35fr_0.65fr]">
        <div class="rounded-lg border bg-card p-6 shadow-sm sm:p-8">
          <div class="flex h-full flex-col justify-between gap-8">
            <div class="space-y-4">
              <span class="inline-flex w-fit items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
                <Leaf class="h-4 w-4" />
                Painel da horta
              </span>

              <div class="max-w-3xl space-y-3">
                <h1 class="text-3xl font-bold tracking-normal text-foreground sm:text-4xl">
                  Pagina Inicial
                </h1>

                <p v-if="authStore.isLoggedIn" class="text-base text-muted-foreground sm:text-lg">
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

        <div class="grid gap-4 sm:grid-cols-3 lg:grid-cols-1">
          <article
            v-for="card in resumoCards"
            :key="card.titulo"
            class="rounded-lg border bg-card p-5 shadow-sm"
          >
            <div class="flex items-center justify-between gap-4">
              <div>
                <p class="text-sm font-medium text-muted-foreground">{{ card.titulo }}</p>
                <p class="mt-2 text-3xl font-bold">{{ card.valor }}</p>
                <p class="mt-1 text-sm text-muted-foreground">{{ card.descricao }}</p>
              </div>

              <div class="flex h-11 w-11 shrink-0 items-center justify-center rounded-md bg-primary/10 text-primary">
                <component :is="card.icon" class="h-5 w-5" />
              </div>
            </div>
          </article>
        </div>
      </section>

      <section class="rounded-lg border bg-card p-5 shadow-sm sm:p-6">
        <div class="mb-6 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
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
          class="grid gap-5 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4"
        >
          <HortaCard
            v-for="(h, index) in hortas"
            :key="index"
            :nome="h.especie.nome"
            :descricao="'Quantidade: ' + h.quantidade"
            :progresso="h.quantidade"
            :status="h.quantidade > 0 ? 'Ativo' : 'Vazio'"
            :imagem="h.especie.imagemLink"
          />
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
  </main>
</template>
