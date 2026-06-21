import type { Task, TaskPriority, TaskStatus } from '@/types/tarefa'

const STORAGE_KEY = 'tarefas'

export type StoredTask = Omit<Task, 'icon'>

export function getStoredTarefas(): StoredTask[] {
  const rawTasks = localStorage.getItem(STORAGE_KEY)

  if (!rawTasks) {
    return []
  }

  try {
    return JSON.parse(rawTasks) as StoredTask[]
  } catch {
    return []
  }
}

export function saveStoredTarefa(task: Omit<StoredTask, 'id' | 'status' | 'observacoes'> & {
  status?: TaskStatus
  observacoes?: string[]
  prioridade: TaskPriority
}) {
  const tasks = getStoredTarefas()
  const nextId = tasks.length ? Math.max(...tasks.map((item) => item.id)) + 1 : 1

  const storedTask: StoredTask = {
    ...task,
    id: nextId,
    status: task.status ?? 'Pendente',
    observacoes: task.observacoes ?? [],
  }

  localStorage.setItem(STORAGE_KEY, JSON.stringify([storedTask, ...tasks]))

  return storedTask
}

export function updateStoredTarefa(id: number, task: Omit<StoredTask, 'id' | 'status' | 'observacoes'> & {
  status?: TaskStatus
  observacoes?: string[]
  prioridade: TaskPriority
}) {
  const tasks = getStoredTarefas()
  const currentTask = tasks.find((item) => item.id === id)

  if (!currentTask) {
    return null
  }

  const updatedTask: StoredTask = {
    ...currentTask,
    ...task,
    id,
    status: task.status ?? currentTask.status,
    observacoes: task.observacoes ?? currentTask.observacoes,
  }

  localStorage.setItem(
    STORAGE_KEY,
    JSON.stringify(tasks.map((item) => (item.id === id ? updatedTask : item))),
  )

  return updatedTask
}

export function deleteStoredTarefa(id: number) {
  const tasks = getStoredTarefas()
  localStorage.setItem(STORAGE_KEY, JSON.stringify(tasks.filter((item) => item.id !== id)))
}

export function completeStoredTarefa(id: number) {
  const tasks = getStoredTarefas()
  const updatedTasks = tasks.map((item) => (
    item.id === id ? { ...item, status: 'Concluida' as TaskStatus } : item
  ))

  localStorage.setItem(STORAGE_KEY, JSON.stringify(updatedTasks))
}
