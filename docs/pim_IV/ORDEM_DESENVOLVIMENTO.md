# Ordem de desenvolvimento — PharmaStock PIM IV

Checklist vivo do time. Atualize a coluna **Status** com:

| Emoji | Significado |
|-------|-------------|
| ✅ | Feito |
| ⚠️ | Parcial / em andamento / bloqueado por dependência |
| ❌ | Não feito |

**Atualizado em:** 03/11/2026

**Complementares:** [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md) · [RELATORIO_DECISOES_KICKOFF.md](./RELATORIO_DECISOES_KICKOFF.md) · [`database/README.md`](../../database/README.md)

---

## Ordem ideal de desenvolvimento (detalhamento)

O PharmaStock do PIM IV não deve começar pela API nem pelas telas. A ordem existe para evitar retrabalho: cada camada só faz sentido quando a anterior já está estável o bastante para o próximo papel consumir.

O ponto de partida é o **Ágil** (Geovanna). Sem negócio fictício claro, backlog priorizado e critérios de aceite, o time implementa coisas diferentes com o mesmo nome. Essa etapa não precisa estar 100% “bonita” no documento ABNT para o resto avançar, mas precisa existir um MVP acordado: login, fornecedor/produto, entrada de lote, venda com baixa de estoque, listagens na web e uma funcionalidade complementar no Flutter.

Em paralelo ao fechamento do backlog — e como próximo bloqueio técnico real — vem o **banco SQL Server** (Geovanni). As procedures e triggers de entrada, venda e movimentação **já estão no repositório**; o que ainda falta é o **schema** (`CREATE TABLE`) e o **seed**. Sem as tabelas físicas, as SPs não podem ser aplicadas de ponta a ponta e o Repositório C# não tem onde gravat. O ideal é: schema → procedures → triggers → seed, testados no SQL Server local. O PostgreSQL do PIM III fica só em `legacy/`, como referência histórica.

Enquanto o schema nasce, o **núcleo C#** (Anderson) alinha os **Modelos** às tabelas e aos contratos das SPs — inclusive `Entrada` e `EntradaItem`, que ainda não estão no domínio. Modelos não carregam regra de negócio; só representam os dados. Em seguida entram **Repositório** (chamar as stored procedures, connection string no `appsettings`, acesso simples com EF Core e/ou Dapper) e **Negócio** (validações e FIFO: o Negócio escolhe o lote por validade e só então pede ao Repositório para registrar a venda). Só depois disso a **API** expõe REST + Swagger. Web e Api nunca acessam banco nem Repositório diretamente: tudo passa por Negócio.

Com a API mínima estável (pelo menos um fluxo completo no Swagger/Postman), **Web** (Vinicius) e **Mobile** (Roberto) avançam em paralelo. O Vinicius monta o MVC Razor responsivo; o Roberto o Flutter consumindo os mesmos endpoints, com ao menos uma funcionalidade complementar ao sistema web. A **UX** pode ter começado cedo (wireframes e protótipos), mas a validação forte acontece quando as telas reais existem.

Por fim, o time **integra** o caminho feliz ponta a ponta (cadastro → entrada → venda → consulta no app), registra limitações e fecha o **documento ABNT**. Machine Learning e LIBRAS, se cobrados, entram como seções curtas no texto — não como projetos separados que atrapalham essa sequência.

Em resumo: **Ágil define o quê → Banco e Modelos firmam os dados → Repositório e Negócio firmam as regras → API publica o contrato → Web e Mobile consomem → integração e documentação fecham a entrega.** Pular etapas (por exemplo, API ou telas sem schema) gera retrabalho e dificulta explicar o sistema na apresentação.

---

## Visão geral dos blocos

| # | Bloco | Quem | Status | Nota |
|---|--------|------|--------|------|
| 0 | Fundação do repo | Anderson / grupo | ✅ | Solution + docs + Modelos iniciais |
| 1 | Ágil / backlog | Geovanna | ❌ | RF, backlog, sprints |
| 2A | Banco — schema + seed | Geovanni | ❌ | `schema.sql` ainda só placeholder |
| 2B | Banco — SPs / triggers | Geovanni | ✅ | Entrada, venda, movimentação, lote |
| 3 | Modelos C# | Anderson | ⚠️ | Falta Entrada / EntradaItem + alinhar ao schema |
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
| Docs de divisão e decisões de kickoff | ✅ |
| SGBD = **SQL Server** (não PostgreSQL) | ✅ |
| Domínio mínimo inicial em `PharmaStock.Modelos` | ✅ |
| Testes xUnit básicos (Produto / ItemVenda) | ✅ |

