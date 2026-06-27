import { api } from '@/services/api'

 
export type AddMembroPayload = {
  hortaId: number
  perfilId: string
  role: 'Admin' | 'Membro'
}

export async function getMembrosByHorta(hortaId: number) {
  return api.get(`/Membro/horta/${hortaId}`)
}

export async function getMembroById(id: number) {
  return api.get(`/Membro/${id}`)
}

export async function getMembrosByPerfil(perfilId: string) {
  return api.get(`/Membro/perfil/${perfilId}`)
}

export async function updateMembro(id: number, data: unknown) {
  return api.put(`/Membro/${id}`, data)
}

export async function deleteMembro(id: number) {
  return api.delete(`/Membro/${id}`)
}

export async function getUsuariosDisponiveisParaHorta(hortaId: number) {
  return api.get(`/Membro/add/members/horta/${hortaId}`)
}
 
export async function addMembro(data: AddMembroPayload) {
  return api.post('/Membro', data)
}
