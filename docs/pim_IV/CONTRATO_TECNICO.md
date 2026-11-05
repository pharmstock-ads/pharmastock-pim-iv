# Contrato técnico — PharmaStock PIM IV

Documento de referência compartilhada: decisões de arquitetura, fronteiras entre papéis e contratos reutilizados por Banco, Núcleo/API, Web e Mobile.

**Atualizado em:** 05/11/2026 (fechados C13–C16; §11 = o que ainda falta decidir)  
**Status:** rascunho vivo — ver **§11** para pendências de decisão explícitas

**Complementares:** [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) · [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md) · [`database/README.md`](../../database/README.md) · [guias por papel](./README.md)

---

## Legenda

| Marcação | Significado |
|----------|-------------|
| **FECHADO** | Acordo do grupo — seguir assim |
| **PROPOSTA** | Recomendação (pode ajustar; avisar o time) |
| **ABERTO** | Falta resposta do dono indicado |

---

## 1. Arquitetura (FECHADO)

### 1.1 Camadas

```
Web ──┐
      ├──► Negocio ──► Repositorio ──► Modelos
Api ──┘                      │
                             └──► SQL Server
Flutter ──► Api
```

| Camada | Responsabilidade |
|--------|------------------|
| Modelos | Entidades; sem dependência de outras camadas |
| Repositório | **Única** que acessa o banco; CRUD + **execução de stored procedures** |
| Negócio | Regras (**FEFO**/validade, validações, disponibilidade); consumido por Web e Api |
| Web | MVC Razor; controller só chama Negócio |
| Api | REST para Flutter; DTOs + Swagger; controller só chama Negócio |

### 1.2 Regras obrigatórias (manual)

- Web e Api **não** acessam banco nem Repositório diretamente  
- Regras de negócio **não** ficam em controller/view  
- Connection string em `appsettings.json` (nunca hardcoded)  
- Consultas parametrizadas  
- ≥ 1 trigger e ≥ 1 procedure; procedure chamada pelo **Repositório**  

### 1.3 SGBD (FECHADO)

**SQL Server.** PostgreSQL do III só em `legacy/database-postgresql/` (referência).

### 1.4 Fonte da verdade dos dados (FECHADO)

**Banco (schema + SPs)** é a fonte da verdade.  
O C# (`PharmaStock.Modelos` e Repositório) **se adapta** ao que o Geovanni versionar — não o contrário.

---

## 2. Papéis (FECHADO)

| Papel | Quem | No contrato responde por |
|-------|------|--------------------------|
| Líder UNIP + Núcleo C# + API | Anderson | Coordenação; Modelos, Negócio, Repositório, rotas/DTOs |
| Banco | Geovanni | Schema, seed, SPs, triggers, integridade SQL |
| Web | Vinicius | Telas MVC / ViewModels (consome Negócio) |
| Mobile | Roberto | Flutter: login + **lotes a vencer** + **estoque baixo** (A7) |
| Eng. Software Ágil | Geovanna | Backlog, RF/RNF, aceite |
| Documentação / UX | _a definir_ | — |

---

## 3. Divisão de responsabilidades Banco × Negócio (FECHADO)

| Assunto | Dono | Notas |
|---------|------|-------|
| Schema, PK/FK, CHECK, seed | **Geovanni** | Inclui impedir (ou não) estoque negativo no SQL |
| Assinatura das procedures (nome, params, OUTPUT) | **Geovanni** | Anderson só consome |
| FEFO (escolher lote por validade mais próxima) | **Anderson (Negócio)** | SPs **não** escolhem lote; recebem `@pIdLote` |
| Validar estoque suficiente / mensagens de erro amigáveis | **Anderson (Negócio)** → API | Antes de chamar a SP de venda |
| Integridade “burra” no banco (FK, quantidade > 0, tipo_mov) | **Geovanni** | — |
| Obter/cadastrar lote na entrada | **Banco (SP)** + Negócio orquestra | `prc_ObterOuCadastrarLote` |
| Venda atravessando vários lotes (**C13**) | **Anderson (Negócio)** | **FECHADO** — gerar N `vendaitem` (FEFO em sequência) até completar a qtd |
| Transação multi-item (**C15**) | **Anderson (Repositório)** | **FECHADO** — uma transação (commit/rollback) por entrada ou venda |

**Fluxo de venda (acordo — FECHADO):**

