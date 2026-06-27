import { ClipboardList, Droplets, Leaf, Scissors, Search, Sprout } from 'lucide-vue-next'
import type { Component } from 'vue'
import type { TaskStatus, TaskTipo } from '@/types/tarefa'

export const STATUS_OPTIONS: TaskStatus[] = ['Aberto', 'EmAndamento', 'Concluido', 'Cancelado']
export const TIPO_OPTIONS: TaskTipo[] = ['Regar', 'Plantar', 'Colher', 'Limpar', 'Adubar', 'Inspecionar', 'Podar']

export const STATUS_LABELS: Record<TaskStatus, string> = {
  Aberto: 'Aberto',
  EmAndamento: 'Em andamento',
  Concluido: 'Concluido',
  Cancelado: 'Cancelado',
}

export const STATUS_CLASSES: Record<TaskStatus, string> = {
  Aberto: 'bg-amber-50 text-amber-700 border-amber-200',
  EmAndamento: 'bg-blue-50 text-blue-700 border-blue-200',
  Concluido: 'bg-green-50 text-green-700 border-green-200',
  Cancelado: 'bg-gray-50 text-gray-600 border-gray-200',
}

export const TIPO_ICONS: Record<TaskTipo, Component> = {
  Regar: Droplets,
  Plantar: Sprout,
  Colher: Leaf,
  Limpar: ClipboardList,
  Adubar: ClipboardList,
  Inspecionar: Search,
  Podar: Scissors,
}

export function statusLabel(status: TaskStatus) {
  return STATUS_LABELS[status]
}

export function statusClasses(status: TaskStatus) {
  return STATUS_CLASSES[status]
}

export function tipoIcon(tipo: TaskTipo) {
  return TIPO_ICONS[tipo] ?? ClipboardList
}

// Aceita 'YYYY-MM-DD' (DateOnly da API) ou um Date/ISO completo e devolve 'DD/MM/AAAA'
export function formatDateBr(value?: string | null) {
  if (!value) return '-'

  const datePart = value.slice(0, 10)
  const [year, month, day] = datePart.split('-')

  if (!year || !month || !day) return value

  return `${day}/${month}/${year}`
}