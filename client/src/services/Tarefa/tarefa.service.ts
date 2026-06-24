import { api } from '@/services/api'
import type { CreateTarefaPayload, UpdateTarefaPayload } from '@/types/tarefa'

export async function getTarefas() {
  return api.get('/Todo')
}

export async function getTarefaById(id: number) {
  return api.get(`/Todo/${id}`)
}

export async function createTarefa(data: CreateTarefaPayload) {
  return api.post('/Todo', data)
}

export async function updateTarefa(id: number, data: UpdateTarefaPayload) {
  return api.put(`/Todo/${id}`, data)
}

export async function deleteTarefa(id: number) {
  return api.delete(`/Todo/${id}`)
}