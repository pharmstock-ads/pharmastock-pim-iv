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
| Web | ASP.NET Core MVC (Razor) — chama **Negócio**, não a Api |
| API | ASP.NET Core Web API (REST + OpenAPI + **Scalar**) — consumida pelo **Flutter** |
| Camadas | Modelos → Repositório → Negócio → Web / Api |
| Persistência | **Dapper** (CRUD + stored procedures) — sem EF Migrations |
| Banco | **SQL Server** (Express / LocalDB / full) |
| Mobile | Flutter consumindo a Api |
| Testes | xUnit (+ testes Flutter em `mobile/test/`) |

---

## Arquitetura (manual)

```
Browser ──► Web (MVC/Razor) ──┐
                              ├──► Negocio ──► Repositorio ──► Modelos
Flutter ──► Api (REST) ───────┘                      │
                                                     └──► SQL Server
Scalar ───► Api (só para testar rotas)
```

Regras importantes:

- Web e Api **não** acessam o banco nem o Repositório diretamente  
- Controllers só recebem classes de **Negócio** (`*Negocio`)  
- A Web **não** chama HTTP na Api; os dois hosts compartilham o Negócio  
- DTOs ficam em `PharmaStock.Modelos/Dtos/`  
- Connection string só em `appsettings.json`, chave **`Padrao`**  
- DI idêntica nos dois `Program.cs` (Web e Api)  
- Procedure chamada pela camada Repositório (Dapper)  
- Auth MVP: sem JWT; senha demo no seed (ver credenciais abaixo)

---

## Estrutura do repositório

```
pharmastock-pim-iv/
├── src/                         ← Solution principal (entrega do IV)
│   ├── PharmaStock.sln
│   ├── PharmaStock.Modelos      (+ Dtos/)
│   ├── PharmaStock.Repositorio  (Dapper)
│   ├── PharmaStock.Negocio      (*Negocio + FEFO)
│   ├── PharmaStock.Web          (MVC + sessão)
│   ├── PharmaStock.Api          (REST + Scalar)
│   └── PharmaStock.Tests
├── mobile/                      ← App Flutter (A7: alertas)
├── database/                    ← schema.sql, procedures.sql, triggers.sql, seed.sql
├── docs/
│   ├── manual/
│   ├── pim_IV/                  ← contrato, guias, ordem de desenvolvimento
│   └── pim_III/
├── uiux/
└── legacy/                      ← Console, WinForms, HTML e PG do PIM III
```

Checklist vivo: [`docs/pim_IV/ORDEM_DESENVOLVIMENTO.md`](docs/pim_IV/ORDEM_DESENVOLVIMENTO.md)


---

## Pré-requisitos

