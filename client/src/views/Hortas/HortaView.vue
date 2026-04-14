<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RouterLink } from 'vue-router'
import { getHortas } from '@/services/Horta/horta.service'

type HortaResumo = {
    id: number
    nome: string
    local: string
    descricao?: string
    countMembros: number
    countPlantios: number
}

const hortas = ref<HortaResumo[]>([])
const isLoading = ref(true)
const errorMessage = ref<string | null>(null)

onMounted(async () => {
    try {
        const res = await getHortas()
        hortas.value = res?.data ?? []
    } catch {
        errorMessage.value = 'Não foi possível carregar as hortas.'
    } finally {
        isLoading.value = false
    }
})
</script>

<template>
    <main class="flex-1 overflow-auto">
        <div class="mx-auto w-full max-w-screen-2xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
            <div>
                <h1 class="text-3xl font-bold">Hortas</h1>
                <p class="mt-2 text-muted-foreground">Selecione uma horta para ver os detalhes.</p>
            </div>

            <div v-if="isLoading" class="py-12 text-center text-muted-foreground">
                Carregando hortas...
            </div>

            <div v-else-if="errorMessage" class="rounded-lg border border-red-200 bg-red-50 p-4 text-red-800">
                {{ errorMessage }}
            </div>

            <div v-else-if="hortas.length === 0" class="rounded-lg border border-dashed p-8 text-center text-muted-foreground">
                Nenhuma horta encontrada.
            </div>

            <div v-else class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
                <RouterLink
                    v-for="horta in hortas"
                    :key="horta.id"
                    :to="`/horta/${horta.id}`"
                    class="rounded-xl border bg-white p-4 shadow-sm transition hover:shadow-md"
                >
                    <h2 class="text-lg font-semibold text-gray-800">{{ horta.nome }}</h2>
                    <p class="mt-1 text-sm text-gray-600">{{ horta.local }}</p>

                    <div class="mt-3 flex flex-wrap gap-2 text-xs">
                        <span class="rounded-full bg-blue-50 px-2 py-1 font-medium text-blue-700">
                            Membros: {{ horta.countMembros }}
                        </span>
                        <span class="rounded-full bg-green-50 px-2 py-1 font-medium text-green-700">
                            Plantios: {{ horta.countPlantios }}
                        </span>
                    </div>
                </RouterLink>
            </div>
        </div>
    </main>
</template>