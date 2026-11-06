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
| API | ASP.NET Core Web API (REST + OpenAPI + **Scalar**) |
| Camadas | Modelos → Repositório → Negócio → Web / Api |
| Persistência | **Dapper** (CRUD + stored procedures) — sem EF Migrations |
| Banco | **SQL Server** (Express / LocalDB / full) |
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
- Controllers só recebem classes de **Negócio** (`*Negocio`)  
- DTOs ficam em `PharmaStock.Modelos/Dtos/` (Web e Api compartilham)  
- Connection string só em `appsettings.json`, chave **`Padrao`**  
- DI idêntica nos dois `Program.cs` (Web e Api)  
- Procedure chamada pela camada Repositório (Dapper)

---

## Estrutura do repositório

```
pharmastock-pim-iv/
├── src/                         ← Solution principal (entrega do IV)
│   ├── PharmaStock.sln
│   ├── PharmaStock.Modelos      (+ Dtos/)
│   ├── PharmaStock.Repositorio  (Dapper)
│   ├── PharmaStock.Negocio      (*Negocio + FEFO)
│   ├── PharmaStock.Web
│   ├── PharmaStock.Api          (REST + Scalar)
│   └── PharmaStock.Tests
├── mobile/                      ← App Flutter
├── database/                    ← SQL Server (tabelas, procedure/, trigger/, inserts)
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
| Modelos + DTOs | Alinhados ao schema + `Modelos/Dtos/` |
| Negócio (FEFO, auth, entrada, venda, alertas) | Pronto (`*Negocio`) |
| Repositório Dapper + SPs | Pronto |
| API REST + Scalar | Pronto — fluxo testável em `/scalar` |
| Banco — procedures/triggers/inserts | Entregues em `database/` |
| Banco — `schema.sql` unificado | Pendente (Geovanni; usar `tabelas/tabelas.txt`) |
| Web MVC | Vinicius (DI já registrada) |
| Mobile | Roberto |
| Docs de equipe | `docs/pim_IV/` |

---

## Pré-requisitos

1. [.NET 10 SDK](https://dotnet.microsoft.com/download) — `dotnet --version`  
2. **SQL Server** (Express, LocalDB ou full) + [SSMS](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)  
3. Flutter SDK (só para o app mobile)  
4. Windows recomendado para desenvolvimento local  

> **SSMS ≠ banco.** O Management Studio só conecta e executa scripts. Você precisa do **motor** SQL Server instalado.

---

## Como subir o banco (passo a passo)

### 1. Instalar / confirmar o SQL Server

No PowerShell:

```powershell
# Se usou LocalDB:
sqllocaldb info
sqllocaldb start MSSQLLocalDB

