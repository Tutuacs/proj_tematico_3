<script setup lang="ts">
import type { Component } from 'vue'
import { CalendarDays, CheckCircle2, Edit, Eye, Trash2, User2, Sprout } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'

type TaskStatus = 'Pendente' | 'Em andamento' | 'Concluida'
type TaskPriority = 'Baixa' | 'Media' | 'Alta'

defineProps<{
  id: number
  nome: string
  descricao: string
  status: TaskStatus
  prioridade: TaskPriority
  data: string
  horta: string
  responsavel: string
  tipo: string
  icon: Component
}>()

defineEmits<{
  delete: [id: number]
  complete: [id: number]
}>()

function statusClasses(status: TaskStatus) {
  const classes = {
    Pendente: 'bg-amber-50 text-amber-700 border-amber-200',
    'Em andamento': 'bg-blue-50 text-blue-700 border-blue-200',
    Concluida: 'bg-green-50 text-green-700 border-green-200',
  }

  return classes[status]
}

function priorityClasses(prioridade: TaskPriority) {
  const classes = {
    Baixa: 'bg-gray-50 text-gray-700 border-gray-200',
    Media: 'bg-lime-50 text-lime-700 border-lime-200',
    Alta: 'bg-red-50 text-red-700 border-red-200',
  }

  return classes[prioridade]
}
</script>

<template>
  <article
    class="group flex min-h-72 flex-col justify-between rounded-xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:border-primary/40 hover:shadow-md"
  >
    <div>
      <div class="mb-4 flex items-start justify-between gap-3">
        <div class="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10 text-primary">
          <component :is="icon" class="h-5 w-5" />
        </div>

        <span
          class="inline-flex items-center rounded-full border px-2.5 py-1 text-xs font-medium"
          :class="statusClasses(status)"
        >
          {{ status }}
        </span>
      </div>

      <p class="mb-2 text-xs font-medium uppercase text-gray-400">{{ tipo }}</p>
      <h2 class="line-clamp-2 text-lg font-semibold text-gray-900">{{ nome }}</h2>
      <p class="mt-2 line-clamp-2 text-sm text-gray-500">{{ descricao }}</p>

      <div class="mt-4 space-y-2 text-sm text-gray-600">
        <p class="flex items-center gap-2">
          <Sprout class="h-4 w-4 shrink-0 text-green-600" />
          <span class="truncate">{{ horta }}</span>
        </p>
        <p class="flex items-center gap-2">
          <User2 class="h-4 w-4 shrink-0 text-blue-600" />
          <span class="truncate">{{ responsavel }}</span>
        </p>
      </div>

      <div class="mt-4 flex flex-wrap gap-2 text-xs">
        <span
          class="inline-flex items-center rounded-full border px-2.5 py-1 font-medium"
          :class="priorityClasses(prioridade)"
        >
          Prioridade {{ prioridade.toLowerCase() }}
        </span>
        <span class="inline-flex items-center gap-1 rounded-full bg-gray-50 px-2.5 py-1 font-medium text-gray-700">
          <CalendarDays class="h-3.5 w-3.5" />
          {{ data }}
        </span>
      </div>
    </div>

    <div class="mt-5 grid gap-2 border-t pt-4 sm:grid-cols-[1fr_auto]">
      <Button
        variant="outline"
        size="sm"
        class="w-full border-green-200 text-green-700 transition-colors hover:bg-green-50 hover:text-green-800"
        :disabled="status === 'Concluida'"
        @click="$emit('complete', id)"
      >
        <CheckCircle2 class="h-4 w-4" />
        {{ status === 'Concluida' ? 'Concluida' : 'Concluir' }}
      </Button>

      <div class="grid grid-cols-3 gap-1 sm:flex">
        <Button as-child variant="outline" size="sm" class="px-2" title="Ver detalhes">
          <RouterLink :to="`/tarefas/${id}`">
            <Eye class="h-4 w-4" />
          </RouterLink>
        </Button>
        <Button as-child variant="outline" size="sm" class="px-2" title="Editar">
          <RouterLink :to="`/tarefas/nova?edit=${id}`">
            <Edit class="h-4 w-4" />
          </RouterLink>
        </Button>
        <Button
          variant="outline"
          size="sm"
          class="px-2 text-red-600 hover:bg-red-50 hover:text-red-700"
          title="Excluir"
          @click="$emit('delete', id)"
        >
          <Trash2 class="h-4 w-4" />
        </Button>
      </div>
    </div>
  </article>
</template>
