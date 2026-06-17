import type { Component } from 'vue'

export type TaskStatus = 'Pendente' | 'Em andamento' | 'Concluida'
export type TaskPriority = 'Baixa' | 'Media' | 'Alta'

export type Task = {
  id: number
  nome: string
  descricao: string
  status: TaskStatus
  prioridade: TaskPriority
  data: string
  hortaId: number
  horta: string
  responsavelId?: number
  responsavel: string
  tipo: string
  plantioId?: number
  plantio: string
  observacoes: string[]
  icon: Component
}

export type CreateTarefaPayload = {
  titulo: string
  descricao: string
  tipo: string
  hortaId: number
  plantioId?: number
  membroId?: number
  prioridade: TaskPriority
  dataLimite: string
}
