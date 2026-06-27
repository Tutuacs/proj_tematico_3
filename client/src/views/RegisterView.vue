<script setup lang="ts">
import Container from "@/components/Container.vue";
import Button from "@/components/ui/button/Button.vue";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useAuthStore } from "@/stores/auth";
import type { RegisterPayload } from "@/types/auth";
import { computed, ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();
const authStore = useAuthStore();

const registerPayload = ref<RegisterPayload>({
  name: "",
  email: "",
  password: "",
  confirmPassword: "",
});

const errorMessage = ref("");
const isRegistering = ref(false);

const canSubmit = computed(() => {
  const { name, email, password, confirmPassword } = registerPayload.value;

  return (
    name.trim() !== "" &&
    email.trim() !== "" &&
    password !== "" &&
    confirmPassword !== "" &&
    password === confirmPassword
  );
});

async function onSubmit() {
  errorMessage.value = "";

  if (!canSubmit.value) {
    errorMessage.value = "Preencha nome, email, senha e confirmação corretamente.";
    return;
  }

  isRegistering.value = true; 

  const result = await authStore.register(registerPayload.value);
  isRegistering.value = false;

  if (!result.success) {
    errorMessage.value = result.message;
    return;
  }

  router.push("/");
}
</script>

<template>
  <Container>
    <main class="grid min-h-[calc(100vh-65px)] place-items-center p-4">
      <Card class="w-full max-w-md">
        <CardHeader>
          <CardTitle>Cadastro</CardTitle>
          <CardDescription
            >Preencha os campos abaixo para criar sua conta.</CardDescription
          >
        </CardHeader>

        <CardContent>
          <form class="space-y-4" @submit.prevent="onSubmit">
            <div class="space-y-2">
              <Label for="name">Nome</Label>
              <Input
                id="name"
                v-model="registerPayload.name"
                type="text"
                placeholder="Seu nome"
              />
            </div>

            <div class="space-y-2">
              <Label for="email">Email</Label>
              <Input
                id="email"
                v-model="registerPayload.email"
                type="email"
                placeholder="voce@exemplo.com"
              />
            </div>

            <div class="space-y-2">
              <Label for="password">Senha</Label>
              <Input
                id="password"
                v-model="registerPayload.password"
                type="password"
                placeholder="********"
              />
            </div>

            <div class="space-y-2">
              <Label for="confirmPassword">Confirmação de senha</Label>
              <Input
                id="confirmPassword"
                v-model="registerPayload.confirmPassword"
                type="password"
                placeholder="********"
              />
            </div>

            <p v-if="errorMessage" class="text-sm text-destructive">
              {{ errorMessage }}
            </p>

            <Button
              type="submit"
              class="w-full"
              :disabled="isRegistering || !canSubmit"
            >
              {{ isRegistering ? "Registrando..." : "Registrar-se" }}
            </Button>
          </form>
        </CardContent>
      </Card>
    </main>
  </Container>
</template>

<style></style>

