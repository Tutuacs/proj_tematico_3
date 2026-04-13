import { api } from "../api";

export async function getPlantiosByHorta(hortaId: number) {
  const response = await api.get(`/Plantio/horta/${hortaId}`);
  return response.data;
}