1. [.NET 10 SDK](https://dotnet.microsoft.com/download) — `dotnet --version`  
2. **SQL Server** (Express, LocalDB ou full) + [SSMS](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)  
3. Flutter SDK (só para o app) — ver [`mobile/README.md`](mobile/README.md)  
4. Windows recomendado para desenvolvimento local  

> **SSMS ≠ banco.** O Management Studio só conecta e executa scripts. É preciso o **motor** SQL Server instalado.

---

## Como subir o banco

### 1. Conectar no SSMS

| Instalação | Server name |
|------------|-------------|
| Express | `localhost\SQLEXPRESS` |
| LocalDB | `(localdb)\MSSQLLocalDB` |

Autenticação: **Windows Authentication**.

```powershell
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

### 2. Criar o banco

```sql
CREATE DATABASE PharmaStock;
GO
```

### 3. Scripts — ordem oficial

Pasta: [`database/`](database/) · detalhe: [`database/README.md`](database/README.md)

| Ordem | Arquivo |
|------:|---------|
| 1 | `schema.sql` |
| 2 | `procedures.sql` |
| 3 | `triggers.sql` |
| 4 | `seed.sql` |

(Alternativa: scripts individuais em `tabelas/`, `procedure/`, `trigger/`, `inserts/` — manter sincronizados com os consolidados.)

O seed deixa lotes com **quantidade 0**. Estoque sobe com **entrada** (Web `/Entradas/Registrar` ou `POST /api/entradas` no Scalar).

### Credenciais de demonstração

| Campo | Valor |
|-------|--------|
| E-mail | `ana.souza@pharmastock.local` |
| Senha | `hash_demo_ana` |

(Outros usuários no seed: `hash_demo_carlos`, `hash_demo_mariana`.)

---

## Connection string

Chave **`Padrao`** em:

- `src/PharmaStock.Api/appsettings.json`  
- `src/PharmaStock.Web/appsettings.json`  

```json
{
  "ConnectionStrings": {
    "Padrao": "Server=localhost\\SQLEXPRESS;Database=PharmaStock;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

| Seu SQL | `Server=` |
|---------|-----------|
| Express | `localhost\\SQLEXPRESS` |
| LocalDB | `(localdb)\\mssqllocaldb` |

---

## Como rodar

```bash
git clone https://github.com/pharmstock-ads/pharmastock-pim-iv.git
cd pharmastock-pim-iv/src

# 1) Ajuste ConnectionStrings:Padrao (Api e Web)
# 2) Banco PharmaStock criado + scripts aplicados

dotnet restore PharmaStock.sln
dotnet build PharmaStock.sln
dotnet test PharmaStock.Tests
```

### API (Flutter / Scalar)

```bash
dotnet run --project PharmaStock.Api --launch-profile http
```

- Scalar: **http://localhost:5152/scalar**  
- Base REST: `http://localhost:5152/api`

Fluxo rápido no Scalar: login → produtos → **entrada** → venda → alertas.

### Web (MVC)

```bash
dotnet run --project PharmaStock.Web --launch-profile http
```

- URL: **http://localhost:5238**  
- Sem sessão → redireciona para login  
- Após login: hub Início (operações) + **Dashboard** (resumo/alertas)  
- Caminho feliz: fornecedor/produto → **entrada** → venda → lotes / listagem de vendas  

A Web **não** precisa da Api no ar. Detalhe: [`src/PharmaStock.Web/README.md`](src/PharmaStock.Web/README.md).

### Mobile (Flutter)

```bash
cd mobile
powershell -ExecutionPolicy Bypass -File .\configurar.ps1   # 1ª vez
flutter run
# Windows: flutter run -d windows --dart-define=API_BASE_URL=http://localhost:5152/api
```

Detalhes, emulador e alertas vazios: [`mobile/README.md`](mobile/README.md).  
Api precisa estar rodando. Chrome Flutter sem CORS na Api → use emulador ou Windows.

---

## MVP funcional

| # | Item | Onde |
|---|------|------|
| 1 | Login (sem JWT) | Web + Api + Flutter |
| 2 | Fornecedor / produto | Api ✅ · Web ✅ |
| 3 | Entrada de lote | Api ✅ · Web ✅ |
| 4 | Venda FEFO | Api ✅ · Web ✅ |
| 5 | Alertas A7 | Api + Flutter ✅ · Web Dashboard (resumo) |
| 6 | Listagens Web | ✅ vendas + lotes por produto |

Contrato HTTP: [`docs/pim_IV/CONTRATO_TECNICO.md`](docs/pim_IV/CONTRATO_TECNICO.md) §7.

---

## Papéis

| Papel | Quem | Foco |
|-------|------|------|
| Núcleo C# + API (+ líder UNIP) | Anderson | Modelos, Negócio, Repositório, Api |
| Banco | Geovanni | Schema, SP, trigger, seed — SQL Server |
| Web | Vinicius | MVC + responsivo |
| Mobile | Roberto | Flutter A7 |
| Eng. Software Ágil | Geovanna | RF/RNF, backlog · [Trello](https://trello.com/b/9ifnCGAL/pim-iv) |
| UX/UI | Daniel | Personas, fluxos, protótipos |
| Documentação | _a definir_ | ABNT |

Guias: [`docs/pim_IV/README.md`](docs/pim_IV/README.md)

---

## Legado (PIM III)

Em `legacy/`: console, WinForms, HTML e PostgreSQL. Só referência — a entrega do IV usa **SQL Server** em `database/`.

---

## Pendências conhecidas

- Web: **prints ABNT** (desktop × ~375px) + trecho de controller no Word  
- Mobile: ensaio ponta a ponta + capturas (estoque via entrada)  
- Ágil: prosa de negócio / sprints no board  
- Hash real de senha / JWT — fora do MVP (planos internos do time)  
- Plataforma UNIP: até **6** integrantes  

---

## Licença

MIT
