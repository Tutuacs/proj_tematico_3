<script setup lang="ts">
defineProps<{
  nome: string
  descricao: string
  progresso: number
  status: string
  imagem?: string  
}>()
</script>

<template>
  <div class="bg-white rounded-xl border p-4 shadow-sm w-full">

    <div class="flex justify-between items-center mb-2">

      <div>
        <img
          v-if="imagem"
          :src="imagem"
          class="w-8 h-8 rounded object-cover"
        />
        <span v-else>🌱</span>
      </div>

      <span class="text-xs px-2 py-1 rounded bg-green-100 text-green-700">
        {{ status }}
      </span>

    </div>

    <h3 class="font-semibold text-gray-800">
      {{ nome }}
    </h3>

    <p class="text-sm text-gray-500 mb-3">
      {{ descricao }}
    </p>

    <div>
      <div class="h-2 bg-gray-200 rounded-full">
        <div
          class="h-2 bg-green-600 rounded-full"
          :style="{ width: progresso + '%' }"
        ></div>
      </div>

      <p class="text-xs text-right mt-1 text-gray-500">
        {{ progresso }}%
      </p>
    </div>

  </div>
</template>