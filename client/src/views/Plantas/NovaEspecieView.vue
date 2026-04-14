<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { createEspecie } from '@/services/Especie/especie.service'

const router = useRouter()
const isSubmitting = ref(false)
const errorMessage = ref('')

const form = reactive({
  nome: '',
  diasParaRegar: 2,
  imageLink: '',
  diasParaColher: 60,
  mesesPlantio: '',
  mesesColheita: '',
  descricao: '',
})

async function onSubmit() {
  errorMessage.value = ''
  isSubmitting.value = true

  try {
    await createEspecie({
      nome: form.nome,
      diasParaRegar: Number(form.diasParaRegar),
      imageLink: form.imageLink || undefined,
      diasParaColher: Number(form.diasParaColher) || undefined,
      mesesPlantio: form.mesesPlantio || undefined,
      mesesColheita: form.mesesColheita || undefined,
      descricao: form.descricao || undefined,
    })

    router.push('/plantas')
  } catch {
    errorMessage.value = 'Não foi possível criar a espécie.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-3xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <div>
        <h1 class="text-3xl font-bold">Nova espécie</h1>
        <p class="mt-2 text-muted-foreground">Cadastre uma nova espécie para aparecer na listagem de plantas.</p>
      </div>

      <form class="space-y-4 rounded-xl border bg-white p-5" @submit.prevent="onSubmit">
        <div class="grid gap-4 md:grid-cols-2">
          <div class="space-y-2 md:col-span-2">
            <Label for="nome">Nome</Label>
            <Input id="nome" v-model="form.nome" required placeholder="Ex.: Couve" />
          </div>

          <div class="space-y-2">
            <Label for="diasParaRegar">Dias para regar</Label>
            <Input id="diasParaRegar" v-model.number="form.diasParaRegar" type="number" min="1" required />
          </div>

          <div class="space-y-2">
            <Label for="diasParaColher">Dias para colher</Label>
            <Input id="diasParaColher" v-model.number="form.diasParaColher" type="number" min="1" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="imageLink">Link da imagem</Label>
            <Input id="imageLink" v-model="form.imageLink" placeholder="https://static.vecteezy.com/" />
          </div>

          <div class="space-y-2">
            <Label for="mesesPlantio">Meses de plantio</Label>
            <Input id="mesesPlantio" v-model="form.mesesPlantio" placeholder="1,2,3" />
          </div>

          <div class="space-y-2">
            <Label for="mesesColheita">Meses de colheita</Label>
            <Input id="mesesColheita" v-model="form.mesesColheita" placeholder="4,5,6" />
          </div>

          <div class="space-y-2 md:col-span-2">
            <Label for="descricao">Descrição</Label>
            <Input id="descricao" v-model="form.descricao" placeholder="Descrição breve da espécie" />
          </div>
        </div>

        <p v-if="errorMessage" class="text-sm text-red-600">{{ errorMessage }}</p>

        <div class="flex items-center justify-end gap-2 pt-2">
          <Button type="button" variant="outline" @click="router.push('/plantas')">Cancelar</Button>
          <Button type="submit" :disabled="isSubmitting">{{ isSubmitting ? 'Salvando...' : 'Salvar espécie' }}</Button>
        </div>
      </form>
    </div>
  </main>
</template>