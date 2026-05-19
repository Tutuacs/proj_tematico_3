<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { toast } from 'vue-sonner'
import { CalendarDays, CheckCircle2, ClipboardList, ListTodo, Save, X } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'

const router = useRouter()
const isSubmitting = ref(false)

const hortas = ['Horta Comunitaria Central', 'Horta da Escola', 'Canteiro de Ervas']
const responsaveis = ['teste 1']
const prioridades = ['Baixa', 'Media', 'Alta']
const statusOptions = ['Pendente', 'Em andamento', 'Concluida']

const form = reactive({
  titulo: '',
  descricao: '',
  horta: '',
  responsavel: '',
  prioridade: 'Media',
  status: 'Pendente',
  dataPrevista: '',
})

function resetForm() {
  form.titulo = ''
  form.descricao = ''
  form.horta = ''
  form.responsavel = ''
  form.prioridade = 'Media'
  form.status = 'Pendente'
  form.dataPrevista = ''
}

function onSubmit() {
  isSubmitting.value = true

  window.setTimeout(() => {
    toast.success('Tarefa cadastrada com sucesso.')
    resetForm()
    isSubmitting.value = false
    router.push('/tarefas')
  }, 500)
}
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
        <div class="mb-5 flex items-center gap-3 border-b pb-4">
          <div class="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10 text-primary">
            <ClipboardList class="h-5 w-5" />
          </div>

          <div>
            <h2 class="text-lg font-semibold text-gray-900">Informacoes da tarefa</h2>
            <p class="text-sm text-gray-600">Dados usados apenas localmente nesta etapa.</p>
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
            <Label for="horta">Horta relacionada</Label>
            <select
              id="horta"
              v-model="form.horta"
              required
              class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
            >
              <option value="" disabled>Selecione uma horta</option>
              <option v-for="horta in hortas" :key="horta" :value="horta">{{ horta }}</option>
            </select>
          </div>

          <div class="space-y-2">
            <Label for="responsavel">Responsavel</Label>
            <select
              id="responsavel"
              v-model="form.responsavel"
              required
              class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
            >
              <option value="" disabled>Selecione um responsavel</option>
              <option v-for="responsavel in responsaveis" :key="responsavel" :value="responsavel">
                {{ responsavel }}
              </option>
            </select>
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

          <div class="space-y-2">
            <Label for="status">Status</Label>
            <select
              id="status"
              v-model="form.status"
              required
              class="h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-sm shadow-xs outline-none transition-[color,box-shadow] focus-visible:border-ring focus-visible:ring-[3px] focus-visible:ring-ring/50"
            >
              <option v-for="status in statusOptions" :key="status" :value="status">
                {{ status }}
              </option>
            </select>
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="dataPrevista">Data prevista</Label>
            <div class="relative">
              <CalendarDays class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" />
              <Input id="dataPrevista" v-model="form.dataPrevista" required type="date" class="pl-9" />
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
