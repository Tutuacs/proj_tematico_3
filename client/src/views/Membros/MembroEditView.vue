<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { getMembrosByPerfil, deleteMembro } from '@/services/Membro/membro.service'
import { useRoute, useRouter } from 'vue-router'
import { getMembroById, updateMembro } from '@/services/Membro/membro.service'
import { getApiErrorMessage } from '@/services/api'

const route = useRoute()
const router = useRouter()
const membroId = Number(route.params.id)

const membro = ref<any>(null)
const isLoading = ref(false)
const isSaving = ref(false)
const error = ref<string | null>(null)

const roleOptions = [
  {
    value: 'Membro',
    label: 'Membro',
    description: 'Pode visualizar e gerenciar plantios.',
    className: 'border-blue-200 bg-blue-50 text-blue-800',
    dotClass: 'bg-blue-500',
  },
  {
    value: 'Admin',
    label: 'Admin',
    description: 'Acesso total à horta e configurações.',
    className: 'border-purple-200 bg-purple-50 text-purple-800',
    dotClass: 'bg-purple-500',
  },
]

const authStore = useAuthStore()
const isCurrentUserAdmin = ref(false)

async function load() {
  isLoading.value = true
  try {
    const res = await getMembroById(membroId)
    membro.value = res.data?.data || res.data
    // determine if current user is admin of this horta
    const currentProfileId = authStore.user?.id
    if (currentProfileId) {
      const res2 = await getMembrosByPerfil(currentProfileId)
      const memberships = res2.data?.data || res2.data || []
      isCurrentUserAdmin.value = memberships.some((m: any) => m.hortaId === membro.value.hortaId && m.role === 'Admin')
    }
  } catch (err) {
    error.value = getApiErrorMessage(err)
  } finally {
    isLoading.value = false
  }
}

async function save() {
  try {
    isSaving.value = true
    await updateMembro(membroId, { role: membro.value.role })
    router.back()
  } catch (err) {
    error.value = getApiErrorMessage(err)
  } finally {
    isSaving.value = false
  }
}

async function removeMember() {
  if (!confirm('Tem certeza que deseja remover este membro da horta?')) return
  try {
    isSaving.value = true
    await deleteMembro(membroId)
    router.back()
  } catch (err) {
    error.value = getApiErrorMessage(err)
  } finally {
    isSaving.value = false
  }
}

onMounted(load)
</script>

<template>
  <main class="flex-1 overflow-auto">
    <div class="mx-auto w-full max-w-screen-lg px-4 py-6 sm:px-6 lg:px-8">
      <div class="mb-6">
        <button @click="$router.back()" class="text-blue-600 hover:text-blue-800 text-sm font-medium mb-4">
          ← Voltar
        </button>
        <h1 class="text-3xl font-bold">Editar Membro</h1>
      </div>

      <div v-if="isLoading" class="flex items-center justify-center py-12">
        <div class="text-center">
          <div class="inline-block h-12 w-12 animate-spin rounded-full border-b-2 border-green-600"></div>
          <p class="mt-4 text-muted-foreground">Carregando membro...</p>
        </div>
      </div>

      <div v-else-if="error" class="rounded-lg border border-red-200 bg-red-50 p-4">
        <p class="text-red-800">{{ error }}</p>
      </div>

      <div v-else-if="membro" class="space-y-6">
        <!-- Perfil Card -->
        <div class="bg-white border rounded-lg p-6">
          <h2 class="text-lg font-semibold mb-6">Informações do Membro</h2>
          
          <div class="space-y-4">
            <div>
              <label class="block text-xs font-semibold text-gray-500 uppercase mb-2">Nome do Usuário</label>
              <div class="px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 font-medium text-gray-900">
                {{ membro.profile?.name || membro.perfilId }}
              </div>
            </div>

            <div>
              <label class="block text-xs font-semibold text-gray-500 uppercase mb-2">Email</label>
              <div class="px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 text-gray-700">
                {{ membro.profile?.email || 'Sem email' }}
              </div>
            </div>

            <div>
              <label class="block text-xs font-semibold text-gray-500 uppercase mb-2">Horta</label>
              <div class="px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 text-gray-700">
                Horta #{{ membro.hortaId }}
              </div>
            </div>
          </div>
        </div>

        <!-- Role Card -->
        <div class="bg-white border rounded-lg p-6">
          <h2 class="text-lg font-semibold mb-4">Permissões</h2>
          
          <div class="space-y-4">
            <div>
              <label class="block text-xs font-semibold text-gray-500 uppercase mb-2">Cargo Atual</label>
              <div class="mb-4">
                <span :class="[
                  'px-4 py-2 rounded-lg text-sm font-semibold inline-block',
                  membro.role === 'Admin' 
                    ? 'bg-purple-100 text-purple-700' 
                    : 'bg-blue-100 text-blue-700'
                ]">
                  {{ membro.role }}
                </span>
              </div>
            </div>

            <div>
              <label class="block text-xs font-semibold text-gray-500 uppercase mb-2">Alterar Cargo</label>
              <div class="grid gap-3">
                <button
                  v-for="option in roleOptions"
                  :key="option.value"
                  type="button"
                  @click="membro.role = option.value"
                  class="rounded-xl border p-3 text-left transition-all w-full text-left"
                  :class="membro.role === option.value ? option.className : 'border-gray-200 bg-white text-gray-700 hover:border-gray-300 hover:bg-gray-50'"
                >
                  <div class="flex items-center gap-3">
                    <span class="h-3 w-3 rounded-full" :class="option.dotClass"></span>
                    <div class="min-w-0 flex-1">
                      <div class="flex items-center justify-between gap-3">
                        <p class="font-semibold text-sm">{{ option.label }}</p>
                        <span v-if="membro.role === option.value" class="text-xs font-medium uppercase">Selecionado</span>
                      </div>
                      <p class="mt-1 text-xs leading-5 opacity-80">{{ option.description }}</p>
                    </div>
                  </div>
                </button>
              </div>
            </div>

            <div class="mt-4 p-4 bg-blue-50 border border-blue-200 rounded-lg">
              <p class="text-sm text-blue-900">
                <strong>Admin:</strong> Tem permissão para gerenciar membros, plantios e todas as configurações da horta.
              </p>
            </div>
          </div>
        </div>

        <!-- Actions -->
        <div class="flex gap-3 sticky bottom-0 bg-white p-4 rounded-lg border">
          <button
            @click="save"
            :disabled="isSaving"
            class="flex-1 px-4 py-2 bg-green-600 hover:bg-green-700 disabled:bg-gray-400 text-white font-medium rounded-lg transition-colors"
          >
            {{ isSaving ? 'Salvando...' : 'Salvar Alterações' }}
          </button>
          <button
            @click="$router.back()"
            :disabled="isSaving"
            class="px-4 py-2 border border-gray-300 text-gray-700 font-medium rounded-lg hover:bg-gray-50 disabled:bg-gray-50 transition-colors"
          >
            Cancelar
          </button>
          <button
            v-if="isCurrentUserAdmin"
            @click="removeMember"
            :disabled="isSaving"
            class="px-4 py-2 bg-red-600 hover:bg-red-700 text-white font-medium rounded-lg transition-colors"
          >
            Remover Membro
          </button>
        </div>
      </div>
    </div>
  </main>
</template>