# Se usou Express, no SSMS o servidor costuma ser:
# localhost\SQLEXPRESS
```

### 2. Conectar no SSMS

| Instalação | Server name no SSMS |
|------------|---------------------|
| Express (comum) | `localhost\SQLEXPRESS` |
| LocalDB | `(localdb)\MSSQLLocalDB` |

Autenticação: **Windows Authentication**.

### 3. Criar o banco

```sql
CREATE DATABASE PharmaStock;
GO
```

Selecione o banco `PharmaStock` no SSMS (ou `USE PharmaStock;`).

### 4. Rodar os scripts **nesta ordem**

Pasta: [`database/`](database/)

| Ordem | Arquivo(s) |
|-------|------------|
| 1 | `tabelas/tabelas.txt` |
| 2 | todos em `procedure/*.txt` |
| 3 | todos em `trigger/*.txt` |
| 4 | `inserts/inserts.txt` |

Copie o conteúdo de cada arquivo → New Query no SSMS → Execute (F5).

Confira: tabelas em **Tables**, `prc_*` em **Programmability → Stored Procedures**, triggers nas tabelas.

### 5. (Recomendado) Gerar estoque demo

O seed deixa lotes com **quantidade 0**. Depois de subir a Api, faça um `POST /api/entradas` no Scalar (ou peça ao Geovanni um script de entrada demo — B2).

Login seed:

- e-mail: `ana.souza@pharmastock.local`  
- senha: `hash_demo_ana`

---

## Connection string — ajustar por máquina

A chave no código é sempre **`Padrao`**. O valor de **`Server=` muda em cada PC**.

Arquivos:

- `src/PharmaStock.Api/appsettings.json`  
- `src/PharmaStock.Web/appsettings.json`  

Exemplo (Express — máquina de referência do núcleo):

```json
{
  "ConnectionStrings": {
    "Padrao": "Server=localhost\\SQLEXPRESS;Database=PharmaStock;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

| Seu SQL | Troque só o `Server=` |
|---------|------------------------|
| Express | `localhost\\SQLEXPRESS` |
| LocalDB | `(localdb)\\mssqllocaldb` |
| Outro host | `NOME-DO-PC\\SQLEXPRESS` |

Se no SSMS você usa usuário/senha SQL (`sa`), use `User Id=...;Password=...` em vez de `Trusted_Connection=True`.

O C# só chama `GetConnectionString("Padrao")` — **não** precisa mudar código ao trocar de máquina.

Detalhes: [`database/README.md`](database/README.md) · [`docs/pim_IV/CONTRATO_TECNICO.md`](docs/pim_IV/CONTRATO_TECNICO.md)

---

## Como rodar a solution

```bash
git clone https://github.com/pharmstock-ads/pharmastock-pim-iv.git
cd pharmastock-pim-iv/src

# 1) Ajuste ConnectionStrings:Padrao nos appsettings (Api e Web) — ver seção acima
# 2) Banco PharmaStock já criado e scripts aplicados

dotnet restore PharmaStock.sln
dotnet build PharmaStock.sln
dotnet test PharmaStock.Tests

dotnet run --project PharmaStock.Api
# Documentação interativa: http://localhost:5152/scalar  (perfil http)
# ou https://localhost:7275/scalar (perfil https)

dotnet run --project PharmaStock.Web
```

### Fluxo rápido no Scalar

1. `POST /api/auth/login`  
2. `GET /api/produtos`  
3. `POST /api/entradas` (sobe estoque)  
4. `POST /api/vendas` (FEFO)  
5. `GET /api/alertas/lotes-a-vencer` e `/api/alertas/estoque-baixo`  

Contrato HTTP: [`docs/pim_IV/CONTRATO_TECNICO.md`](docs/pim_IV/CONTRATO_TECNICO.md) §7.

---

## MVP funcional

1. Login / funcionário (sem JWT no MVP)  
2. Fornecedor e produto  
3. Entrada de lote (lote nasce na entrada — sem `POST /api/lotes`)  
4. Venda com FEFO + baixa de estoque (SPs/triggers)  
5. Alertas A7 (lotes a vencer / estoque baixo)  
6. Listagens na Web (Vinicius)  
7. Feature complementar no Flutter (Roberto)  

Roteiro: [`docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md`](docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md)  
Eng. Software Ágil: [`docs/pim_IV/RELATORIO_ENG_SOFTWARE_AGIL.md`](docs/pim_IV/RELATORIO_ENG_SOFTWARE_AGIL.md)

---

## Papéis (resumo)

| Papel | Quem | Foco |
|-------|------|------|
| Núcleo C# + API | Anderson | Modelos, Negócio, Repositório, Api |
| Banco | Geovanni | Schema, trigger, procedure, seed — **SQL Server** |
| Web | Vinicius | MVC + responsivo |
| Mobile | Roberto | Flutter |
| Eng. Software Ágil | Geovanna | RF/RNF, backlog, sprints · [Trello](https://trello.com/b/9ifnCGAL/pim-iv) |
| Documentação | _a definir_ | ABNT / consolidação |
| UX/UI | Daniel | Personas, fluxos, protótipos |

---

## Legado (PIM III)

Em `legacy/`: console, WinForms, HTML e PostgreSQL antigo.  
Só referência de domínio — a entrega do IV usa **SQL Server** em `database/`.

---

## Pendências conhecidas

- Completar `database/schema.sql` / `seed.sql` formal (Geovanni)  
- Web MVC de ponta a ponta (Vinicius)  
- App Flutter (Roberto)  
- Grupo na plataforma UNIP: limite de **até 6** integrantes  

---

## Licença

MIT
