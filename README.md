# PharmaStock — PIM IV

Sistema de gestão de estoque farmacêutico — **continuidade** do [PIM III](https://github.com/pimIII/pimIII).

**Curso:** Análise e Desenvolvimento de Sistemas — UNIP Ribeirão Preto, 2026  
**Repositório:** [pharmstock-ads/pharmastock-pim-iv](https://github.com/pharmstock-ads/pharmastock-pim-iv)

No III entregamos Console + WinForms (dados em memória), scripts PostgreSQL de referência e protótipo HTML.  
Neste semestre o foco é **finalizar o sistema integrado**: ASP.NET Core MVC + Web API, banco com trigger/procedure, app Flutter e documentação ABNT.

---

## Stack

| Camada | Tecnologia |
|--------|------------|
| Linguagem | C# / .NET 10 |
| Web | ASP.NET Core MVC (Razor) |
| API | ASP.NET Core Web API (REST + Swagger) |
| Camadas | Modelos → Repositório → Negócio → Web / Api |
| Banco | Relacional — **SQL Server no manual**; time avalia PostgreSQL (ver docs de decisão) |
| Mobile | Flutter consumindo a API |
| Testes | xUnit |

---

## Arquitetura (manual)

```
Web ──┐
      ├──► Negocio ──► Repositorio ──► Modelos
Api ──┘                      │
                             └──► Banco (trigger + stored procedure)
Flutter ──► Api
```

Regras importantes:

- Web e Api **não** acessam o banco nem o Repositório diretamente
- Regras de negócio ficam em `PharmaStock.Negocio`
- Connection string em `appsettings.json`, nunca hardcoded
- Procedure chamada pela camada Repositório

---

## Estrutura do repositório

```
pharmastock-pim-iv/
├── src/                         ← Solution principal (entrega do IV)
│   ├── PharmaStock.sln
│   ├── PharmaStock.Modelos      ← Entidades de domínio
│   ├── PharmaStock.Repositorio  ← Acesso a dados
│   ├── PharmaStock.Negocio      ← Regras de negócio
│   ├── PharmaStock.Web          ← MVC Razor
│   ├── PharmaStock.Api          ← Web API (Flutter)
│   └── PharmaStock.Tests
├── mobile/                      ← App Flutter
├── database/                    ← Scripts SQL (schema, trigger, procedure, seed)
├── docs/
│   ├── manual/                  ← Manual do PIM IV
│   ├── pim_IV/                  ← Roteiro, decisões, doc acadêmico
│   └── pim_III/                 ← Diagramas/material útil do semestre passado
├── uiux/                        ← Casos de uso e protótipos
└── legacy/                      ← Console, WinForms, HTML e PG do PIM III
```

---

## Status atual

| Área | Situação |
|------|----------|
| Estrutura do repo + solution | Pronta |
| Domínio (`Modelos`) | MVP criado (entidades do III adaptadas) |
| Testes de domínio | 10 testes xUnit (código de barras, subtotal) |
| Repositório / Negócio / API | A iniciar |
| Web / Mobile / Banco físico | A cargo dos respectivos papéis |
| Docs de equipe | Roteiro + relatório de kickoff em `docs/pim_IV/` |

### Entidades em `PharmaStock.Modelos`

`Fornecedor`, `Funcionario`, `Categoria`, `Produto`, `ProdutoPreco`, `Lote`, `Movimentacao`, `TipoMovimentacao`, `UsuarioLog`, `Venda`, `ItemVenda`

---

## MVP funcional (partida do backlog)

1. Login / funcionário  
2. Fornecedor e produto  
3. Entrada de lote  
4. Venda com baixa FIFO por validade  
5. Listagens na Web  
6. Uma funcionalidade complementar no Flutter  

Detalhes e checklists: [`docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md`](docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md)  
Decisões do kickoff (para Ágil): [`docs/pim_IV/RELATORIO_DECISOES_KICKOFF.md`](docs/pim_IV/RELATORIO_DECISOES_KICKOFF.md)

---

## Papéis (resumo)

| Papel | Foco |
|-------|------|
| Núcleo C# + API | Modelos, Negócio, Repositório, Api |
| Banco | Schema, trigger, procedure, seed |
| Web | MVC + responsivo |
| Mobile | Flutter |
| Eng. Software Ágil | RF/RNF, backlog, sprints |
| Documentação | ABNT / consolidação |
| UX/UI | Personas, fluxos, protótipos |

Ordem de trabalho: **Ágil → Banco + Modelos → Negócio/Repositório → API → Web/Mobile**.

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) — `dotnet --version`
- Banco relacional (SQL Server LocalDB/Express **ou** PostgreSQL, conforme decisão do grupo/professor)
- Flutter SDK (app mobile)
- Windows recomendado para desenvolvimento local

---

## Como rodar

```bash
git clone https://github.com/pharmstock-ads/pharmastock-pim-iv.git
cd pharmastock-pim-iv/src

dotnet restore PharmaStock.sln
dotnet build PharmaStock.sln

# Testes
dotnet test PharmaStock.Tests

# Web (MVC)
dotnet run --project PharmaStock.Web

# API (+ Swagger em Development)
dotnet run --project PharmaStock.Api
```

Manual oficial: `docs/manual/Manual_PIM_IV_2026.docx`

---

## Legado (PIM III)

Em `legacy/`: console, WinForms, protótipo HTML e scripts PostgreSQL.  
Útil como referência de domínio e regras (ex.: FIFO) — **não** é a entrega principal do IV.

---

## Pendências conhecidas

- Confirmar com o professor: **SQL Server (manual) vs PostgreSQL** (preferência do time / reaproveitamento do III)
- Alinhar com o time de banco: trigger, procedure e se `Venda` vira tabela própria ou só `movimentacao`
- Grupo na plataforma UNIP: manual limita a **até 6** integrantes

---

## Licença

MIT
