<script setup lang="ts">
import { computed, ref } from 'vue'
import { CalendarCheck2, ClipboardList, Droplets, ListTodo, Scissors, Sprout, TriangleAlert } from 'lucide-vue-next'
import TaskCard from '@/components/TaskCard.vue'

type TaskStatus = 'Pendente' | 'Em andamento' | 'Concluida'
type TaskPriority = 'Baixa' | 'Media' | 'Alta'

type Task = {
  id: number
  nome: string
  descricao: string
  status: TaskStatus
  prioridade: TaskPriority
  data: string
  icon: typeof Droplets
}

const selectedStatus = ref('Todos')

const filters = ['Todos', 'Pendente', 'Em andamento', 'Concluida']

const tasks = ref<Task[]>([
  {
    id: 1,
    nome: 'Regar canteiro de alfaces',
    descricao: 'Verificar umidade do solo e realizar a rega no periodo da manha.',
    status: 'Pendente',
    prioridade: 'Alta',
    data: '20/05/2026',
    icon: Droplets,
  },
  {
    id: 2,
    nome: 'Revisar crescimento dos tomates',
    descricao: 'Acompanhar folhas, galhos e sinais de pragas no plantio principal.',
    status: 'Em andamento',
    prioridade: 'Media',
    data: '21/05/2026',
    icon: Sprout,
  },
  {
    id: 3,
    nome: 'Podar ervas aromaticas',
    descricao: 'Remover folhas secas e organizar mudas de manjericao e hortela.',
    status: 'Concluida',
    prioridade: 'Baixa',
    data: '18/05/2026',
    icon: Scissors,
  },
  {
    id: 4,
    nome: 'Preparar novo canteiro',
    descricao: 'Separar ferramentas, adubo organico e area para o proximo plantio.',
    status: 'Pendente',
    prioridade: 'Alta',
    data: '23/05/2026',
    icon: ClipboardList,
  },
])

const totalTarefas = computed(() => tasks.value.length)
const totalPendentes = computed(() => tasks.value.filter((task) => task.status === 'Pendente').length)
const totalConcluidas = computed(() => tasks.value.filter((task) => task.status === 'Concluida').length)
const totalPrioridadeAlta = computed(() => tasks.value.filter((task) => task.prioridade === 'Alta').length)

const visibleTasks = computed(() => {
  if (selectedStatus.value === 'Todos') {
    return tasks.value
  }

  return tasks.value.filter((task) => task.status === selectedStatus.value)
})
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
              Organize atividades da horta, acompanhe prioridades e mantenha a rotina de cuidados em dia.
            </p>
          </div>

        </div>
      </section>

      <section class="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Total de tarefas</p>
          <p class="mt-2 text-2xl font-bold text-gray-900">{{ totalTarefas }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Pendentes</p>
          <p class="mt-2 text-2xl font-bold text-amber-700">{{ totalPendentes }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Concluidas</p>
          <p class="mt-2 text-2xl font-bold text-green-700">{{ totalConcluidas }}</p>
        </div>

        <div class="rounded-xl border bg-white p-4 shadow-sm">
          <p class="text-xs font-medium text-gray-500">Prioridade alta</p>
          <p class="mt-2 text-2xl font-bold text-red-700">{{ totalPrioridadeAlta }}</p>
        </div>
      </section>

      <div class="grid gap-6 xl:grid-cols-[minmax(0,1fr)_320px]">
        <section class="space-y-4">
          <div class="flex flex-col gap-3 rounded-xl border bg-white p-4 shadow-sm sm:flex-row sm:items-center sm:justify-between">
            <div>
              <h2 class="text-base font-semibold text-gray-900">Lista de tarefas</h2>
              <p class="mt-1 text-sm text-gray-600">Filtro visual por status para organizar a rotina.</p>
            </div>

            <div class="flex flex-wrap gap-2">
              <button
                v-for="filter in filters"
                :key="filter"
                type="button"
                class="inline-flex items-center rounded-full border px-3 py-1.5 text-sm font-medium transition hover:border-primary/40 hover:bg-primary/5"
                :class="selectedStatus === filter ? 'border-primary bg-primary/10 text-primary' : 'border-gray-200 bg-white text-gray-600'"
                @click="selectedStatus = filter"
              >
                {{ filter }}
              </button>
            </div>
          </div>

          <div class="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            <TaskCard
              v-for="task in visibleTasks"
              :key="task.id"
              :nome="task.nome"
              :descricao="task.descricao"
              :status="task.status"
              :prioridade="task.prioridade"
              :data="task.data"
              :icon="task.icon"
            />
          </div>
        </section>

        <aside class="h-fit rounded-xl border bg-white p-5 shadow-sm xl:sticky xl:top-6">
          <h2 class="text-base font-semibold text-gray-900">Resumo geral</h2>
          <p class="mt-1 text-sm text-gray-600">Indicadores rapidos das tarefas cadastradas localmente.</p>

          <div class="mt-5 space-y-3">
            <div class="rounded-lg bg-gray-50 p-3">
              <p class="text-xs font-medium text-gray-500">Status em foco</p>
              <p class="mt-1 text-sm font-semibold text-gray-900">{{ selectedStatus }}</p>
              <p class="text-xs text-gray-600">{{ visibleTasks.length }} tarefas exibidas</p>
            </div>

            <div class="rounded-lg border border-green-200 bg-green-50 p-3">
              <p class="flex items-center gap-2 text-xs font-semibold text-green-800">
                <CalendarCheck2 class="h-3.5 w-3.5" />
                Rotina da horta
              </p>
              <p class="mt-1 text-xs text-green-700">
                Use os cards para acompanhar atividades, datas e prioridades sem sair da tela.
              </p>
            </div>

            <div class="rounded-lg border border-amber-200 bg-amber-50 p-3">
              <p class="flex items-center gap-2 text-xs font-semibold text-amber-800">
                <TriangleAlert class="h-3.5 w-3.5" />
                Atencao
              </p>
              <p class="mt-1 text-xs text-amber-700">
                Tarefas de prioridade alta aparecem destacadas nos cards e no resumo superior.
              </p>
            </div>
          </div>
        </aside>
      </div>
    </div>
  </main>
</template>
