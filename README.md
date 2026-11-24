# PharmaStock — PIM IV

Sistema de gestão de estoque farmacêutico — **continuidade** do [PIM III](https://github.com/pimIII/pimIII).

**Curso:** Análise e Desenvolvimento de Sistemas — UNIP Ribeirão Preto, 2026  
**Repositório:** [pharmstock-ads/pharmastock-pim-iv](https://github.com/pharmstock-ads/pharmastock-pim-iv)

| PIM III | PIM IV |
|---------|--------|
| Console + WinForms (memória) | ASP.NET Core **MVC** + **Web API** |
| Scripts PostgreSQL (referência) | **SQL Server** (procedures + triggers) |
| Protótipo HTML | App **Flutter** + documentação ABNT |

---

## Stack

| Camada | Tecnologia |
|--------|------------|
| Linguagem | C# / **.NET 10** |
| Web | ASP.NET Core MVC (Razor) — chama **Negócio**, não a Api |
| API | ASP.NET Core Web API (REST + OpenAPI + **Scalar**) — consumida pelo **Flutter** |
| Camadas | Modelos → Repositório → Negócio → Web / Api |
| Persistência | **Dapper** (CRUD + stored procedures) — sem EF Migrations |
| Banco | **SQL Server** (Express / LocalDB / full) |
| Auth | Sessão na Web; Api sem JWT. Senha com **BCrypt** no Negócio |
| Mobile | Flutter consumindo a Api (alertas A7) |
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

- Web e Api **não** acessam o banco nem o Repositório nos controllers  
- Controllers só recebem classes de **Negócio** (`*Negocio`)  
- A Web **não** chama HTTP na Api; os dois hosts compartilham o Negócio  
- DTOs ficam em `PharmaStock.Modelos/Dtos/`  
- Connection string só em `appsettings.json`, chave **`Padrao`**  
- DI idêntica nos dois `Program.cs` (Web e Api)  
- Procedure chamada pela camada Repositório (Dapper)  
- Login: cliente envia senha **plana**; o Negócio verifica com **BCrypt** contra `senha_hash`  
- Cadastro de funcionário: `POST /api/funcionarios` (Api/Scalar) — hasheia antes de gravar  

---

## Estrutura do repositório

```
pharmastock-pim-iv/
├── src/                         ← Solution principal (entrega do IV)
│   ├── PharmaStock.sln
│   ├── PharmaStock.Modelos      (+ Dtos/)
│   ├── PharmaStock.Repositorio  (Dapper)
│   ├── PharmaStock.Negocio      (*Negocio + FEFO + SenhaHasher)
│   ├── PharmaStock.Web          (MVC + sessão)
│   ├── PharmaStock.Api          (REST + Scalar)
│   └── PharmaStock.Tests
├── mobile/                      ← App Flutter (A7: alertas)
├── database/                    ← schema, procedures, triggers, seed
├── docs/
│   ├── manual/
│   ├── pim_IV/                  ← contrato, ordem de desenvolvimento
│   └── pim_III/
├── uiux/
└── legacy/                      ← Console, WinForms, HTML e PG do PIM III
```

Checklist vivo: [`docs/pim_IV/ORDEM_DESENVOLVIMENTO.md`](docs/pim_IV/ORDEM_DESENVOLVIMENTO.md)  
Contrato HTTP: [`docs/pim_IV/CONTRATO_TECNICO.md`](docs/pim_IV/CONTRATO_TECNICO.md)

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

(Alternativa: scripts em `tabelas/`, `procedure/`, `trigger/`, `inserts/` — manter sincronizados com os consolidados.)

O seed deixa lotes com **quantidade 0**. Estoque sobe com **entrada** (Web `/Entradas/Registrar` ou `POST /api/entradas` no Scalar).

### Credenciais de demonstração

| Campo | Valor |
|-------|--------|
| E-mail (Ana) | `ana.souza@pharmastock.local` |
| Senha | `@Ana123` |

A mesma senha plana vale para **Carlos** (`carlos.lima@pharmastock.local`) e **Mariana** (`mariana.alves@pharmastock.local`).  
No banco fica **apenas** o hash BCrypt em `Funcionario.senha_hash` (`VARCHAR(255)`).

**Banco antigo** (ainda com `hash_demo_*`): rode o seed novo **ou** atualize as três linhas:

```sql
-- Só se a coluna ainda for curta (ex. varchar(25)):
-- ALTER TABLE dbo.Funcionario ALTER COLUMN senha_hash VARCHAR(255) NOT NULL;

UPDATE dbo.Funcionario
SET senha_hash = '$2a$11$.Nl.EB9iVvItIxEW4B7gU.FDFb.6gcxid2W0jNQRwAHRFJVu3KtAG'
WHERE email = 'ana.souza@pharmastock.local';

UPDATE dbo.Funcionario
SET senha_hash = '$2a$11$dvJ.Ztubo1QchHEw7FK2a.7ZnSZiaQZgPkjZcJmCwGUOtWHPeXxkC'
WHERE email = 'carlos.lima@pharmastock.local';

UPDATE dbo.Funcionario
SET senha_hash = '$2a$11$OQSTMBPIJW/Wf.sQ49ieEe9hTB0dW03Rw8DQKLgo3oBhFdW5jwSQW'
WHERE email = 'mariana.alves@pharmastock.local';
```

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

| Recurso | URL |
|---------|-----|
| Api | http://localhost:5152 |
| Scalar | **http://localhost:5152/scalar** |
| Base REST | `http://localhost:5152/api` |

Fluxo rápido no Scalar:

1. `POST /api/auth/login` (ou `POST /api/funcionarios` para criar um usuário novo)  
2. `GET /api/produtos`  
3. `POST /api/entradas` (sobe estoque)  
4. `POST /api/vendas` (FEFO no Negócio)  
5. `GET /api/alertas/...`  

**Criar funcionário** (senha plana no body → hash no banco):

```http
POST /api/funcionarios
```

```json
{
  "nome": "Convidado Demo",
  "cpf": "11122233344",
  "email": "demo@pharmastock.local",
  "senha": "@Ana123",
  "cargo": "Atendente"
}
```

Depois o mesmo e-mail/senha funcionam no login da Web e no `POST /api/auth/login` (Mobile).

Detalhe: [`src/PharmaStock.Api/README.md`](src/PharmaStock.Api/README.md).

### Web (MVC)

```bash
dotnet run --project PharmaStock.Web --launch-profile http
```

- URL: **http://localhost:5238**  
- Sem sessão → redireciona para login (`@Ana123` com o e-mail do seed)  
- Após login: **Início** (hub) + **Dashboard** (resumo/alertas)  
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
| 1 | Login (BCrypt, sem JWT) | Web + Api + Flutter |
| 2 | Criar funcionário | Api (`POST /api/funcionarios`) |
| 3 | Fornecedor / produto | Api ✅ · Web ✅ |
| 4 | Entrada de lote | Api ✅ · Web ✅ |
| 5 | Venda FEFO | Api ✅ · Web ✅ |
| 6 | Alertas A7 | Api + Flutter ✅ · Web Dashboard (resumo) |
| 7 | Listagens Web | ✅ vendas (produto, qtd, funcionário) + lotes por produto |

---


## Legado (PIM III)

Em `legacy/`: console, WinForms, HTML e PostgreSQL. Só referência — a entrega do IV usa **SQL Server** em `database/`.

---


## Licença

MIT
