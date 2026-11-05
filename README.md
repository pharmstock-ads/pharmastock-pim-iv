# PharmaStock — PIM IV

Sistema de gestão de estoque farmacêutico — **continuidade** do [PIM III](https://github.com/pimIII/pimIII).

**Curso:** Análise e Desenvolvimento de Sistemas — UNIP Ribeirão Preto, 2026  
**Repositório:** [pharmstock-ads/pharmastock-pim-iv](https://github.com/pharmstock-ads/pharmastock-pim-iv)

No III: Console + WinForms (memória), scripts PostgreSQL só de referência e protótipo HTML.  
No IV: ASP.NET Core MVC + Web API, **SQL Server** (trigger + procedure), app Flutter e documentação ABNT.

---

## Stack

| Camada | Tecnologia |
|--------|------------|
| Linguagem | C# / .NET 10 |
| Web | ASP.NET Core MVC (Razor) |
| API | ASP.NET Core Web API (REST + Swagger) |
| Camadas | Modelos → Repositório → Negócio → Web / Api |
| Banco | **SQL Server** (LocalDB / Express / full) |
| Mobile | Flutter consumindo a API |
| Testes | xUnit |

---

## Arquitetura (manual)

```
Web ──┐
      ├──► Negocio ──► Repositorio ──► Modelos
Api ──┘                      │
                             └──► SQL Server (trigger + stored procedure)
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
│   ├── PharmaStock.Modelos
│   ├── PharmaStock.Repositorio
│   ├── PharmaStock.Negocio
│   ├── PharmaStock.Web
│   ├── PharmaStock.Api
│   └── PharmaStock.Tests
├── mobile/                      ← App Flutter
├── database/                    ← SQL Server (schema, procedure/, trigger/, seed)
├── docs/
│   ├── manual/
│   ├── pim_IV/
│   └── pim_III/
├── uiux/
└── legacy/                      ← Console, WinForms, HTML e PG do PIM III (só referência)
```

---

## Status atual

| Área | Situação |
|------|----------|
| Estrutura do repo + solution | Pronta |
| Domínio (`Modelos`) | MVP criado (adaptar `Entrada`/`EntradaItem` ao SQL novo) |
| Testes de domínio | 10 testes xUnit |
| Banco — procedures/triggers | **Entregues** (entrada, venda, movimentação, lote) |
| Banco — `schema.sql` / seed | Pendente |
| Repositório / Negócio / API | A iniciar (já há contrato das SPs) |
| Web / Mobile | Vinicius (Web) / Roberto (Mobile) |
| Docs de equipe | Atualizados em `docs/pim_IV/` |

### Entidades em `PharmaStock.Modelos` (hoje)

`Fornecedor`, `Funcionario`, `Categoria`, `Produto`, `ProdutoPreco`, `Lote`, `Movimentacao`, `TipoMovimentacao`, `UsuarioLog`, `Venda`, `ItemVenda`

### Procedures / triggers (resumo)

Ver detalhe em [`database/README.md`](database/README.md).

- SPs: `prc_RegistrarEntrada`, `prc_RegistrarEntradaItem`, `prc_RegistrarVenda`, `prc_RegistrarVendaItem`, `prc_MovimentaItens`
- Triggers: `trg_MovimentaItens`, `trg_RecalculaEntrada`, `trg_RecalculaVenda`

---

## MVP funcional

1. Login / funcionário  
2. Fornecedor e produto  
3. Entrada de lote  
4. Venda com baixa de estoque (FIFO no Negócio + SPs/triggers)  
5. Listagens na Web  
6. Funcionalidade complementar no Flutter  

Roteiro: [`docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md`](docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md)  
Decisões: [`docs/pim_IV/RELATORIO_DECISOES_KICKOFF.md`](docs/pim_IV/RELATORIO_DECISOES_KICKOFF.md)

---

## Papéis (resumo)

| Papel | Quem | Foco |
|-------|------|------|
| Núcleo C# + API | Anderson | Modelos, Negócio, Repositório, Api |
| Banco | Geovanni | Schema, trigger, procedure, seed — **SQL Server** |
| Web | Vinicius | MVC + responsivo |
| Mobile | Roberto | Flutter |
| Eng. Software Ágil | Geovanna | RF/RNF, backlog, sprints |
| Documentação | _a definir_ | ABNT / consolidação |
| UX/UI | _a definir_ | Personas, fluxos, protótipos |

Ordem: **Ágil → Banco + Modelos → Negócio/Repositório → API → Web/Mobile**.

---

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download) — `dotnet --version`
- **SQL Server** (LocalDB, Express ou full)
- Flutter SDK (app mobile)
- Windows recomendado para desenvolvimento local

---

## Como rodar

```bash
git clone https://github.com/pharmstock-ads/pharmastock-pim-iv.git
cd pharmastock-pim-iv/src

dotnet restore PharmaStock.sln
dotnet build PharmaStock.sln

dotnet test PharmaStock.Tests
dotnet run --project PharmaStock.Web
dotnet run --project PharmaStock.Api
```

Scripts de banco: pasta `database/` (aplicar procedures/triggers após o schema).  
Manual: `docs/manual/Manual_PIM_IV_2026.docx`

---

## Legado (PIM III)

Em `legacy/`: console, WinForms, HTML e PostgreSQL antigo.  
Só referência de domínio — a entrega do IV usa **SQL Server** em `database/`.

---

## Pendências conhecidas

- Completar `database/schema.sql` e `seed.sql`
- Núcleo chamar as SPs no Repositório + entidades `Entrada`/`EntradaItem`
- Grupo na plataforma UNIP: limite de **até 6** integrantes

---

## Licença

MIT
