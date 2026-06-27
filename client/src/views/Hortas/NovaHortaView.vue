<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Save, Sprout, X } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { createHorta } from '@/services/Horta/horta.service'
import { getApiErrorMessage } from '@/services/api'

const router = useRouter()
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

const form = reactive({
  nome: '',
  local: '',
  descricao: '',
  largura: 1,
  profundidade: 1,
})

async function onSubmit() {
  errorMessage.value = null
  isSubmitting.value = true

  try {
    await createHorta({
      nome: form.nome,
      local: form.local,
      descricao: form.descricao || undefined,
      largura: Number(form.largura),
      profundidade: Number(form.profundidade),
    })

    router.push('/horta')
  } catch (error) {
    errorMessage.value = getApiErrorMessage(error, 'Nao foi possivel cadastrar a horta.')
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-3xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <section class="rounded-xl border bg-white p-6 shadow-sm">
        <div class="flex items-start gap-4">
          <div class="flex h-11 w-11 shrink-0 items-center justify-center rounded-lg bg-primary/10 text-primary">
            <Sprout class="h-5 w-5" />
          </div>

          <div>
            <h1 class="text-3xl font-bold text-gray-900">Cadastrar horta</h1>
            <p class="mt-2 text-muted-foreground">
              Informe os dados principais da area de cultivo para vincular membros e plantios depois.
            </p>
          </div>
        </div>
      </section>

      <form class="space-y-5 rounded-xl border bg-white p-6 shadow-sm" @submit.prevent="onSubmit">
        <div class="grid gap-4 md:grid-cols-2">
          <div class="space-y-2 md:col-span-2">
            <Label for="nome">Nome</Label>
            <Input id="nome" v-model="form.nome" required placeholder="Ex.: Horta comunitaria central" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="local">Localizacao</Label>
            <Input id="local" v-model="form.local" required placeholder="Ex.: Rua das Flores, 120" />
          </div>

          <div class="space-y-2">
            <Label for="largura">Largura</Label>
            <Input id="largura" v-model.number="form.largura" type="number" min="0.01" step="0.01" required />
          </div>

          <div class="space-y-2">
            <Label for="profundidade">Profundidade</Label>
            <Input id="profundidade" v-model.number="form.profundidade" type="number" min="0.01" step="0.01" required />
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="descricao">Descricao</Label>
            <Input id="descricao" v-model="form.descricao" placeholder="Resumo da horta" />
          </div>
        </div>

        <p v-if="errorMessage" class="rounded-lg border border-red-200 bg-red-50 p-3 text-sm text-red-700">
          {{ errorMessage }}
        </p>

        <div class="flex flex-col-reverse gap-2 border-t pt-5 sm:flex-row sm:justify-end">
          <Button type="button" variant="outline" class="sm:w-fit" @click="router.push('/horta')">
            <X class="mr-2 h-4 w-4" />
            Cancelar
          </Button>
          <Button type="submit" class="sm:w-fit" :disabled="isSubmitting">
            <Save class="mr-2 h-4 w-4" />
            {{ isSubmitting ? 'Salvando...' : 'Salvar horta' }}
          </Button>
        </div>
      </form>
    </div>
  </main>
</template>
