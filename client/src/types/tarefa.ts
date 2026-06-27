import type { Component } from 'vue'

export type TaskTipo = 'Regar' | 'Plantar' | 'Colher' | 'Limpar' | 'Adubar' | 'Inspecionar' | 'Podar'
export type TaskStatus = 'Aberto' | 'EmAndamento' | 'Concluido' | 'Cancelado'

export type HortaResumo = {
  id: number
  nome: string
}

export type PlantioResumo = {
  id: number
  especie?: {
    nome?: string
  } | null
}

export type MembroResumo = {
  id: number
  profile?: {
    name?: string
    email?: string
  } | null
}

export type Task = {
  id: number
  tipo: TaskTipo
  descricao?: string | null
  dataLimite: string
  plantioId?: number | null
  hortaId: number
  membroId?: number | null
  completedAt?: string | null
  status: TaskStatus
  createdAt: string
  horta?: HortaResumo | null
  plantio?: PlantioResumo | null
  membro?: MembroResumo | null
  icon: Component
}

export type CreateTarefaPayload = {
  tipo: TaskTipo
  descricao?: string
  dataLimite: string
  hortaId: number
  plantioId?: number
  membroId?: number
}

export type UpdateTarefaPayload = {
  tipo?: TaskTipo
  descricao?: string
  dataLimite?: string
  plantioId?: number
  membroId?: number
  completedAt?: string
  status?: TaskStatus
}