import { api } from '@/services/api'
import type { CreateTarefaPayload } from '@/types/tarefa'

export async function getTarefas() {
  return api.get('/Tarefa')
}

export async function getTarefaById(id: number) {
  return api.get(`/Tarefa/${id}`)
}

export async function createTarefa(data: CreateTarefaPayload) {
  return api.post('/Tarefa', data)
}

export async function updateTarefa(id: number, data: Partial<CreateTarefaPayload>) {
  return api.put(`/Tarefa/${id}`, data)
}

export async function deleteTarefa(id: number) {
  return api.delete(`/Tarefa/${id}`)
}
