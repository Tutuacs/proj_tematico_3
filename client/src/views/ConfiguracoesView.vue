<script setup lang="ts">
import { reactive } from 'vue'
import { toast } from 'vue-sonner'
import { LockKeyhole, Save, Settings, User2 } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

const form = reactive({
  nome: authStore.user?.name || '',
  senhaAtual: '',
  novaSenha: '',
  confirmarSenha: '',
})

function salvarConfiguracoes() {
  if (form.novaSenha && form.novaSenha !== form.confirmarSenha) {
    toast.error('A nova senha e a confirmacao precisam ser iguais.')
    return
  }

  toast.success('Configuracoes atualizadas com sucesso.')
  form.senhaAtual = ''
  form.novaSenha = ''
  form.confirmarSenha = ''
}
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-4xl space-y-6 px-4 py-6 sm:px-6 lg:px-8">
      <section class="rounded-xl border bg-white p-6 shadow-sm">
        <div class="max-w-3xl">
          <span class="inline-flex items-center gap-2 rounded-full bg-primary/10 px-3 py-1 text-sm font-medium text-primary">
            <Settings class="h-4 w-4" />
            Configuracoes
          </span>
          <h1 class="mt-4 text-3xl font-bold text-gray-900">Minha conta</h1>
          <p class="mt-2 text-muted-foreground">
            Atualize seu nome e altere sua senha de acesso.
          </p>
        </div>
      </section>

      <form class="rounded-xl border bg-white p-5 shadow-sm sm:p-6" @submit.prevent="salvarConfiguracoes">
        <div class="grid gap-6">
          <section class="space-y-4">
            <div class="flex items-center gap-3 border-b pb-4">
              <div class="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10 text-primary">
                <User2 class="h-5 w-5" />
              </div>

              <div>
                <h2 class="text-lg font-semibold text-gray-900">Nome</h2>
                <p class="text-sm text-gray-600">Nome exibido no sistema.</p>
              </div>
            </div>

            <div class="space-y-2">
              <Label for="nome">Nome</Label>
              <Input id="nome" v-model="form.nome" required placeholder="Digite seu nome" />
            </div>
          </section>

          <section class="space-y-4">
            <div class="flex items-center gap-3 border-b pb-4">
              <div class="flex h-11 w-11 items-center justify-center rounded-lg bg-primary/10 text-primary">
                <LockKeyhole class="h-5 w-5" />
              </div>

              <div>
                <h2 class="text-lg font-semibold text-gray-900">Alteracao de senha</h2>
                <p class="text-sm text-gray-600">Preencha os campos somente quando quiser trocar a senha.</p>
              </div>
            </div>

            <div class="grid gap-4 md:grid-cols-2">
              <div class="space-y-2 md:col-span-2">
                <Label for="senhaAtual">Senha atual</Label>
                <Input id="senhaAtual" v-model="form.senhaAtual" type="password" placeholder="Digite a senha atual" />
              </div>

              <div class="space-y-2">
                <Label for="novaSenha">Nova senha</Label>
                <Input id="novaSenha" v-model="form.novaSenha" type="password" placeholder="Digite a nova senha" />
              </div>

              <div class="space-y-2">
                <Label for="confirmarSenha">Confirmar nova senha</Label>
                <Input id="confirmarSenha" v-model="form.confirmarSenha" type="password" placeholder="Confirme a nova senha" />
              </div>
            </div>
          </section>
        </div>

        <div class="mt-6 flex justify-end border-t pt-5">
          <Button type="submit" class="w-full sm:w-fit">
            <Save class="h-4 w-4" />
            Salvar alteracoes
          </Button>
        </div>
      </form>
    </div>
  </main>
</template>
