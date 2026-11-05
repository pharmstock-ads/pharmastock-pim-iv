# Ordem de desenvolvimento — PharmaStock PIM IV

Checklist vivo do time. Atualize a coluna **Status** com:

| Emoji | Significado |
|-------|-------------|
| ✅ | Feito |
| ⚠️ | Parcial / em andamento / bloqueado por dependência |
| ❌ | Não feito |

**Atualizado em:** 05/11/2026

**Complementares:** [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md) · [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) · [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [`database/README.md`](../../database/README.md) · [guias](./README.md)

---

## Ordem ideal de desenvolvimento (detalhamento)

O PharmaStock do PIM IV não deve começar pela API nem pelas telas. A ordem existe para evitar retrabalho: cada camada só faz sentido quando a anterior já está estável o bastante para o próximo papel consumir.

O ponto de partida é o **Ágil** (Geovanna). Baseline de RF/RNF e gaps do manual estão em [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) (§4–§5). Ainda faltam texto formal do negócio, stakeholders completos, board e 2–3 sprints. O MVP acordado: login, fornecedor/produto, entrada de lote, venda com baixa de estoque (FEFO), listagens na web e Flutter com **lotes a vencer + estoque baixo** (A7).

Em paralelo — bloqueio técnico principal — o **banco SQL Server** (Geovanni). Já existem `tabelas/tabelas.txt`, `inserts/inserts.txt`, SPs (incl. `prc_ObterOuCadastrarLote`, entrada com `@pPrecoUnitario`) e triggers. Falta unificar em `schema.sql` / `seed.sql`, ordem de deploy testada e **entrada demo** (lotes do insert estão com qtd 0). Ideal: schema → procedures → triggers → seed. PostgreSQL do III só em `legacy/`.

Enquanto isso, o **núcleo C#** (Anderson) alinha **Modelos** a `tabelas.txt` (campo **`PrecoUnitario`** na entrada; `Entrada`/`EntradaItem` ainda faltam). Depois **Repositório** (Dapper proposto + SPs; connection string no `appsettings`) e **Negócio** (validações + **FEFO**). Só então a **API** (contrato em [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) §7). Web e Api nunca acessam banco/Repositório.

Com API mínima estável, **Web** (Vinicius) e **Mobile** (Roberto) em paralelo. Flutter foca A7 via `/api/alertas/...`. UX pode começar cedo; validação forte com telas reais.

Por fim, o time **integra** o caminho feliz ponta a ponta (cadastro → entrada → venda → consulta no app), registra limitações e fecha o **documento ABNT**. Machine Learning e LIBRAS, se cobrados, entram como seções curtas no texto — não como projetos separados que atrapalham essa sequência.

Em resumo: **Ágil define o quê → Banco e Modelos firmam os dados → Repositório e Negócio firmam as regras → API publica o contrato → Web e Mobile consomem → integração e documentação fecham a entrega.** Pular etapas (por exemplo, API ou telas sem schema) gera retrabalho e dificulta explicar o sistema na apresentação.

---

## Visão geral dos blocos

| # | Bloco | Quem | Status | Nota |
|---|--------|------|--------|------|
| 0 | Fundação do repo | Anderson / grupo | ✅ | Solution + docs + Modelos iniciais |
| 1 | Ágil / backlog | Geovanna | ⚠️ | RF/RNF no relatório; falta §5 (negócio, board, sprints) |
| 2A | Banco — schema + seed | Geovanni | ⚠️ | `tabelas.txt` + `inserts.txt`; falta `schema.sql` + estoque demo |
| 2B | Banco — SPs / triggers | Geovanni | ✅ | Entrada (`@pPrecoUnitario`), ObterOuCadastrarLote, venda, triggers |
| 3 | Modelos C# | Anderson | ⚠️ | Falta Entrada / EntradaItem + `PrecoUnitario` |
| 4 | Repositório + Negócio | Anderson | ❌ | — |
| 5 | API | Anderson | ❌ | — |
| 6A | Web | Vinicius | ❌ | Template MVC só |
| 6B | Mobile | Roberto | ❌ | — |
| 6C | UX | _a definir_ | ⚠️ | Protótipos do III em `uiux/` |
| 7 | Integração + ABNT | Docs + grupo | ❌ | — |

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
| Board (Trello) + 2–3 sprints propostas | ❌ |
| Líder UNIP = Anderson | ✅ |

**Critério de saída:** o time sabe o que entra na próxima sprint sem ambiguidade.

---

## Bloco 2 — Banco SQL Server

### 2A — Schema e seed *(bloqueio parcial)*

**Quem:** Geovanni · **Status do bloco:** ⚠️

> Fonte das tabelas: `database/tabelas/tabelas.txt` (`entradaitem.preco_unitario`).  
> Seed parcial: `database/inserts/inserts.txt` (lotes com quantidade **0**).  
> `schema.sql` / `seed.sql` ainda placeholders — consolidar.

| Item | Status |
|------|--------|
| CREATE TABLE versionado (`tabelas.txt`) | ✅ |
| `schema.sql` unificado a partir de `tabelas.txt` | ❌ |
| Tabelas mínimas das SPs + cadastros | ✅ (em `tabelas.txt`) |
| Inserts de cadastro / lotes zerados | ✅ (`inserts.txt`) |
| Entrada demo / estoque para Flutter A7 | ❌ |
| Ordem aplicável: schema → procedures → triggers → seed | ❌ |
| DER / modelo lógico para o ABNT | ❌ |

**Critério de saída:** banco local sobe; SPs rodam sem erro de objeto inválido; há estoque demo para alertas.

### 2B — Procedures e triggers

**Quem:** Geovanni · **Status do bloco:** ✅ *(refinos ⚠️)*

| Item | Status |
|------|--------|
| `prc_ObterOuCadastrarLote` | ✅ |
| `prc_RegistrarEntrada` / `prc_RegistrarEntradaItem` (`@pPrecoUnitario`) | ✅ |
| `prc_RegistrarVenda` / `prc_RegistrarVendaItem` | ✅ |
| `prc_MovimentaItens` | ✅ |
| `trg_MovimentaItens` (atualiza `lote`) | ✅ |
| `trg_RecalculaEntrada` (`preco_unitario` + COALESCE) / `trg_RecalculaVenda` | ✅ |
| Homogeneizar scripts (CREATE vs CREATE OR ALTER) | ⚠️ |
| Testar SPs+triggers em cima do schema real | ⚠️ |
| Doc Service (orientações Negócio) em `database/` | ✅ |

---

## Bloco 3 — Modelos C#

**Quem:** Anderson · **Status do bloco:** ⚠️

| Item | Status |
|------|--------|
| Entidades iniciais (Fornecedor, Funcionario, Categoria, Produto, ProdutoPreco, Lote, Movimentacao, UsuarioLog, Venda, ItemVenda) | ✅ |
| `Entrada` e `EntradaItem` alinhados às SPs (`PrecoUnitario`) | ❌ |
| Revisar Venda / itens / Movimentacao vs `tabelas.txt` | ❌ |
| Remover/ignorar `ProdutoPreco` / desconto no MVP (A5/A6) | ⚠️ |
| Ajustar testes se entidades mudarem | ❌ |

**Critério de saída:** cada tabela essencial do schema tem classe correspondente.

---

## Bloco 4 — Repositório + Negócio

**Quem:** Anderson · **Status do bloco:** ❌

### 4A — Repositório

| Item | Status |
|------|--------|
| Connection string em `appsettings.json` | ❌ |
| Acesso a dados (EF Core e/ou Dapper) — simples | ❌ |
| Wrappers: ObterOuCadastrarLote + entrada/venda | ❌ |
| CRUD básico dos cadastros (conforme backlog) | ❌ |
| Dapper + sem Migrations (**C1/C2 FECHADO**) | ✅ (decisão; implementação ❌) |
| Cadastros SQL no Repo (**C3 FECHADO**) | ✅ (decisão; implementação ❌) |
| `HasTrigger` se usar EF Core 7+ | ❌ |

### 4B — Negócio

| Item | Status |
|------|--------|
| Validações (Service: funcionário, produto, qtd, preço) | ❌ |
| FEFO (escolher lote por validade ASC) | ❌ |
| Orquestrar: Negócio escolhe lote → Repo chama SP | ❌ |
| Testes xUnit das regras críticas | ❌ |

**Critério de saída:** registrar entrada/venda sem UI, só via Negócio.

---

## Bloco 5 — API

**Quem:** Anderson · **Status do bloco:** ❌

| Item | Status |
|------|--------|
| Controllers REST + DTOs | ❌ |
| Endpoints do MVP (contrato §7, alertas A7; **sem** POST /lotes — C4) | ❌ |
| Auth MVP sem JWT (`funcionarioId`) | ❌ |
| Swagger/OpenAPI | ❌ |
| Lista de rotas para Roberto e Vinicius | ⚠️ (proposta no CONTRATO_TECNICO) |
| DI no `Program.cs` | ❌ |

**Critério de saída:** Swagger/Postman executa ≥ 1 fluxo completo.

---

## Bloco 6 — Interfaces *(paralelo após API mínima)*

### 6A — Web

**Quem:** Vinicius · **Status do bloco:** ❌

| Item | Status |
|------|--------|
| Controllers MVC finos (só Negócio) | ❌ |
| Telas do backlog | ❌ |
| Layout responsivo | ❌ |
| Capturas para o documento | ❌ |

### 6B — Mobile

**Quem:** Roberto · **Status do bloco:** ❌

| Item | Status |
|------|--------|
| App Flutter em `mobile/` | ❌ |
| Consumir a API | ❌ |
| A7: lotes a vencer + estoque baixo | ❌ |
| Capturas para o documento | ❌ |

### 6C — UX

**Quem:** _a definir_ · **Status do bloco:** ⚠️

| Item | Status |
|------|--------|
| Personas e fluxos | ❌ |
| Wireframes / protótipos atualizados | ⚠️ |
| Notas de usabilidade | ❌ |

**Critério de saída:** demo web + mobile na mesma API/banco.

---

## Bloco 7 — Integração e documentação

**Quem:** Docs (_a definir_) + grupo · **Status do bloco:** ❌

| Item | Status |
|------|--------|
| Fluxo ponta a ponta | ❌ |
| Checklist RF (feito / parcial / não feito) | ❌ |
| Limitações e evolução futura | ❌ |
| Documento ABNT (20–30 págs) | ❌ |
| Seções curtas ML / LIBRAS (se cobrarem) | ❌ |
| Postagem UNIP (líder = Anderson; ≤ 6 integrantes) | ❌ |

---

## Como atualizar

1. Troque ❌ → ⚠️ → ✅ na coluna Status.  
2. Atualize também a **visão geral** no topo.  
3. Não pule: API sem schema + Repo = retrabalho.  
4. Schema pronto → avisar Anderson (blocos 3 e 4).

---

*Próximo foco: **2A** `schema.sql` + entrada demo ⚠️ · **1** gaps §5 do relatório ágil (board/sprints) ⚠️ · Anderson **Entrada/EntradaItem** + FEFO + SPs ❌*