---

## Bloco 1 — Ágil e escopo

**Quem:** Geovanna · **Status do bloco:** ❌

| Item | Status |
|------|--------|
| Texto curto do negócio fictício (PharmaStock / farmácia) | ❌ |
| Stakeholders / atores | ❌ |
| RF e RNF listados | ❌ |
| Backlog priorizado do MVP | ❌ |
| Critérios de aceite dos itens de alta prioridade | ❌ |
| Board (Trello) + 2–3 sprints propostas | ❌ |

**Critério de saída:** o time sabe o que entra na próxima sprint sem ambiguidade.

---

## Bloco 2 — Banco SQL Server

### 2A — Schema e seed *(bloqueio atual)*

**Quem:** Geovanni · **Status do bloco:** ❌

> `database/schema.sql` **não tem** `CREATE TABLE` — só comentários. As SPs já assumem as tabelas.

| Item | Status |
|------|--------|
| `schema.sql` com tabelas, PK, FK, CHECKs | ❌ |
| Tabelas mínimas das SPs (`entrada`, `entradaitem`, `venda`, `vendaitem`, `movimentacao`, `lote` + cadastros) | ❌ |
| `seed.sql` com dados de demo | ❌ |
| Ordem aplicável: schema → procedures → triggers → seed | ❌ |
| DER / modelo lógico para o ABNT | ❌ |

**Critério de saída:** banco local sobe; SPs rodam sem erro de objeto inválido.

### 2B — Procedures e triggers

**Quem:** Geovanni · **Status do bloco:** ✅ *(refinos ⚠️)*

| Item | Status |
|------|--------|
| `prc_RegistrarEntrada` / `prc_RegistrarEntradaItem` | ✅ |
| `prc_RegistrarVenda` / `prc_RegistrarVendaItem` | ✅ |
| `prc_MovimentaItens` | ✅ |
| `trg_MovimentaItens` (atualiza `lote`) | ✅ |
| `trg_RecalculaEntrada` / `trg_RecalculaVenda` | ✅ |
| Homogeneizar scripts (CREATE vs CREATE OR ALTER) | ⚠️ |
| Testar SPs+triggers em cima do schema real | ⚠️ |

---

## Bloco 3 — Modelos C#

**Quem:** Anderson · **Status do bloco:** ⚠️

| Item | Status |
|------|--------|
| Entidades iniciais (Fornecedor, Funcionario, Categoria, Produto, ProdutoPreco, Lote, Movimentacao, UsuarioLog, Venda, ItemVenda) | ✅ |
| `Entrada` e `EntradaItem` alinhados às SPs | ❌ |
| Revisar Venda / itens / Movimentacao vs colunas do schema | ❌ |
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
| Wrappers das procedures de entrada/venda | ❌ |
| CRUD básico dos cadastros (conforme backlog) | ❌ |
| `HasTrigger` se usar EF Core 7+ | ❌ |

### 4B — Negócio

| Item | Status |
|------|--------|
| Validações (fornecedor, código de barras, etc.) | ❌ |
| FIFO (escolher lote por validade) | ❌ |
| Orquestrar: Negócio escolhe lote → Repo chama SP | ❌ |
| Testes xUnit das regras críticas | ❌ |

**Critério de saída:** registrar entrada/venda sem UI, só via Negócio.

---

## Bloco 5 — API

**Quem:** Anderson · **Status do bloco:** ❌

| Item | Status |
|------|--------|
| Controllers REST + DTOs | ❌ |
| Endpoints do MVP | ❌ |
| Swagger/OpenAPI | ❌ |
| Lista de rotas para Roberto e Vinicius | ❌ |
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
| ≥ 1 funcionalidade complementar | ❌ |
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
| Postagem UNIP (líder; ≤ 6 integrantes) | ❌ |

---

## Como atualizar

1. Troque ❌ → ⚠️ → ✅ na coluna Status.  
2. Atualize também a **visão geral** no topo.  
3. Não pule: API sem schema + Repo = retrabalho.  
4. Schema pronto → avisar Anderson (blocos 3 e 4).

---

*Próximo foco: **2A schema.sql** ❌ + **1 backlog** ❌ + Anderson **Entrada/EntradaItem** ❌*
