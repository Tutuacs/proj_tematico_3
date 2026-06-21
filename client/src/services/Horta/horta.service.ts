import { api } from "../api";

export type CreateHortaPayload = {
  nome: string
  descricao?: string
  local: string
  largura: number
  profundidade: number
}

export async function getHortas() {
  const response = await api.get('/Horta');
  return response.data;
}

export async function createHorta(data: CreateHortaPayload) {
  const response = await api.post('/Horta', data);
  return response.data;
}

export async function getHortaById(hortaId: number) {
  const response = await api.get(`/Horta/${hortaId}`);
  return response.data;
}

export async function getPlantiosByHorta(hortaId: number) {
  const response = await api.get(`/Plantio/horta/${hortaId}`);
  return response.data;
}
