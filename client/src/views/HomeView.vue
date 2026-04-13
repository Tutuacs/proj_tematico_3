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
  <main class="mx-auto max-w-5xl space-y-6 p-6">

    <h1 class="text-2xl font-semibold">Home</h1>

    <p v-if="authStore.isLoggedIn">
      Você está logado como <strong>{{ authStore.user?.email }}</strong>.
    </p>

    <RouterLink class="underline" to="/login">
      Ir para login
    </RouterLink>

    <!-- CARDS -->
    <div class="mt-6">
      <h2 class="text-lg font-semibold mb-4">Hortas Ativas</h2>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        
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
    </div>

  </main>
</template>