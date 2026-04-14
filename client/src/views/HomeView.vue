<script setup lang="ts">
import { RouterLink } from "vue-router";
import { useAuthStore } from "@/stores/auth";
import HortaCard from "@/components/HortaCard.vue";
import { ref, onMounted } from "vue";
import { getPlantiosByHorta } from "@/services/Horta/horta.service";

const authStore = useAuthStore();

const hortas = ref<any[]>([]);

onMounted(async () => {
  try {
    const res = await getPlantiosByHorta(1);

    console.log("RESPOSTA COMPLETA:", res);
    console.log("DADOS:", res.data);

    hortas.value = res.data;

  } catch (error) {
    console.error(error);
  }
});
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-7xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <h1 class="text-3xl font-bold">Página Inicial</h1>

      <p v-if="authStore.isLoggedIn" class="text-muted-foreground">
        Bem-vindo, <strong>{{ authStore.user?.name || authStore.user?.email }}</strong>
      </p>

      <!-- CARDS -->
      <div class="mt-8">
        <h2 class="mb-6 text-xl font-semibold">Hortas Ativas</h2>

        <div class="grid gap-6 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
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

        <p v-if="hortas.length === 0" class="py-12 text-center text-muted-foreground">
          Nenhuma horta cadastrada ainda.
        </p>
      </div>
    </div>
  </main>
</template>