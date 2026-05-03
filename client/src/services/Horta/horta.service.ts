import { api } from "../api";

export async function getHortas() {
  const response = await api.get('/Horta');
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