1. Cliente manda `produtoId` + `quantidade` (+ `precoUnitario` no MVP; sem lote)  
2. Negócio ordena lotes do produto por **validade ASC** (FEFO) com qtd > 0  
3. Se a soma dos estoques < quantidade pedida → erro (não grava nada)  
4. Senão, reparte a quantidade nos lotes (ex.: precisa 50, lote A tem 30 → item 30 no A + 20 no B)  
5. Repositório, **numa única transação**: `prc_RegistrarVenda` + N× `prc_RegistrarVendaItem`  
6. Trigger atualiza `lote.quantidade` em cada movimentação  

---

## 4. Acesso a dados (PROPOSTA — alinhada ao manual)

O manual permite EF Core, Dapper ou **combinação**.

**Proposta acadêmica (simples de explicar):**

> **Dapper** no Repositório para CRUD de cadastros **e** para stored procedures (`CommandType.StoredProcedure`).

Motivos: SPs de entrada/venda já são o caminho principal de escrita; evita `HasTrigger` no começo; trecho da chamada da procedure fica claro no documento.

**Alternativa também válida (manual):** EF Core nos cadastros + Dapper nas SPs.

| Item | Status |
|------|--------|
| Tecnologia do Repositório | **PROPOSTA:** Dapper (confirmar com Anderson) |
| Migrations EF | **ABERTO** (se for 100% Dapper + scripts em `database/`, não precisa) |

---

## 5. Banco — o que já existe (FECHADO parcial)

### 5.1 Procedures

| Procedure | Função |
|-----------|--------|
| `prc_ObterOuCadastrarLote` | Localiza/cria lote; `@pIdLote OUTPUT` |
| `prc_RegistrarEntrada` | Abre cabeçalho; `@pIdEntrada OUTPUT` |
| `prc_RegistrarEntradaItem` | Item + mov. `'e'`; param **`@pPrecoUnitario`** (coluna `entradaitem.preco_unitario`) |
| `prc_RegistrarVenda` | Abre cabeçalho; `@pIdVenda OUTPUT` |
| `prc_RegistrarVendaItem` | Item + movimentação tipo `'s'` |
| `prc_MovimentaItens` | Insere em `movimentacao` |

Detalhes: [`database/README.md`](../../database/README.md) e arquivos em `database/procedure/`.

### 5.2 Triggers

| Trigger | Efeito |
|---------|--------|
| `trg_MovimentaItens` | Atualiza `lote.quantidade` |
| `trg_RecalculaEntrada` | `valor_total` = `COALESCE(SUM(preco_unitario * quantidade), 0)` |
| `trg_RecalculaVenda` | Recalcula `venda.valor_total` |

### 5.3 Modelo físico

Fonte atual: `database/tabelas/tabelas.txt` · Seed parcial: `database/inserts/inserts.txt`  
Documentos: `entrada` / `entradaitem`, `venda` / `vendaitem` · Estoque: `movimentacao`, `lote`

### 5.4 Checklist Geovanni

| # | Entregar / confirmar | Status |
|---|----------------------|--------|
| B1 | `schema.sql` completo (a partir de `tabelas.txt`) | ABERTO |
| B2 | Seed (funcionário, produto, lote, etc.) | **PARCIAL** — `inserts/inserts.txt` (lotes qtd 0; falta entrada demo) |
| B3 | Ordem de deploy: schema → procedures → triggers → seed | ABERTO |
| B4 | Lote na entrada | **FECHADO** — `prc_ObterOuCadastrarLote` (+ inserts já criam lotes zerados) |
| B5 | Assinatura final das SPs (casing, CREATE OR ALTER) | ABERTO (entrada item já usa `preco_unitario`) |
| B6 | Banco impede estoque negativo? (CHECK/trigger) ou só Negócio? | ABERTO |
| B7 | Connection string Dev (LocalDB/Express) — nome do banco | ABERTO |
| B8 | Homogeneizar scripts e testar SPs no schema real | ABERTO |

---

## 6. Modelos C# (orientação)

| Situação | Ação |
|----------|------|
| Classes atuais (Produto, Lote, Venda, …) | Base; **ajustar** quando schema fechar |
| `Entrada` / `EntradaItem` | **Criar** (ainda não existem) |
| `ItemVenda` sem `IdLote` | Incluir lote no fluxo interno do Negócio/Repo (API pública pode não expor lote) |
| `Desconto` em `Venda` | **Adiado (A5/A6)** — fora do MVP; schema atual sem desconto |
| `EntradaItem.PrecoUnitario` (não Custo) | **FECHADO** — alinhar Modelos/DTOs ao schema |
| Quantidades `int` vs `decimal` | Seguir `tabelas.txt` (lote/`entradaitem` decimal; `vendaitem`/mov. int) |

