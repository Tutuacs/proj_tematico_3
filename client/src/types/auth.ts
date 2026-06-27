export interface LoginPayload {
  email: string
  password: string
}

export interface RegisterPayload {
  name: string
  email: string
  password: string
  confirmPassword: string
}

export interface LoginProfile {
  id: string
  name: string
  email: string
  createdAt: string
}

export interface LoginResponse {
  profile: LoginProfile
  token: string
  expiration: string
}