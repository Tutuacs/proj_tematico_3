import { api } from '@/services/api'

export async function getPlantioById(id: number) {
  return api.get(`/Plantio/${id}`)
}

export async function updatePlantio(id: number, data: unknown) {
  return api.put(`/Plantio/${id}`, data)
}

export async function deletePlantio(id: number) {
  return api.delete(`/Plantio/${id}`)
}