---

## 7. Contrato da API HTTP (PROPOSTA)

Base: `/api` · JSON · Swagger em Development · Controllers → **Negócio** apenas.  
Consumidores: **Roberto (Flutter)** e, se útil, ferramentas de teste do Vinicius.

### 7.1 Autenticação (MVP didático)

| Método | Rota | Request | Response |
|--------|------|---------|----------|
| POST | `/api/auth/login` | `{ "cpfOuEmail", "senha" }` | `{ "funcionarioId", "nome", "cargo" }` |

Cliente guarda `funcionarioId` e envia nas operações.  
**FECHADO no MVP:** sem JWT (A2 adiado). JWT só depois, se sobrar tempo.

**Login / senha (C16 — FECHADO):** comparar a senha enviada com `funcionario.senha_hash` do banco (valores demo do `inserts.txt`, ex. `hash_demo_ana`). Sem BCrypt/Identity obrigatório no primeiro MVP. Se Geovanni ampliar `senha_hash` depois, o Negócio pode evoluir o algoritmo sem mudar a rota.

### 7.2 Cadastros

| Método | Rota | Uso |
|--------|------|-----|
| GET | `/api/fornecedores` | Listar |
| POST | `/api/fornecedores` | Criar |
| GET | `/api/produtos` | Listar |
| GET | `/api/produtos/{id}` | Detalhe |
| POST | `/api/produtos` | Criar |
| GET | `/api/lotes?produtoId=` | Listar lotes (validade/qtd) |
| POST | `/api/lotes` | Opcional — preferir criar lote via entrada + `prc_ObterOuCadastrarLote` (B4 **FECHADO**) |

### 7.3 Entrada

| Método | Rota | Request (DTO) |
|--------|------|----------------|
| POST | `/api/entradas` | `{ "funcionarioId", "itens": [ { "produtoId", "numeroLote", "validade", "quantidade", "precoUnitario" } ] }` |

Negócio: `prc_ObterOuCadastrarLote` → `prc_RegistrarEntrada` → `prc_RegistrarEntradaItem` (`@pPrecoUnitario`).

### 7.4 Venda (FEFO no Negócio)

| Método | Rota | Request (DTO) |
|--------|------|----------------|
| POST | `/api/vendas` | `{ "funcionarioId", "itens": [ { "produtoId", "quantidade", "precoUnitario" } ] }` |
| GET | `/api/vendas` | Histórico |
| GET | `/api/vendas/{id}` | Detalhe |

Cliente **não** envia `loteId`; Negócio escolhe lote(s) FEFO e pode gerar **vários** itens internos (**C13**).

### 7.5 Alertas mobile (A7 — FECHADO; defaults **C14**)

| Método | Rota | Default | Uso |
|--------|------|---------|-----|
| GET | `/api/alertas/lotes-a-vencer?dias=` | **`dias=30`** se omitido | Lotes com `validade ≤ hoje + dias` |
| GET | `/api/alertas/estoque-baixo?limite=` | **`limite=10`** se omitido | Lotes com `quantidade ≤ limite` |

### 7.6 Status HTTP

| Código | Quando |
|--------|--------|
| 200 / 201 | Sucesso / criado |
| 400 | Validação, estoque insuficiente (mensagem clara) |
| 401 | Login inválido |
| 404 | Não encontrado |

### 7.7 Fora de escopo do MVP (não fazer)

GraphQL, MediatR/CQRS, versionamento `/v2`, microsserviços.

---

## 8. Cadastros fora das SPs (ABERTO / PROPOSTA)

SPs cobrem entrada/venda. Cadastros (fornecedor, produto, funcionário, lote):

| Opção | Status |
|-------|--------|
| SQL parametrizado via Dapper no Repositório | **PROPOSTA** |
| Novas procedures de cadastro | ABERTO (só se Geovanni quiser) |

---

## 9. Pendências gerais (não técnicas / adiadas)

