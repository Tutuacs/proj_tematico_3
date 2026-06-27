<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/stores/auth";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import Container from "@/components/Container.vue";
import type { LoginPayload } from "@/types/auth";

const router = useRouter();
const authStore = useAuthStore();

const loginPayload = ref<LoginPayload>({
  email: "",
  password: "",
});
const errorMessage = ref("");
const isSubmitting = ref(false);

async function onSubmit() {
  errorMessage.value = "";
  isSubmitting.value = true;

  const result = await authStore.login(loginPayload.value);
  isSubmitting.value = false;

  if (!result.success) {
    errorMessage.value = result.message;
    return;
  }

  router.push("/");
}
</script>

<template>
  <Container>
    <main class="grid min-h-screen md:grid-cols-2">

<!-- LADO ESQUERDO -->
<div class="hidden md:flex items-start justify-start relative min-h-screen">
  <img
    src="https://img.freepik.com/vetores-gratis/fundo-realista-de-folhas-tropicais-escuras_52683-30655.jpg?semt=ais_hybrid&w=740&q=80"
    class="absolute inset-0 w-full h-full object-cover"
  />

  <div class="absolute inset-0 bg-black/50"></div>

  <div class="relative z-10 flex flex-col justify-between h-full pl-8 pr-6 py-16 text-white">

    <div>
      <h1 class="text-4xl font-bold mb-2 tracking-tight">
        HortaFácil
      </h1>
      <p class="text-sm text-green-100">
        Cultivando a comunidade com precisão.
      </p>
    </div>

    <div class="space-y-3 text-sm text-green-100">
      <p>Gestão inteligente das hortas</p>
      <p>Crescimento colaborativo</p>
      <p>Ferramentas de controle e organização</p>
    </div>

  </div>

</div>

      <!-- LADO DIREITO -->
      <div class="flex items-center justify-center p-6 bg-gray-50">
        <Card class="w-full max-w-md rounded-2xl shadow-xl border border-gray-200">

          <CardHeader>
            <CardTitle>Seja Bem-vindo</CardTitle>
            <CardDescription>
              Acesse o sistema para gerenciar suas hortas.
            </CardDescription>
          </CardHeader>

          <CardContent>
            <form class="space-y-4" @submit.prevent="onSubmit">

              <div class="space-y-2">
                <Label for="email">Email</Label>
                <Input
                  id="email"
                  v-model="loginPayload.email"
                  type="email"
                  placeholder="voce@exemplo.com"
                />
              </div>

              <div class="space-y-2">
                <Label for="password">Senha</Label>
                <Input
                  id="password"
                  v-model="loginPayload.password"
                  type="password"
                  placeholder="********"
                />
              </div>

              <p v-if="errorMessage" class="text-sm text-red-500">
                {{ errorMessage }}
              </p>

              <Button
                type="submit"
                class="w-full bg-emerald-700 hover:bg-emerald-800 text-white transition-all"
                :disabled="isSubmitting"
              >
                {{ isSubmitting ? "Entrando..." : "Entrar" }}
              </Button>
            </form>
          </CardContent>

          <CardFooter class="text-sm text-gray-500 justify-center">
            Não tem uma conta?
            <RouterLink
            to="/register"
            class="text-emerald-700 font-medium hover:text-emerald-900 hover:underline pl-1 transition-colors"
            >
            Registre-se
          </RouterLink>
          </CardFooter>

        </Card>
      </div>

    </main>
  </Container>
</template>
