# Projeto Temático 3 - UCS

## Sobre o Projeto

Projeto desenvolvido para a disciplina de Projeto Temático 3 da Universidade de Caxias do Sul (UCS).

O software tem como foco a gestão e gerenciamento de **Hortas Comunitárias**.

## Funcionalidades Principais

- **Gestão de Hortas**: Cadastro e gerenciamento de hortas comunitárias
- **Gestão de Membros**: Adicionar e remover membros das hortas
- **Gestão de Tarefas**: Criar e gerenciar tarefas relacionadas às hortas (regar, plantar, colher, etc.)
- **Atribuição de Tarefas**: Designar tarefas específicas para membros da comunidade

## Estrutura do Banco de Dados

### Perfil
```
{
  id: string (UUID),
  email: string,
  senha: string,
  nome: string,
  createdAt: datetime
}
```

### Membro
```
{
  id: int,
  hortaId: int,
  perfilId: int,
  role: string  // "ADMIN", "MEMBRO", "VISUALIZADOR"
}
```

### Horta
```
{
  id: int,
  nome: string,
  descricao?: string,
  local: string,
  largura: decimal,
  profundidade: decimal,
  status?: string,  // "ATIVA", "INATIVA"
  createdAt: datetime
}
```

### Especie
```
{
  id: int,
  nome: string,
  imageLink?: string,
  diasParaRegar: int,      // intervalo entre regas (ex: 2 = a cada 2 dias)
  diasParaColher?: int,    // tempo até colheita (ex: 45 dias)
  mesesPlantio?: string,   // "3,4,5,6" (mar-jun)
  mesesColheita?: string,  // "11,12,1,2" (nov-fev)
  descricao?: string
}
```

### Plantio
```
{
  id: int,
  especieId: int,
  hortaId: int,
  dataPlantio: datetime,
  quantidade?: int,
  status?: string,  // "ATIVO", "COLHIDO", "PERDIDO"
  createdAt: datetime
}
```

### Todo
```
{
  id: int,
  tipo: string,        // "REGAR", "PLANTAR", "COLHER", "LIMPAR", "ADUBAR"
  descricao?: string,
  dataLimite: datetime,
  plantioId?: int,     // opcional - tarefa pode ser geral da horta
  hortaId: int,
  membroId?: int,      // quem pegou a tarefa
  completedAt?: datetime,  // quando concluiu
  status: string,      // "ABERTO", "EM_ANDAMENTO", "CONCLUIDO", "CANCELADO"
  createdAt: datetime
}
```