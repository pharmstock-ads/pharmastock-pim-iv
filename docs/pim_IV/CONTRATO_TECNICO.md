# Contrato técnico — PharmaStock PIM IV

Documento de referência compartilhada: decisões de arquitetura, fronteiras entre papéis e contratos reutilizados por Banco, Núcleo/API, Web e Mobile.

**Atualizado em:** 03/11/2026  
**Status:** rascunho vivo — preencher seções marcadas como **ABERTO**

**Complementares:** [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) · [RELATORIO_DECISOES_KICKOFF.md](./RELATORIO_DECISOES_KICKOFF.md) · [`database/README.md`](../../database/README.md)

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
| Negócio | Regras (FIFO, validações, disponibilidade); consumido por Web e Api |
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
| Núcleo C# + API | Anderson | Modelos, Negócio, Repositório, rotas/DTOs da API |
| Banco | Geovanni | Schema, seed, SPs, triggers, integridade SQL |
| Web | Vinicius | Telas MVC / ViewModels (consome Negócio) |
| Mobile | Roberto | Flutter (consome API) |
| Eng. Software Ágil | Geovanna | Backlog, RF/RNF, aceite |
| Documentação / UX | _a definir_ | — |

---

## 3. Divisão de responsabilidades Banco × Negócio (FECHADO)

| Assunto | Dono | Notas |
|---------|------|-------|
| Schema, PK/FK, CHECK, seed | **Geovanni** | Inclui impedir (ou não) estoque negativo no SQL |
| Assinatura das procedures (nome, params, OUTPUT) | **Geovanni** | Anderson só consome |
| FIFO (escolher lote por validade) | **Anderson (Negócio)** | SPs atuais **não** escolhem lote; recebem `@pIdLote` |
| Validar estoque suficiente / mensagens de erro amigáveis | **Anderson (Negócio)** → API | Antes de chamar a SP de venda |
| Integridade “burra” no banco (FK, quantidade > 0, tipo_mov) | **Geovanni** | — |

**Fluxo de venda (acordo):**

1. Cliente da API manda `produtoId` + `quantidade` (sem lote)  
2. Negócio escolhe lote FIFO  
3. Repositório chama `prc_RegistrarVenda` + `prc_RegistrarVendaItem` (com o lote escolhido)  
4. Trigger atualiza `lote.quantidade`  

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
| `prc_RegistrarEntrada` | Abre cabeçalho; `@pIdEntrada OUTPUT` |
| `prc_RegistrarEntradaItem` | Item + movimentação tipo `'e'` |
| `prc_RegistrarVenda` | Abre cabeçalho; `@pIdVenda OUTPUT` |
| `prc_RegistrarVendaItem` | Item + movimentação tipo `'s'` |
| `prc_MovimentaItens` | Insere em `movimentacao` |

Detalhes: [`database/README.md`](../../database/README.md) e arquivos em `database/procedure/`.

### 5.2 Triggers

| Trigger | Efeito |
|---------|--------|
| `trg_MovimentaItens` | Atualiza `lote.quantidade` |
| `trg_RecalculaEntrada` | Recalcula `entrada.valor_total` |
| `trg_RecalculaVenda` | Recalcula `venda.valor_total` |

### 5.3 Modelo implícito

Documentos: `entrada` / `entradaitem`, `venda` / `vendaitem`  
Estoque: `movimentacao`, `lote`

### 5.4 Checklist Geovanni (ABERTO)

| # | Entregar / confirmar | Status |
|---|----------------------|--------|
| B1 | `schema.sql` completo (CREATE TABLE, PK, FK, CHECK) | ABERTO |
| B2 | `seed.sql` (funcionário, produto, lote, etc.) | ABERTO |
| B3 | Ordem de deploy: schema → procedures → triggers → seed | ABERTO |
| B4 | Lote já existe **antes** da entrada ou é criado na entrada? | ABERTO |
| B5 | Assinatura final das SPs (casing, CREATE OR ALTER) | ABERTO |
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
| `Desconto` em `Venda` | **ABERTO** — schema/SPs atuais do cabeçalho não mostram desconto; confirmar com Geovanni ou remover do MVP |
| Quantidades `int` vs `decimal` | Seguir tipo do schema quando existir |