| ID | Tema | Status | Dono |
|----|------|--------|------|
| P-Docs | Nome do responsável por Documentação ABNT | **ABERTO** | Grupo |
| P-UX | Nome do responsável por UX | **ABERTO** | Grupo |
| P-Líder | Líder da postagem UNIP | **FECHADO → Anderson** | — |
| P-Flutter | Feature complementar do app | **FECHADO → lotes a vencer + estoque baixo** | Roberto + Anderson |
| P-Auth | JWT vs funcionarioId | **FECHADO no MVP → funcionarioId**; JWT depois (A2) | Anderson |
| P-Desconto | Campo desconto na venda | **Adiado (A5/A6)** | Depois |
| C13 | Multilote na venda (FEFO) | **FECHADO** — N `vendaitem` | Anderson |
| C14 | Defaults alertas A7 | **FECHADO** — `dias=30`, `limite=10` | Anderson + Roberto |
| C15 | Transação multi-item | **FECHADO** — 1 transação no Repositório | Anderson |
| C16 | Senha no MVP | **FECHADO** — compara com `senha_hash` demo; sem BCrypt obrigatório | Anderson |

---

## 10. Como usar este documento

1. Fechar itens da **§11** (Anderson e/ou Geovanni) antes de codar em cima de dúvida.  
2. Geovanni entrega/executa o que for **execução** na §5.4 (B1–B3, B5, B7–B8) e avisa Anderson.  
3. Anderson adapta Modelos/Repositório e implementa as rotas da seção **7**.  
4. Roberto e Vinicius tratam a seção **7** como contrato de integração.  
5. Qualquer mudança de rota/campo: atualizar este arquivo **antes** de codar em cima.

---

## 11. O que ainda falta decidir (explícito)

Lista viva do que **ainda é decisão** (não confundir com tarefa de implementação já acordada).  
Quando fechar: mudar status aqui + refletir na seção correspondente.

### 11.1 Anderson pode fechar sozinho (recomendado — ainda PROPOSTA/ABERTO)

| ID | Tema | Onde no doc | Recomendação | Status |
|----|------|-------------|--------------|--------|
| **C1** | Tecnologia do Repositório | §4 | **Dapper** 100% (CRUD + SPs) | **PROPOSTA** — confirmar |
| **C2** | Migrations EF | §4 | **Não usar** — schema só em `database/` | **ABERTO** (segue C1) |
| **C3** | Cadastros fora das SPs | §8 | **SQL parametrizado no Repositório**; SPs de cadastro só se Geovanni quiser | **PROPOSTA** — confirmar |
| **C4** | `POST /api/lotes` no MVP | §7.2 | **Não implementar** — lote via entrada + `prc_ObterOuCadastrarLote` | **PROPOSTA** — confirmar |

### 11.2 Alinhar com Geovanni (banco / ambiente)

| ID | Tema | Onde | Recomendação | Status |
|----|------|------|--------------|--------|
| **B1** | Unificar `schema.sql` a partir de `tabelas.txt` | §5.4 | Fazer (entrega, não debate) | **ABERTO** |
| **B2** | Seed completo + estoque demo (entrada) | §5.4 | Manter `inserts.txt` + script de entrada; 1 lote perto de vencer | **PARCIAL** |
| **B3** | Ordem de deploy | §5.4 | schema → procedures → triggers → inserts → entrada demo | **ABERTO** |
| **B5** | Casing / `CREATE OR ALTER` das SPs | §5.4 | Padronizar `CREATE OR ALTER` + `dbo.prc_...` | **ABERTO** |
| **B6** | Estoque negativo no SQL? | §5.4 | MVP: só **Negócio** valida; CHECK/trigger no banco = nice-to-have | **ABERTO** |
| **B7** | Connection string Dev + nome do banco | §5.4 | Ex.: LocalDB, `Database=PharmaStock`, Trusted_Connection | **ABERTO** |
| **B8** | Homogeneizar e testar SPs no schema real | §5.4 | Rodar entrada/venda no SSMS e avisar o núcleo | **ABERTO** |
| **P10** | Tamanho de `senha_hash` (varchar 25) | schema | Ampliar (ex. 128) **ou** manter demo curto alinhado ao C16 | **ABERTO** |

### 11.3 Grupo (não bloqueia núcleo/API)

| ID | Tema | Status |
|----|------|--------|
| P-Docs | Quem redige o ABNT | **ABERTO** |
| P-UX | Quem fecha UX/UI | **ABERTO** |

### 11.4 Já fechado (não reabrir)

Arquitetura em camadas · SQL Server · FEFO · A7 · auth sem JWT · `preco_unitario` · B4 ObterOuCadastrarLote · desconto adiado · **C13–C16** (multilote, defaults alerta, transação, senha demo).

---

*Vivo: C13–C16 fechados nesta revisão. Próximo: Anderson confirma C1–C4; Geovanni avança B1–B3/B5–B8.*
