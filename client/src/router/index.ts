import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import HortaView from '@/views/Hortas/HortaView.vue'
import HortaDetalheView from '@/views/Hortas/HortaDetalheView.vue'
import HortaPlantiosView from '@/views/Hortas/HortaPlantiosView.vue'
import HortaMembrosView from '@/views/Hortas/HortaMembrosView.vue'
import NovaHortaView from '@/views/Hortas/NovaHortaView.vue'
import PlantasView from '@/views/Plantas/PlantasView.vue'
import NovaEspecieView from '@/views/Plantas/NovaEspecieView.vue'
import TarefasView from '@/views/TarefasView.vue'
import NovaTarefaView from '@/views/Tarefas/NovaTarefaView.vue'
import TarefaDetalheView from '@/views/Tarefas/TarefaDetalheView.vue'
import ConfiguracoesView from '@/views/ConfiguracoesView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import MembroEditView from '@/views/Membros/MembroEditView.vue'
import PlantioEditView from '@/views/Plantios/PlantioEditView.vue'
import AdicionaMembroView from '@/views/Membros/AdicionaMembroView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/horta',
      name: 'horta',
      component: HortaView,
    },
    {
      path: '/horta/nova',
      name: 'nova-horta',
      component: NovaHortaView,
    },
    {
      path: '/horta/:id',
      name: 'horta-detalhe',
      component: HortaDetalheView,
    },
    {
      path: '/horta/:id/plantios',
      name: 'horta-plantios',
      component: HortaPlantiosView,
    },
    {
      path: '/horta/:id/membros',
      name: 'horta-membros',
      component: HortaMembrosView,
    },
    {
      path: '/plantas',
      name: 'plantas',
      component: PlantasView,
    },
    {
      path: '/tarefas',
      name: 'tarefas',
      component: TarefasView,
    },
    {
      path: '/tarefas/nova',
      name: 'nova-tarefa',
      component: NovaTarefaView,
    },
    {
      path: '/tarefas/:id',
      name: 'tarefa-detalhe',
      component: TarefaDetalheView,
    },
    {
      path: '/configuracoes',
      name: 'configuracoes',
      component: ConfiguracoesView,
    },
    {
      path: '/membro/:id',
      name: 'membro-edit',
      component: MembroEditView,
    },
    {
      path: '/plantio/:id',
      name: 'plantio-edit',
      component: PlantioEditView,
    },
    {
      path: '/plantas/nova-especie',
      name: 'nova-especie',
      component: NovaEspecieView,
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
    },
    {
      path: '/horta/:id/membros/adicionar',
      name: 'horta-adicionar-membro',
      component: () => AdicionaMembroView
    }
  ],
})

router.beforeEach((to) => {
  const isLoggedIn = Boolean(localStorage.getItem('token'))
  const publicRoutes = ['/login', '/register']

  if (!isLoggedIn && !publicRoutes.includes(to.path)) {
    return '/login'
  }

  if (isLoggedIn && publicRoutes.includes(to.path)) {
    return '/'
  }

  return true
})

export default router
