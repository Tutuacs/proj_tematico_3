import { ref, computed } from "vue";
import { defineStore } from "pinia";
import type { Profile } from "@/types/Profile";
import {
  login as loginRequest,
  register as registerRequest,
} from "@/services/Auth/auth.service";
import { getApiErrorMessage } from "@/services/api";
import type { LoginPayload, RegisterPayload } from "@/types/auth";

type LoginResult = {
  success: boolean;
  message: string;
};

export const useAuthStore = defineStore("auth", () => {
  const token = ref<string | null>(localStorage.getItem("token"));
  const user = ref<Profile | null>(null);
  const expiresAt = ref<Date | null>(null);

  hydrateSession();

  const isLoggedIn = computed(() => {
    if (!token.value) return false;
    if (!expiresAt.value) return true;

    return expiresAt.value.getTime() > Date.now();
  });

  async function login(data: LoginPayload): Promise<LoginResult> {
    const { email, password } = data;
    if (!email || !password) {
      return {
        success: false,
        message: "Preencha email e senha para continuar.",
      };
    }

    try {
      const response = await loginRequest(data);

      if (!response.data) {
        return {
          success: false,
          message: response.message ?? "Falha ao autenticar usuário.",
        };
      }

      token.value = response.data.token;
      user.value = response.data.profile;
      expiresAt.value = new Date(response.data.expiration);

      localStorage.setItem("token", response.data.token);
      localStorage.setItem("user", JSON.stringify(response.data.profile));
      localStorage.setItem("expiresAt", response.data.expiration);

      return {
        success: true,
        message: response.message ?? "Login realizado com sucesso.",
      };
    } catch (error) {
      return {
        success: false,
        message: getApiErrorMessage(
          error,
          "Não foi possível realizar o login.",
        ),
      };
    }
  }

  async function register(data: RegisterPayload): Promise<LoginResult> {
    const { email, password } = data;
    if (!email || !password) {
      return {
        success: false,
        message: "Preencha email e senha para continuar.",
      };
    }

    try {
      const response = await registerRequest(data);

      if (!response.data) {
        return {
          success: false,
          message: response.message ?? "Falha ao registrar usuário.",
        };
      }

      token.value = response.data.token;
      user.value = response.data.profile;
      expiresAt.value = new Date(response.data.expiration);

      localStorage.setItem("token", response.data.token);
      localStorage.setItem("user", JSON.stringify(response.data.profile));
      localStorage.setItem("expiresAt", response.data.expiration);

      return {
        success: true,
        message: response.message ?? "Registro realizado com sucesso.",
      };
    } catch (error) {
      return {
        success: false,
        message: getApiErrorMessage(
          error,
          "Não foi possível realizar o registro.",
        ),
      };
    }
  }

  function logout() {
    token.value = null;
    user.value = null;
    expiresAt.value = null;

    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.removeItem("expiresAt");
  }

  function hydrateSession() {
    const storedUser = localStorage.getItem("user");
    const storedExpiration = localStorage.getItem("expiresAt");

    if (storedUser) {
      user.value = JSON.parse(storedUser) as Profile;
    }

    if (storedExpiration) {
      expiresAt.value = new Date(storedExpiration);
    }

    if (expiresAt.value && expiresAt.value.getTime() <= Date.now()) {
      logout();
    }
  }

  return { isLoggedIn, user, expiresAt, login, register, logout };
});
