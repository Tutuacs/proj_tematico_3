<script setup lang="ts">
import type { Component } from 'vue'
import { CalendarDays, CheckCircle2 } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'

type TaskStatus = 'Pendente' | 'Em andamento' | 'Concluida'
type TaskPriority = 'Baixa' | 'Media' | 'Alta'

defineProps<{
  nome: string
  descricao: string
  status: TaskStatus
  prioridade: TaskPriority
  data: string
  icon: Component
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
    class="group flex min-h-56 flex-col justify-between rounded-xl border bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:border-primary/40 hover:shadow-md"
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

      <h2 class="text-lg font-semibold text-gray-900">{{ nome }}</h2>
      <p class="mt-2 line-clamp-2 text-sm text-gray-500">{{ descricao }}</p>

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

    <div class="mt-5 border-t pt-4">
      <Button
        variant="outline"
        size="sm"
        class="w-full border-green-200 text-green-700 transition-colors hover:bg-green-50 hover:text-green-800"
      >
        <CheckCircle2 class="h-4 w-4" />
        Marcar como concluida
      </Button>
    </div>
  </article>
</template>
