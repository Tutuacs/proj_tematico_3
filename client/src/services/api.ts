import axios from 'axios'
import type { AxiosError } from 'axios'
import type { ApiResponse } from '@/types/api'

export const api = axios.create({
  baseURL: "http://localhost:5444/api",
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

export function getApiErrorMessage(error: unknown, fallback = 'Ocorreu um erro inesperado') {
  const axiosError = error as AxiosError<ApiResponse<unknown>>
  return axiosError.response?.data?.message ?? fallback
}