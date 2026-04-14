import { api } from "../api";

export async function getAllEspecies() {
  const response = await api.get(`/Especie`);
  return response.data;
}

type CreateEspeciePayload = {
  nome: string;
  diasParaRegar: number;
  imageLink?: string;
  diasParaColher?: number;
  mesesPlantio?: string;
  mesesColheita?: string;
  descricao?: string;
};

export async function createEspecie(data: CreateEspeciePayload) {
  const response = await api.post('/Especie', data);
  return response.data;
}
