<script setup lang="ts">
import { computed } from 'vue'
import { RouterLink, useRoute } from 'vue-router'
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
import { mockTasks } from '@/mocks/tarefas'
import type { TaskPriority, TaskStatus } from '@/types/tarefa'

const route = useRoute()

const tarefaId = computed(() => Number(route.params.id))
const tarefa = computed(() => mockTasks.find((task) => task.id === tarefaId.value) ?? null)

function statusClasses(status: TaskStatus) {
  return {
    Pendente: 'bg-amber-50 text-amber-700 border-amber-200',
    'Em andamento': 'bg-blue-50 text-blue-700 border-blue-200',
    Concluida: 'bg-green-50 text-green-700 border-green-200',
  }[status]
}

function priorityClasses(prioridade: TaskPriority) {
  return {
    Baixa: 'bg-gray-50 text-gray-700 border-gray-200',
    Media: 'bg-lime-50 text-lime-700 border-lime-200',
    Alta: 'bg-red-50 text-red-700 border-red-200',
  }[prioridade]
}
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

      <section v-if="!tarefa" class="rounded-xl border border-dashed bg-white p-10 text-center shadow-sm">
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
              <h1 class="mt-4 text-3xl font-bold text-gray-900">{{ tarefa.nome }}</h1>
              <p class="mt-2 text-muted-foreground">{{ tarefa.descricao }}</p>
            </div>

            <div class="flex flex-wrap gap-2">
              <span class="inline-flex items-center rounded-full border px-3 py-1 text-sm font-medium" :class="statusClasses(tarefa.status)">
                {{ tarefa.status }}
              </span>
              <span class="inline-flex items-center rounded-full border px-3 py-1 text-sm font-medium" :class="priorityClasses(tarefa.prioridade)">
                Prioridade {{ tarefa.prioridade.toLowerCase() }}
              </span>
            </div>
          </div>
        </section>

        <section class="grid gap-4 md:grid-cols-2">
          <article class="rounded-xl border bg-white p-5 shadow-sm">
            <h2 class="text-base font-semibold text-gray-900">Vinculos</h2>
            <div class="mt-4 space-y-3 text-sm text-gray-700">
              <p class="flex items-center gap-2">
                <Sprout class="h-4 w-4 text-green-600" />
                {{ tarefa.horta }}
              </p>
              <p class="flex items-center gap-2">
                <Leaf class="h-4 w-4 text-lime-600" />
                {{ tarefa.plantio }}
              </p>
              <p class="flex items-center gap-2">
                <User2 class="h-4 w-4 text-blue-600" />
                {{ tarefa.responsavel }}
              </p>
              <p class="flex items-center gap-2">
                <CalendarDays class="h-4 w-4 text-amber-600" />
                Data limite: {{ tarefa.data }}
              </p>
            </div>
          </article>

          <article class="rounded-xl border bg-white p-5 shadow-sm">
            <h2 class="text-base font-semibold text-gray-900">Acoes</h2>
            <div class="mt-4 grid gap-2 sm:grid-cols-2">
              <Button variant="outline" class="justify-start border-green-200 text-green-700 hover:bg-green-50 hover:text-green-800">
                <CheckCircle2 class="h-4 w-4" />
                Concluir tarefa
              </Button>
              <Button variant="outline" class="justify-start">
                <Edit class="h-4 w-4" />
                Editar
              </Button>
              <Button variant="outline" class="justify-start text-red-600 hover:bg-red-50 hover:text-red-700 sm:col-span-2">
                <Trash2 class="h-4 w-4" />
                Excluir
              </Button>
            </div>
          </article>
        </section>

        <section class="rounded-xl border bg-white p-5 shadow-sm">
          <h2 class="text-base font-semibold text-gray-900">Observacoes</h2>
          <ul class="mt-4 space-y-3">
            <li
              v-for="observacao in tarefa.observacoes"
              :key="observacao"
              class="rounded-lg border bg-gray-50 px-4 py-3 text-sm text-gray-700"
            >
              {{ observacao }}
            </li>
          </ul>
        </section>
      </template>
    </div>
  </main>
</template>
