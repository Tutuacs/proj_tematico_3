<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { RouterLink } from 'vue-router'
import { ListTodo, Plus, Search, TriangleAlert } from 'lucide-vue-next'
import TaskCard from '@/components/TaskCard.vue'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { deleteTarefa, getTarefas, updateTarefa } from '@/services/Tarefa/tarefa.service'
import { statusLabel, STATUS_OPTIONS, tipoIcon } from '@/services/Tarefa/tarefaLabels'
import type { ApiResponse } from '@/types/api'
import type { Task, TaskStatus } from '@/types/tarefa'

const isLoading = ref(true)
const loadError = ref<string | null>(null)

const selectedStatus = ref<'Todos' | TaskStatus>('Todos')
const selectedHorta = ref('Todas')
const searchTerm = ref('')

const tasks = ref<Task[]>([])

function extractData<T>(response: ApiResponse<T> | { data?: ApiResponse<T> } | null | undefined): T | null {
  if (!response) return null

  if ('data' in response && response.data && typeof response.data === 'object' && 'data' in response.data) {
    return response.data.data ?? null
  }

  return (response as ApiResponse<T>).data ?? null
}

async function loadTarefas() {
  try {
    isLoading.value = true
    loadError.value = null

    const response = await getTarefas()
    const data = extractData<Task[]>(response) ?? []

    tasks.value = data.map((task) => ({ ...task, icon: tipoIcon(task.tipo) }))
  } catch {
    loadError.value = 'Nao foi possivel carregar as tarefas.'
  } finally {
    isLoading.value = false
  }
}

const totalTarefas = computed(() => tasks.value.length)
const totalAbertas = computed(() => tasks.value.filter((task) => task.status === 'Aberto').length)
const totalConcluidas = computed(() => tasks.value.filter((task) => task.status === 'Concluido').length)
const totalVencidas = computed(() => {
  const hoje = new Date().toISOString().slice(0, 10)
  return tasks.value.filter((task) => task.status !== 'Concluido' && task.status !== 'Cancelado' && task.dataLimite < hoje).length
})

const hortas = computed(() => [
  'Todas',
  ...new Set(tasks.value.map((task) => task.horta?.nome).filter((nome): nome is string => Boolean(nome))),
])

const visibleTasks = computed(() => {
  const term = searchTerm.value.trim().toLowerCase()

  return tasks.value.filter((task) => {
    const matchesStatus = selectedStatus.value === 'Todos' || task.status === selectedStatus.value
    const matchesHorta = selectedHorta.value === 'Todas' || task.horta?.nome === selectedHorta.value
    const responsavel = task.membro?.profile?.name ?? task.membro?.profile?.email ?? ''
    const matchesSearch =
      !term ||
      (task.descricao ?? '').toLowerCase().includes(term) ||
      responsavel.toLowerCase().includes(term) ||
      task.tipo.toLowerCase().includes(term)

    return matchesStatus && matchesHorta && matchesSearch
  })
})

async function deleteTask(id: number) {
  if (!confirm('Tem certeza que deseja excluir esta tarefa?')) {
    return
  }

  try {
    await deleteTarefa(id)
    tasks.value = tasks.value.filter((task) => task.id !== id)
  } catch {
    loadError.value = 'Nao foi possivel excluir a tarefa.'
  }
}

async function completeTask(id: number) {
  try {
    await updateTarefa(id, {
      status: 'Concluido',
      completedAt: new Date().toISOString().slice(0, 10),
    })
    tasks.value = tasks.value.map((task) => (task.id === id ? { ...task, status: 'Concluido' } : task))
  } catch {
    loadError.value = 'Nao foi possivel concluir a tarefa.'
  }
}

