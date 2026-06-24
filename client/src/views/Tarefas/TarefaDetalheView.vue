<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import {
  ArrowLeft,
  CalendarDays,
  CheckCircle2,
  ClipboardList,
  Edit,
  Leaf,
  Sprout,
  Trash2,
  User2,
} from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { deleteTarefa, getTarefaById, updateTarefa } from '@/services/Tarefa/tarefa.service'
import { statusClasses, statusLabel } from '@/services/Tarefa/tarefaLabels'
import type { ApiResponse } from '@/types/api'
import type { Task } from '@/types/tarefa'

const route = useRoute()
const router = useRouter()

const tarefaId = Number(route.params.id)
const tarefa = ref<Task | null>(null)
const isLoading = ref(true)
const loadError = ref<string | null>(null)

function extractData<T>(response: ApiResponse<T> | { data?: ApiResponse<T> } | null | undefined): T | null {
  if (!response) return null

  if ('data' in response && response.data && typeof response.data === 'object' && 'data' in response.data) {
    return response.data.data ?? null
  }

  return (response as ApiResponse<T>).data ?? null
}

async function loadTarefa() {
  try {
    isLoading.value = true
    loadError.value = null

    const response = await getTarefaById(tarefaId)
    tarefa.value = extractData<Task>(response)
  } catch {
    loadError.value = 'Nao foi possivel carregar a tarefa.'
  } finally {
    isLoading.value = false
  }
}

async function deleteTask() {
  if (!tarefa.value || !confirm('Tem certeza que deseja excluir esta tarefa?')) {
    return
  }

  try {
    await deleteTarefa(tarefa.value.id)
    router.push('/tarefas')
  } catch {
    loadError.value = 'Nao foi possivel excluir a tarefa.'
  }
}

async function completeTask() {
  if (!tarefa.value) {
    return
  }

  try {
    await updateTarefa(tarefa.value.id, {
      status: 'Concluido',
      completedAt: new Date().toISOString().slice(0, 10),
    })
    router.push('/tarefas')
  } catch {
    loadError.value = 'Nao foi possivel concluir a tarefa.'
  }
}

onMounted(loadTarefa)
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-5xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <Button as-child variant="outline" size="sm" class="w-fit">
        <RouterLink to="/tarefas">
          <ArrowLeft class="h-4 w-4" />
          Voltar
        </RouterLink>
      </Button>

      <div v-if="loadError" class="rounded-lg border border-amber-200 bg-amber-50 p-3 text-sm text-amber-800">
        {{ loadError }}
      </div>

      <section v-if="isLoading" class="rounded-xl border border-dashed bg-white p-10 text-center shadow-sm">
        <p class="text-sm text-gray-600">Carregando tarefa...</p>
      </section>

      <section v-else-if="!tarefa" class="rounded-xl border border-dashed bg-white p-10 text-center shadow-sm">
        <div class="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10 text-primary">
          <ClipboardList class="h-6 w-6" />
        </div>
        <h1 class="text-xl font-semibold text-gray-900">Tarefa nao encontrada</h1>
        <p class="mt-2 text-sm text-gray-600">Volte para a lista e selecione uma atividade disponivel.</p>
      </section>

      <template v-else>
        <section class="rounded-xl border bg-white p-6 shadow-sm">
          <div class="flex flex-col gap-5 lg:flex-row lg:items-start lg:justify-between">
            <div class="max-w-3xl">
              <span class="inline-flex items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
                <ClipboardList class="h-4 w-4" />
                {{ tarefa.tipo }}
              </span>
              <h1 class="mt-4 text-2xl font-bold text-gray-900">{{ tarefa.descricao || `Tarefa #${tarefa.id}` }}</h1>
            </div>

            <span class="inline-flex items-center rounded-full border px-3 py-1 text-sm font-medium" :class="statusClasses(tarefa.status)">
              {{ statusLabel(tarefa.status) }}
            </span>
          </div>
        </section>

        <section class="grid gap-4 md:grid-cols-2">
          <article class="rounded-xl border bg-white p-5 shadow-sm">
            <h2 class="text-base font-semibold text-gray-900">Vinculos</h2>
            <div class="mt-4 space-y-3 text-sm text-gray-700">
              <p class="flex items-center gap-2">
                <Sprout class="h-4 w-4 text-green-600" />
                {{ tarefa.horta?.nome ?? `Horta #${tarefa.hortaId}` }}
              </p>
              <p class="flex items-center gap-2">
                <Leaf class="h-4 w-4 text-lime-600" />
                {{ tarefa.plantio?.especie?.nome ?? 'Sem plantio relacionado' }}
              </p>
              <p class="flex items-center gap-2">
                <User2 class="h-4 w-4 text-blue-600" />
                {{ tarefa.membro?.profile?.name ?? tarefa.membro?.profile?.email ?? 'Sem responsavel' }}
              </p>
              <p class="flex items-center gap-2">
                <CalendarDays class="h-4 w-4 text-amber-600" />
                Data limite: {{ tarefa.dataLimite }}
              </p>
              <p v-if="tarefa.completedAt" class="flex items-center gap-2">
                <CheckCircle2 class="h-4 w-4 text-green-600" />
                Concluida em: {{ tarefa.completedAt }}
              </p>
            </div>
          </article>

          <article class="rounded-xl border bg-white p-5 shadow-sm">
            <h2 class="text-base font-semibold text-gray-900">Acoes</h2>
            <div class="mt-4 grid gap-2 sm:grid-cols-2">
              <Button
                variant="outline"
                class="justify-start border-green-200 text-green-700 hover:bg-green-50 hover:text-green-800"
                :disabled="tarefa.status === 'Concluido'"
                @click="completeTask"
              >
                <CheckCircle2 class="h-4 w-4" />
                {{ tarefa.status === 'Concluido' ? 'Tarefa concluida' : 'Concluir tarefa' }}
              </Button>
              <Button as-child variant="outline" class="justify-start">
                <RouterLink :to="`/tarefas/nova?edit=${tarefa.id}`">
                  <Edit class="h-4 w-4" />
                  Editar
                </RouterLink>
              </Button>
              <Button
                variant="outline"
                class="justify-start text-red-600 hover:bg-red-50 hover:text-red-700 sm:col-span-2"
                @click="deleteTask"
              >
                <Trash2 class="h-4 w-4" />
                Excluir
              </Button>
            </div>
          </article>
        </section>
      </template>
    </div>
  </main>
</template>