# Backend (API)

API ASP.NET Core do projeto, com Entity Framework Core + MySQL.

## Pré-requisitos

- .NET SDK instalado
- MySQL em execução
- Ferramenta EF CLI (`dotnet-ef`)

Instalar o EF CLI (se necessário):

```bash
dotnet tool install --global dotnet-ef
```

## Configuração

1. Edite as strings de conexão em `appsettings.Development.json`.
2. Confira principalmente:
   - `ConnectionStrings:DefaultConnection`
   - `ConnectionStrings:Redis` (se estiver usando cache Redis)

## Comandos de Migration

Execute dentro da pasta `api/`:

```bash
cd api
```

Criar uma nova migration(quando criada uma nova tabela ou alterada uma tabela existente):

```bash
dotnet ef migrations add NomeDaMigration
```

Aplicar migrations no banco:

```bash
dotnet ef database update
```

Listar migrations:

```bash
dotnet ef migrations list
```

## Rodar a API

Dentro de `api/`:

```bash
dotnet run
```

A API sobe com as URLs configuradas no profile de desenvolvimento (veja `Properties/launchSettings.json`).

## Fluxo rápido (resumo)

```bash
cd api
dotnet ef database update
dotnet run
```
