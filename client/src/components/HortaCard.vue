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
  <div class="bg-white rounded-xl border p-4 shadow-sm w-full h-full flex flex-col hover:shadow-md hover:border-green-300 transition-all">
    <div class="flex justify-between items-center mb-3">
      <div>
        <img
          v-if="imagem"
          :src="imagem"
          class="w-12 h-12 rounded object-cover"
        />
        <span v-else class="text-2xl">🌱</span>
      </div>
      <span class="text-xs px-2 py-1 rounded bg-green-100 text-green-700 whitespace-nowrap ml-2">
        {{ status }}
      </span>
    </div>

    <h3 class="font-semibold text-gray-800 line-clamp-2 mb-2 flex-shrink-0">
      {{ nome }}
    </h3>

    <p class="text-sm text-gray-500 mb-auto line-clamp-2 flex-grow">
      {{ descricao }}
    </p>

    <div class="pt-2 border-t">
      <div class="h-2 bg-gray-200 rounded-full">
        <div
          class="h-2 bg-green-600 rounded-full transition-all"
          :style="{ width: progresso + '%' }"
        ></div>
      </div>
      <p class="text-xs text-right mt-1 text-gray-500">
        {{ progresso }}%
      </p>
    </div>
  </div>
</template>