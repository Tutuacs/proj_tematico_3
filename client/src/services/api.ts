import axios from 'axios'
import type { AxiosError, AxiosResponse } from 'axios'
import type { ApiResponse } from '@/types/api'
import { toast } from 'vue-sonner'

export const api = axios.create({
  baseURL: "http://localhost:5444/api",
})

function getResponseMessage(response: AxiosResponse<ApiResponse<unknown>>) {
  return response.data?.message?.trim()
}

function showToastByStatus(status: number, message: string) {
  if (status >= 200 && status < 300) {
    toast.success(message)
    return
  }

  if (status >= 400 && status < 500) {
    toast.warning(message)
    return
  }

  if (status >= 500) {
    toast.error(message)
  }
}

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

api.interceptors.response.use(
  (response: AxiosResponse<ApiResponse<unknown>>) => {
    const message = getResponseMessage(response)

    if (message) {
      showToastByStatus(response.status, message)
    }

    return response
  },
  (error: AxiosError<ApiResponse<unknown>>) => {
    const status = error.response?.status
    const message = error.response?.data?.message?.trim() ?? 'Ocorreu um erro inesperado'

    if (status) {
      showToastByStatus(status, message)
    } else {
      toast.error(message)
    }

    return Promise.reject(error)
  },
)

export function getApiErrorMessage(error: unknown, fallback = 'Ocorreu um erro inesperado') {
  const axiosError = error as AxiosError<ApiResponse<unknown>>
  return axiosError.response?.data?.message ?? fallback
}