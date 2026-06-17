<script setup lang="ts">
import { onMounted, reactive, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { toast } from 'vue-sonner'
import { CalendarDays, CheckCircle2, ClipboardList, Leaf, ListTodo, Save, Sprout, User2, X } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { getHortas, getPlantiosByHorta } from '@/services/Horta/horta.service'
import { getMembrosByHorta } from '@/services/Membro/membro.service'
import type { CreateTarefaPayload, TaskPriority } from '@/types/tarefa'

const router = useRouter()
const isSubmitting = ref(false)
const isLoadingHortas = ref(true)
const isLoadingVinculos = ref(false)
const loadError = ref<string | null>(null)

type HortaOption = {
  id: number
  nome: string
}

type PlantioOption = {
  id: number
  especie?: {
    nome?: string
  } | null
}

type MembroOption = {
  id: number
  profile?: {
    name?: string
    email?: string
  } | null
}

const hortas = ref<HortaOption[]>([])
const plantios = ref<PlantioOption[]>([])
const membros = ref<MembroOption[]>([])
const tipos = ['Regar', 'Plantar', 'Colher', 'Limpar', 'Adubar', 'Inspecionar', 'Podar']
const prioridades: TaskPriority[] = ['Baixa', 'Media', 'Alta']

const form = reactive({
  titulo: '',
  descricao: '',
  tipo: '',
  hortaId: '',
  plantioId: '',
  membroId: '',
  prioridade: 'Media' as TaskPriority,
  dataLimite: '',
})

function resetForm() {
  form.titulo = ''
  form.descricao = ''
  form.tipo = ''
  form.hortaId = ''
  form.plantioId = ''
  form.membroId = ''
  form.prioridade = 'Media'
  form.dataLimite = ''
}

async function loadHortas() {
  try {
    isLoadingHortas.value = true
    loadError.value = null
    const response = await getHortas()
    hortas.value = response?.data ?? []
  } catch {
    loadError.value = 'Nao foi possivel carregar as hortas para vincular a tarefa.'
  } finally {
    isLoadingHortas.value = false
  }
}

async function loadVinculosByHorta(hortaId: number) {
  try {
    isLoadingVinculos.value = true
    loadError.value = null

    const [plantiosResponse, membrosResponse] = await Promise.all([
      getPlantiosByHorta(hortaId),
      getMembrosByHorta(hortaId),
    ])

    plantios.value = plantiosResponse?.data ?? []
    membros.value = membrosResponse.data?.data ?? []
  } catch {
    plantios.value = []
    membros.value = []
    loadError.value = 'Nao foi possivel carregar plantios e membros desta horta.'
  } finally {
    isLoadingVinculos.value = false
  }
}

watch(
  () => form.hortaId,
  (hortaId) => {
    form.plantioId = ''
    form.membroId = ''
    plantios.value = []
    membros.value = []

    if (hortaId) {
      void loadVinculosByHorta(Number(hortaId))
    }
  },
)

function onSubmit() {
  isSubmitting.value = true

  const payload: CreateTarefaPayload = {
    titulo: form.titulo,
    descricao: form.descricao,
    tipo: form.tipo,
    hortaId: Number(form.hortaId),
    plantioId: form.plantioId ? Number(form.plantioId) : undefined,
    membroId: form.membroId ? Number(form.membroId) : undefined,
    prioridade: form.prioridade,
    dataLimite: form.dataLimite,
  }

  window.setTimeout(() => {
    console.info('Payload preparado para POST /Tarefa:', payload)
    toast.success('Tarefa preparada com vinculos reais. A API de tarefas ainda precisa ser ligada.')
    resetForm()
    isSubmitting.value = false
    router.push('/tarefas')
  }, 500)
}

onMounted(loadHortas)
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-4xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <section class="rounded-xl border bg-white p-6 shadow-sm">
        <div class="max-w-3xl">
          <span class="inline-flex items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
            <ListTodo class="h-4 w-4" />
            Nova tarefa
          </span>
          <h1 class="mt-4 text-3xl font-bold text-gray-900">Cadastrar tarefa</h1>
          <p class="mt-2 text-muted-foreground">
            Preencha as informacoes para organizar as atividades da horta.
          </p>
        </div>
      </section>

      <form class="rounded-xl border bg-white p-5 shadow-sm sm:p-6" @submit.prevent="onSubmit">
        <div v-if="loadError" class="mb-5 rounded-lg border border-amber-200 bg-amber-50 p-3 text-sm text-amber-800">
          {{ loadError }}
        </div>

        <div class="mb-5 flex items-center gap-3 border-b pb-4">
          <div class="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10 text-primary">
            <ClipboardList class="h-5 w-5" />
          </div>

          <div>
            <h2 class="text-lg font-semibold text-gray-900">Informacoes da tarefa</h2>
            <p class="text-sm text-gray-600">Defina a atividade, prazo e pessoa responsavel.</p>
          </div>
        </div>

        <div class="grid gap-4 md:grid-cols-2">
          <div class="space-y-2 md:col-span-2">
            <Label for="titulo">Titulo da tarefa</Label>
            <Input id="titulo" v-model="form.titulo" required placeholder="Ex.: Regar canteiro de alfaces" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="descricao">Descricao</Label>
            <textarea
              id="descricao"
              v-model="form.descricao"
              required
              rows="4"
              placeholder="Descreva brevemente a atividade"
              class="flex min-h-24 w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-xs outline-none transition-[color,box-shadow] placeholder:text-muted-foreground focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50 disabled:cursor-not-allowed disabled:opacity-50"
            ></textarea>
          </div>

          <div class="space-y-2">
            <Label for="tipo">Tipo da tarefa</Label>
            <div class="relative">
              <ListTodo class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
              <select
                id="tipo"
                v-model="form.tipo"
                required
                class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 pl-9 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
              >
                <option value="" disabled>Selecione o tipo</option>
                <option v-for="tipo in tipos" :key="tipo" :value="tipo">{{ tipo }}</option>
              </select>
            </div>
          </div>

          <div class="space-y-2">
            <Label for="horta">Horta relacionada</Label>
            <div class="relative">
              <Sprout class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
              <select
                id="horta"
                v-model="form.hortaId"
                required
                :disabled="isLoadingHortas"
                class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 pl-9 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
              >
                <option value="" disabled>
                  {{ isLoadingHortas ? 'Carregando hortas...' : 'Selecione uma horta' }}
                </option>
                <option v-for="horta in hortas" :key="horta.id" :value="horta.id">{{ horta.nome }}</option>
              </select>
            </div>
          </div>

          <div class="space-y-2">
            <Label for="plantio">Plantio relacionado</Label>
            <div class="relative">
              <Leaf class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
              <select
                id="plantio"
                v-model="form.plantioId"
                :disabled="!form.hortaId || isLoadingVinculos"
                class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 pl-9 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
              >
                <option value="">
                  {{ isLoadingVinculos ? 'Carregando plantios...' : 'Sem plantio relacionado' }}
                </option>
                <option v-for="plantio in plantios" :key="plantio.id" :value="plantio.id">
                  {{ plantio.especie?.nome ?? `Plantio #${plantio.id}` }}
                </option>
              </select>
            </div>
          </div>

          <div class="space-y-2">
            <Label for="responsavel">Responsavel</Label>
            <div class="relative">
              <User2 class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
              <select
                id="responsavel"
                v-model="form.membroId"
                required
                :disabled="!form.hortaId || isLoadingVinculos"
                class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 pl-9 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
              >
                <option value="" disabled>
                  {{ isLoadingVinculos ? 'Carregando membros...' : 'Selecione um responsavel' }}
                </option>
                <option v-for="membro in membros" :key="membro.id" :value="membro.id">
                  {{ membro.profile?.name ?? membro.profile?.email ?? `Membro #${membro.id}` }}
                </option>
              </select>
            </div>
          </div>

          <div class="space-y-2">
            <Label for="prioridade">Prioridade</Label>
            <select
              id="prioridade"
              v-model="form.prioridade"
              required
              class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
            >
              <option v-for="prioridade in prioridades" :key="prioridade" :value="prioridade">
                {{ prioridade }}
              </option>
            </select>
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="dataLimite">Data limite</Label>
            <div class="relative">
              <CalendarDays class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
              <Input id="dataLimite" v-model="form.dataLimite" required type="date" class="pl-9" />
            </div>
          </div>
        </div>

        <div class="mt-6 flex flex-col-reverse gap-2 border-t pt-5 sm:flex-row sm:justify-end">
          <Button type="button" variant="outline" class="sm:w-fit" @click="router.push('/tarefas')">
            <X class="h-4 w-4" />
            Cancelar
          </Button>

          <Button type="submit" class="sm:w-fit" :disabled="isSubmitting">
            <CheckCircle2 v-if="isSubmitting" class="h-4 w-4 animate-pulse" />
            <Save v-else class="h-4 w-4" />
            {{ isSubmitting ? 'Salvando...' : 'Salvar tarefa' }}
          </Button>
        </div>
      </form>
    </div>
  </main>
</template>
