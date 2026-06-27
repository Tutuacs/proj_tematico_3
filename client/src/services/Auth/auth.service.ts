import type { ApiResponse } from '@/types/api'
import type { LoginPayload, LoginResponse, RegisterPayload } from '@/types/auth'
import { api } from '../api'

export async function login(credentials: LoginPayload) {
    const response = await api.post<ApiResponse<LoginResponse>>('/Auth/login', credentials)
    return response.data
}

export async function register(data: RegisterPayload) {
    const response = await api.post<ApiResponse<LoginResponse>>('/Auth/register', {email: data.email, password: data.password, name: data.name})
    console.log('Register response:', response.data)
    return response.data
}