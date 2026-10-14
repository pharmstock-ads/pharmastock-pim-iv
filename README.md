# PharmaStock — PIM IV

Sistema de gestão de estoque farmacêutico — continuidade do [PIM III](https://github.com/pimIII/pimIII).  
Curso Superior de Tecnologia em Análise e Desenvolvimento de Sistemas — UNIP, 2026.

## Stack deste semestre

| Camada | Tecnologia |
|---|---|
| Web | ASP.NET Core MVC (Razor) |
| API | ASP.NET Core Web API (REST + Swagger) |
| Negócio / Dados | Bibliotecas .NET — Negócio → Repositório → Modelos |
| Banco | SQL Server (trigger + stored procedure obrigatórios) |
| Mobile | Flutter consumindo a Web API |

## Estrutura do repositório

```
pharmastock-pim-iv/
├── src/                      ← Solution principal (PIM IV)
│   ├── PharmaStock.sln
│   ├── PharmaStock.Modelos
│   ├── PharmaStock.Repositorio
│   ├── PharmaStock.Negocio
│   ├── PharmaStock.Web
│   ├── PharmaStock.Api
│   └── PharmaStock.Tests
├── mobile/                   ← App Flutter
├── database/                 ← Scripts SQL Server
├── docs/
│   ├── pim_IV/               ← Documentação deste semestre
│   ├── pim_III/              ← Arquivo enxuto do semestre passado
│   └── manual/               ← Manual do PIM IV
├── uiux/                     ← Casos de uso e protótipos
└── legacy/                   ← Console, WinForms, HTML e PG do PIM III
```

Dependências (manual):

```
Web ──┐
      ├──► Negocio ──► Repositorio ──► Modelos
Api ──┘
```

## Pré-requisitos

- .NET 10 SDK (`dotnet --version`)
- SQL Server (LocalDB, Express ou full)
- Flutter SDK (para o app mobile)
- Windows recomendado para desenvolvimento local

## Como rodar

```bash
cd src
dotnet restore PharmaStock.sln
dotnet build PharmaStock.sln

# Web (MVC)
dotnet run --project PharmaStock.Web

# API
dotnet run --project PharmaStock.Api

# Testes
dotnet test PharmaStock.Tests
```

## Legado (PIM III)

Código do semestre passado está em `legacy/` (console, WinForms, protótipo HTML, PostgreSQL). Serve como referência de domínio — a entrega do IV é a solution em `src/`.

## Licença

MIT
