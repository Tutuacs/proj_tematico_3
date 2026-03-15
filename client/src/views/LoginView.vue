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
    <main class="grid min-h-[calc(100vh-65px)] place-items-center p-4">
      <Card class="w-full max-w-md">
        <CardHeader>
          <CardTitle>Entrar</CardTitle>
          <CardDescription>Use seu email e senha para acessar.</CardDescription>
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

            <p v-if="errorMessage" class="text-sm text-destructive">
              {{ errorMessage }}
            </p>

            <Button type="submit" class="w-full" :disabled="isSubmitting">
              {{ isSubmitting ? "Entrando..." : "Entrar" }}
            </Button>
          </form>
        </CardContent>

        <CardFooter class="text-xs text-muted-foreground">
          Não tem uma conta?
          <RouterLink to="/register" class="text-primary hover:underline pl-1"
            >Registre-se</RouterLink
          >
        </CardFooter>
      </Card>
    </main>
  </Container>
</template>