onMounted(loadTarefas)
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-screen-2xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <section class="rounded-xl border bg-white p-6 shadow-sm">
        <div class="flex flex-col gap-5 lg:flex-row lg:items-end lg:justify-between">
          <div class="max-w-3xl">
            <span class="inline-flex items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
              <ListTodo class="h-4 w-4" />
              Gestao de tarefas
            </span>
            <h1 class="mt-4 text-3xl font-bold text-gray-900">Tarefas</h1>
            <p class="mt-2 text-muted-foreground">
              Organize atividades da horta, acompanhe prazos e mantenha a rotina de cuidados em dia.
            </p>
          </div>

          <Button as-child class="w-full sm:w-fit">
            <RouterLink to="/tarefas/nova">
              <Plus class="h-4 w-4" />
              Nova tarefa
            </RouterLink>
          </Button>
        </div>
      </section>

      <div v-if="loadError" class="rounded-lg border border-amber-200 bg-amber-50 p-3 text-sm text-amber-800">
        {{ loadError }}
      </div>

      <section class="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Total de tarefas</p>
          <p class="mt-2 text-2xl font-bold text-gray-900">{{ totalTarefas }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Abertas</p>
          <p class="mt-2 text-2xl font-bold text-amber-700">{{ totalAbertas }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Concluidas</p>
          <p class="mt-2 text-2xl font-bold text-green-700">{{ totalConcluidas }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Vencidas</p>
          <p class="mt-2 text-2xl font-bold text-slate-800">{{ totalVencidas }}</p>
        </div>
      </section>

      <div class="grid gap-6 xl:grid-cols-[minmax(0,1fr)_320px]">
        <section class="space-y-4">
          <div class="space-y-4 rounded-xl border bg-white p-4 shadow-sm">
            <div>
              <h2 class="text-base font-semibold text-gray-900">Lista de tarefas</h2>
              <p class="mt-1 text-sm text-gray-600">Encontre atividades por status, horta, tipo ou responsavel.</p>
            </div>

            <div class="grid gap-3 lg:grid-cols-[minmax(220px,1fr)_180px]">
              <label class="relative block">
                <Search class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
                <Input v-model="searchTerm" class="pl-9" placeholder="Buscar por descricao, tipo ou responsavel" />
              </label>

              <select
                v-model="selectedHorta"
                class="h-9 rounded-md border border-input bg-white px-3 text-sm shadow-xs outline-none focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
              >
                <option v-for="horta in hortas" :key="horta" :value="horta">
                  {{ horta === 'Todas' ? 'Todas hortas' : horta }}
                </option>
              </select>
            </div>

            <div class="flex flex-wrap gap-2">
              <button
                type="button"
                class="inline-flex items-center rounded-full border px-3 py-1.5 text-sm font-medium transition hover:border-primary/40 hover:bg-primary/5"
                :class="selectedStatus === 'Todos' ? 'border-primary bg-primary/10 text-primary' : 'border-gray-200 bg-white text-gray-600'"
                @click="selectedStatus = 'Todos'"
              >
                Todos
              </button>
              <button
                v-for="status in STATUS_OPTIONS"
                :key="status"
                type="button"
                class="inline-flex items-center rounded-full border px-3 py-1.5 text-sm font-medium transition hover:border-primary/40 hover:bg-primary/5"
                :class="selectedStatus === status ? 'border-primary bg-primary/10 text-primary' : 'border-gray-200 bg-white text-gray-600'"
                @click="selectedStatus = status"
              >
                {{ statusLabel(status) }}
              </button>
            </div>
          </div>

          <div v-if="isLoading" class="rounded-xl border border-dashed bg-white p-10 text-center shadow-sm">
            <p class="text-sm text-gray-600">Carregando tarefas...</p>
          </div>

          <div v-else-if="visibleTasks.length === 0" class="rounded-xl border border-dashed bg-white p-10 text-center shadow-sm">
            <div class="mx-auto mb-4 flex h-12 w-12 items-center justify-center rounded-lg bg-primary/10 text-primary">
              <ListTodo class="h-6 w-6" />
            </div>
            <h3 class="text-lg font-semibold text-gray-900">Nenhuma tarefa encontrada</h3>
            <p class="mx-auto mt-2 max-w-md text-sm text-gray-600">
              Ajuste os filtros ou cadastre uma nova atividade para organizar a rotina da horta.
            </p>
          </div>

          <div v-else class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            <TaskCard
              v-for="task in visibleTasks"
              :key="task.id"
              :id="task.id"
              :descricao="task.descricao ?? ''"
              :status="task.status"
              :status-label="statusLabel(task.status)"
              :data="task.dataLimite"
              :horta="task.horta?.nome ?? `Horta #${task.hortaId}`"
              :responsavel="task.membro?.profile?.name ?? task.membro?.profile?.email ?? 'Sem responsavel'"
              :tipo="task.tipo"
              :icon="task.icon"
              @complete="completeTask"
              @delete="deleteTask"
            />
          </div>
        </section>

        <aside class="h-fit rounded-xl border bg-white p-5 shadow-sm xl:sticky xl:top-6">
          <h2 class="text-base font-semibold text-gray-900">Resumo geral</h2>
          <p class="mt-1 text-sm text-gray-600">Indicadores rapidos para priorizar a rotina.</p>

          <div class="mt-5 space-y-3">
            <div class="rounded-lg bg-gray-50 p-3">
              <p class="text-xs font-medium text-gray-500">Status em foco</p>
              <p class="mt-1 text-sm font-semibold text-gray-900">
                {{ selectedStatus === 'Todos' ? 'Todos' : statusLabel(selectedStatus) }}
              </p>
              <p class="text-xs text-gray-600">{{ visibleTasks.length }} tarefas exibidas</p>
            </div>

            <div class="rounded-lg border border-amber-200 bg-amber-50 p-3">
              <p class="flex items-center gap-2 text-xs font-semibold text-amber-800">
                <TriangleAlert class="h-3.5 w-3.5" />
                Atencao
              </p>
              <p class="mt-1 text-xs text-amber-700">
                {{ totalVencidas }} tarefas vencidas devem ser revisadas antes das demais atividades.
              </p>
            </div>
          </div>
        </aside>
      </div>
    </div>
  </main>
</template>