# Ordem de desenvolvimento — PharmaStock PIM IV

Checklist vivo do time. Atualize a coluna **Status** com:

| Emoji | Significado |
|-------|-------------|
| ✅ | Feito |
| ⚠️ | Parcial / em andamento / bloqueado por dependência |
| ❌ | Não feito |

**Atualizado em:** 21/11/2026 (revisão: banco consolidado, Web login+venda, Mobile A7 em código ✅; rascunhos Etapas 7/8/10 encaminhados ao doc)

**Complementares:** [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md) · [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [`database/README.md`](../../database/README.md) · [guias](./README.md)

---

## Ordem ideal de desenvolvimento (detalhamento)

O PharmaStock do PIM IV não deve começar pela API nem pelas telas. A ordem existe para evitar retrabalho: cada camada só faz sentido quando a anterior já está estável o bastante para o próximo papel consumir.

O ponto de partida acadêmico continua sendo o **Ágil** (Geovanna). Baseline de RF/RNF e gaps estão em [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) (§4–§5) — **alinhar §5 com ela**. Tecnicamente, núcleo/API já liberam Web/Mobile. MVP: login, fornecedor/produto, entrada, venda FEFO, listagens web, Flutter A7.

O **banco SQL Server** (Geovanni) cobre o mínimo do manual: `schema.sql` + `procedures.sql` + `triggers.sql` + `seed.sql` (ordem no [README do banco](../../database/README.md)). Seed ainda deixa lote com quantidade **0** — para demo A7, registrar entrada (Scalar/Web) com validade próxima. Detalhe: [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md). PostgreSQL do III só em `legacy/`.

O **núcleo C#** (Anderson) está fechado: Dapper + SPs + DTOs em Modelos + DI Web/Api + API REST + Scalar; fluxo login → produtos → entrada → venda → alertas validado. Web e Api não acessam banco/Repositório nos controllers (só Negócio). Connection string chave **`Padrao`** (ajuste `Server=` por máquina — ver README).

**UI hoje:** **Mobile** (Roberto) — código A7 pronto; falta só evidência (prints / ensaio) para o ABNT. **Web** (Vinicius) — sessão + login + venda já no repo; faltam demais telas do MVP (fornecedor/produto/entrada/listagens) e capturas responsivas. UX (Daniel) pode validar com o que já existe.

Por fim, o time **integra** o caminho feliz ponta a ponta (cadastro → entrada → venda → consulta no app) e fecha o **documento ABNT**. Rascunhos de Machine Learning (Etapa 7), Comunicação/LIBRAS (Etapa 8) e Integração (Etapa 10) já foram encaminhados ao responsável da documentação — entram como seções curtas no texto, não como projetos de código.

Em resumo: **Ágil define o quê → Banco e Modelos firmam os dados → Repositório e Negócio firmam as regras → API publica o contrato → Web e Mobile consomem → integração e documentação fecham a entrega.** Pular etapas (por exemplo, API ou telas sem schema) gera retrabalho e dificulta explicar o sistema na apresentação.

---

## Visão geral dos blocos

| # | Bloco | Quem | Status | Nota |
|---|--------|------|--------|------|
| 0 | Fundação do repo | Anderson / grupo | ✅ | Solution + docs + Modelos |
| 1 | Ágil / backlog | Geovanna | ⚠️ | RF/RNF no relatório; falta §5 (negócio, board, sprints) |
| 2A | Banco — schema + seed | Geovanni | ⚠️ | `schema.sql`/`seed.sql` ✅; estoque demo A7 ainda via entrada manual |
| 2B | Banco — SPs / triggers | Geovanni | ✅ | Consolidados em `procedures.sql` / `triggers.sql` |
| 3 | Modelos C# | Anderson | ✅ | Alinhados ao schema |
| 4 | Repositório + Negócio | Anderson | ✅ | Dapper + SPs + `*Negocio` + FEFO |
| 5 | API | Anderson | ✅ | REST + Scalar + DTOs Modelos |
| 6A | Web | Vinicius | ⚠️ | Login + sessão + venda; falta resto do MVP + prints |
| 6B | Mobile | Roberto | ⚠️ | **Código A7 ✅**; prints/ensaio em andamento (ABNT) |
| 6C | UX | Daniel | ⚠️ | Guia pronto; evoluir `uiux/` (III → IV) |
| 7 | Integração + ABNT | Docs + grupo | ⚠️ | Rascunhos 7/8/10 encaminhados; montagem ABNT + fluxo E2E abertos |

```
Ágil ──► Schema/seed + Modelos ──► Repo/Negócio ──► API ──► Web ∥ Mobile ──► Integração
              ▲
         SPs/triggers (✅)
```

---

## Bloco 0 — Fundação

**Quem:** Anderson / grupo · **Status do bloco:** ✅

| Item | Status |
|------|--------|
| Repo PIM IV + pastas (`src/`, `database/`, `docs/`, `legacy/`) | ✅ |
| Solution em camadas (Modelos, Repositório, Negócio, Web, Api, Tests) | ✅ |
| Docs de divisão + relatório ágil + contrato + guias | ✅ |
| SGBD = **SQL Server** (não PostgreSQL) | ✅ |
| Domínio mínimo inicial em `PharmaStock.Modelos` | ✅ |
| Testes xUnit básicos (Produto / ItemVenda) | ✅ |

---

## Bloco 1 — Ágil e escopo

**Quem:** Geovanna · **Status do bloco:** ⚠️

| Item | Status |
|------|--------|
| Texto curto do negócio fictício (PharmaStock / farmácia) | ❌ (gap manual §5.1 do relatório ágil) |
| Stakeholders / atores | ⚠️ (atores no relatório; falta lista completa) |
| PDF Ágil atualizado no repo | ✅ |
| RF e RNF (baseline RF01–22 / RNF01–16) | ⚠️ (no relatório; falta board/prosa ABNT) |
| Backlog priorizado do MVP | ⚠️ (MVP + MoSCoW; falta board) |
| Critérios de aceite dos itens de alta prioridade | ⚠️ (na tabela RF; falta por sprint) |
| Board (Trello) [pim-iv](https://trello.com/b/9ifnCGAL/pim-iv) | ⚠️ (cards criados; falta sprints) |
| 2–3 sprints propostas | ❌ |
| Líder UNIP = Anderson | ✅ |

**Critério de saída:** o time sabe o que entra na próxima sprint sem ambiguidade.

---

## Bloco 2 — Banco SQL Server

### 2A — Schema e seed

**Quem:** Geovanni · **Status do bloco:** ⚠️

> Fonte consolidada: `database/schema.sql` + `database/seed.sql`.  
> Individuais: `tabelas/`, `inserts/`, `procedure/`, `trigger/`.  
> Seed: lotes com quantidade **0** (estoque sobe só por movimentação/entrada).

| Item | Status |
|------|--------|
| CREATE TABLE versionado | ✅ (`schema.sql` + `tabelas/`) |
| `schema.sql` unificado | ✅ |
| Tabelas mínimas das SPs + cadastros | ✅ |
| Inserts de cadastro / lotes zerados | ✅ (`seed.sql` / `inserts/`) |
| Entrada demo / estoque para Flutter A7 | ⚠️ (fazer entrada no Scalar/Web; script dedicado opcional) |
| Ordem aplicável: schema → procedures → triggers → seed | ✅ (documentada no README do banco) |
| DER / modelo lógico para o ABNT | ❌ |

**Critério de saída:** banco local sobe; SPs rodam; há estoque para alertas (via entrada demo).

### 2B — Procedures e triggers

**Quem:** Geovanni · **Status do bloco:** ✅

| Item | Status |
|------|--------|
| `prc_ObterOuCadastrarLote` | ✅ |
| `prc_RegistrarEntrada` / `prc_RegistrarEntradaItem` (`@pPrecoUnitario`) | ✅ |
| `prc_RegistrarVenda` / `prc_RegistrarVendaItem` | ✅ |
| `prc_MovimentaItens` | ✅ |
| `trg_MovimentaItens` (atualiza `lote`) | ✅ |
| `trg_RecalculaEntrada` / `trg_RecalculaVenda` | ✅ |
| Consolidados `procedures.sql` / `triggers.sql` | ✅ |
| Doc Service (orientações Negócio) em `database/` | ✅ |

---

## Bloco 3 — Modelos C#

**Quem:** Anderson · **Status do bloco:** ✅

| Item | Status |
|------|--------|
| Entidades alinhadas ao schema | ✅ |
| `Entrada` e `EntradaItem` (`PrecoUnitario`) | ✅ |
| `ItemVenda` com `IdLote` | ✅ |
| `ProdutoPreco` / desconto fora do MVP (A5/A6) | ✅ (não usados no Negócio) |
| Testes de entidades atualizados | ✅ |

**Critério de saída:** cada tabela essencial do schema tem classe correspondente — **atendido**.

---

## Bloco 4 — Repositório + Negócio

**Quem:** Anderson · **Status do bloco:** ✅

### 4A — Repositório

| Item | Status |
|------|--------|
| Interfaces em `Repositorio/Abstracoes` | ✅ |
| Connection string em `appsettings.json` (chave **`Padrao`**) | ✅ |
| Implementação Dapper + SPs | ✅ |
| Wrappers: ObterOuCadastrarLote + entrada/venda (transação C15) | ✅ |
| CRUD cadastros (SQL) | ✅ |
| Dapper + sem Migrations (**C1/C2**) | ✅ |
| Cadastros SQL no Repo (**C3**) | ✅ |
| `HasTrigger` (EF) | — (não se aplica; Dapper) |

### 4B — Negócio

| Item | Status |
|------|--------|
| Validações (funcionário, produto, qtd, preço) | ✅ |
| FEFO multilote (`FefoAlocador`, C13) | ✅ |
| Classes `AuthNegocio` / `CadastroNegocio` / `EntradaNegocio` / `VendaNegocio` / `AlertaNegocio` | ✅ |
| Orquestrar via interfaces + Repo Dapper | ✅ |
| Testes xUnit (FEFO, Auth, entidades) | ✅ |

**Critério de saída:** registrar entrada/venda via Negócio + Dapper — **atendido**.

---

## Bloco 5 — API

**Quem:** Anderson · **Status do bloco:** ✅

| Item | Status |
|------|--------|
| Controllers REST + DTOs em `Modelos/Dtos` | ✅ |
| Endpoints do MVP (contrato §7, alertas A7; **sem** POST /lotes — C4) | ✅ |
| Auth MVP sem JWT (`funcionarioId`) | ✅ |
| OpenAPI + **Scalar** (`/scalar`) | ✅ |
| Lista de rotas para Roberto e Vinicius | ✅ (CONTRATO §7 + Scalar) |
| DI no `Program.cs` (Api **e** Web) | ✅ |

**Critério de saída:** Scalar executa fluxo completo (login → entrada → venda → alertas) — **atendido**.

---

## Bloco 6 — Interfaces *(paralelo após API mínima)*

### 6A — Web

**Quem:** Vinicius · **Status do bloco:** ⚠️ · Guia: [GUIA_WEB_VINICIUS.md](./GUIA_WEB_VINICIUS.md)

| Item | Status |
|------|--------|
| Sessão + `AccountController` (login via `AuthNegocio`) | ✅ |
| `VendasController` (grava via `VendaNegocio`, sem lote na tela) | ✅ |
| Menu / layout Bootstrap (navbar colapsável) | ✅ (base) |
| Controllers de fornecedor / produto / entrada | ❌ |
| Listagens do backlog MVP | ⚠️ (venda parcial; resto aberto) |
| Capturas desktop × mobile para o documento | ❌ |

### 6B — Mobile

**Quem:** Roberto · **Status do bloco:** ⚠️ · Guia: [GUIA_MOBILE_ROBERTO.md](./GUIA_MOBILE_ROBERTO.md) · App: `mobile/`

> **Código do A7 fechado** (manual Etapa 9 em nível de implementação). Pendência = evidência ABNT (prints) + ensaio com API/banco com estoque.

| Item | Status |
|------|--------|
| App Flutter em `mobile/` (`lib/` + testes; plataformas via `configurar.ps1`) | ✅ |
| Consumir a API (login, alertas, produtos) | ✅ |
| A7: lotes a vencer + estoque baixo | ✅ |
| Loading / lista vazia / erro de rede | ✅ |
| Sem acesso a banco / FEFO no Dart | ✅ |
| Capturas para o documento | ⚠️ (Roberto em andamento) |

Cruzamento **manual × guia × repo (código)** — 21/11/2026:

| Exigência | Manual / guia | Repo |
|-----------|---------------|------|
| App Flutter consumindo Web API | Etapa 9 | ✅ `mobile/lib` |
| Telas / adaptação a dispositivos | Etapa 9 | ✅ login, home, 2 alertas; layout ≥600 px |
| ≥ 1 função complementar à web | Etapa 9 / A7 | ✅ lotes a vencer + estoque baixo |
| Rotas do contrato | GUIA §3 | ✅ `POST /auth/login`, `GET /alertas/...`, `GET /produtos` |
| Sem SQL no app | GUIA §2 | ✅ |
| Tratamento vazio / rede | GUIA | ✅ |

### 6C — UX

**Quem:** Daniel · **Status do bloco:** ⚠️ · Guia: [GUIA_UX_DANIEL.md](./GUIA_UX_DANIEL.md)

| Item | Status |
|------|--------|
| Personas e fluxos | ❌ |
| Wireframes / protótipos atualizados | ⚠️ (base III em `uiux/`) |
| Notas de usabilidade | ❌ |

**Critério de saída do bloco 6:** demo web + mobile na mesma API/banco (caminho feliz).

---

## Bloco 7 — Integração e documentação

**Quem:** Docs + grupo · **Status do bloco:** ⚠️

| Item | Status |
|------|--------|
| Fluxo ponta a ponta (entrada → venda → alerta no app) | ⚠️ (peças prontas; ensaio E2E + prints) |
| Checklist RF (feito / parcial / não feito) | ❌ |
| Limitações e evolução futura | ⚠️ (rascunho Etapa 10 encaminhado) |
| Seção Machine Learning / análise (Etapa 7) | ⚠️ (rascunho encaminhado ao doc) |
| Seção Comunicação / LIBRAS (Etapa 8) | ⚠️ (rascunho encaminhado ao doc) |
| Documento ABNT (montagem 20–30 págs) | ❌ |
| Postagem UNIP (líder = Anderson; ≤ 6 integrantes) | ❌ |

---

## Como atualizar

1. Troque ❌ → ⚠️ → ✅ na coluna Status.  
2. Atualize também a **visão geral** no topo.  
3. Não pule: API sem schema + Repo = retrabalho.  
4. Schema/seed mudou → avisar quem sobe Api/Web/Mobile.

---

*Próximo foco: Vinicius (resto Web + prints) · Roberto (capturas A7) · Geovanna (gaps Ágil §5) · Docs (montar ABNT com rascunhos 7/8/10) · Núcleo/API ✅*