---

## 7. Contrato da API HTTP (PROPOSTA)

Base: `/api` · JSON · Swagger em Development · Controllers → **Negócio** apenas.  
Consumidores: **Roberto (Flutter)** e, se útil, ferramentas de teste do Vinicius.

### 7.1 Autenticação (MVP didático)

| Método | Rota | Request | Response |
|--------|------|---------|----------|
| POST | `/api/auth/login` | `{ "cpfOuEmail", "senha" }` | `{ "funcionarioId", "nome", "cargo" }` |

Cliente guarda `funcionarioId` e envia nas operações.  
**ABERTO:** evoluir para JWT depois (não obrigatório no primeiro MVP).

### 7.2 Cadastros

| Método | Rota | Uso |
|--------|------|-----|
| GET | `/api/fornecedores` | Listar |
| POST | `/api/fornecedores` | Criar |
| GET | `/api/produtos` | Listar |
| GET | `/api/produtos/{id}` | Detalhe |
| POST | `/api/produtos` | Criar |
| GET | `/api/lotes?produtoId=` | Listar lotes (validade/qtd) |
| POST | `/api/lotes` | Criar lote (**depende de B4**) |

### 7.3 Entrada

| Método | Rota | Request (DTO) |
|--------|------|----------------|
| POST | `/api/entradas` | `{ "funcionarioId", "itens": [ { "produtoId", "loteId", "quantidade", "custoUnitario" } ] }` |

Negócio orquestra `prc_RegistrarEntrada` + `prc_RegistrarEntradaItem`.

### 7.4 Venda (FIFO no Negócio)

| Método | Rota | Request (DTO) |
|--------|------|----------------|
| POST | `/api/vendas` | `{ "funcionarioId", "itens": [ { "produtoId", "quantidade" } ] }` |
| GET | `/api/vendas` | Histórico |
| GET | `/api/vendas/{id}` | Detalhe |

Cliente **não** envia `loteId` na venda; Negócio escolhe o lote.

### 7.5 Status HTTP

| Código | Quando |
|--------|--------|
| 200 / 201 | Sucesso / criado |
| 400 | Validação, estoque insuficiente (mensagem clara) |
| 401 | Login inválido |
| 404 | Não encontrado |

### 7.6 Fora de escopo do MVP (não fazer)

GraphQL, MediatR/CQRS, versionamento `/v2`, microsserviços.

---

## 8. Cadastros fora das SPs (ABERTO / PROPOSTA)

SPs cobrem entrada/venda. Cadastros (fornecedor, produto, funcionário, lote):

| Opção | Status |
|-------|--------|
| SQL parametrizado via Dapper no Repositório | **PROPOSTA** |
| Novas procedures de cadastro | ABERTO (só se Geovanni quiser) |

---

## 9. Pendências gerais (ABERTO)

| ID | Tema | Dono |
|----|------|------|
| P-Docs | Nome do responsável por Documentação ABNT | Grupo |
| P-UX | Nome do responsável por UX | Grupo |
| P-Líder | Líder da postagem UNIP | Grupo |
| P-Flutter | Qual a feature complementar do app | Roberto + Geovanna + Anderson |
| P-Auth | JWT vs funcionarioId no MVP | Anderson (proposta: id simples) |
| P-Desconto | Campo desconto na venda | Geovanni + Anderson |

---

## 10. Como usar este documento

1. Geovanni fecha a seção **5.4** → avisa Anderson.  
2. Anderson adapta Modelos/Repositório e implementa as rotas da seção **7**.  
3. Roberto e Vinicius tratam a seção **7** como contrato de integração.  
4. Qualquer mudança de rota/campo: atualizar este arquivo **antes** de codar em cima.  

---

*Rascunho inicial — completar conforme schema e decisões do time.*
