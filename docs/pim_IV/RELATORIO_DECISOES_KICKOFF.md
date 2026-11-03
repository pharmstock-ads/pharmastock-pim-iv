# Relatório de decisões — Kickoff PharmaStock (PIM IV)

**Para:** Geovanna (Engenharia de Software Ágil)  
**De:** Anderson (Núcleo C# + API)  
**Criado:** 14/10/2026  
**Atualizado:** 03/11/2026 (após entrega de procedures/triggers no SQL Server)  
**Objetivo:** registrar de onde o projeto parte, decisões fechadas e o que ainda falta — base para backlog e sprints.

**Documento complementar:** [DIVISAO_EQUIPE_E_ROTEIRO.md](./DIVISAO_EQUIPE_E_ROTEIRO.md)  
**Banco (detalhe técnico):** [`../../database/README.md`](../../database/README.md)

---

## 1. Contexto

O PIM IV é a **continuidade** do sistema PharmaStock iniciado no PIM III ([repositório do III](https://github.com/pimIII/pimIII); trabalho atual em [pharmastock-pim-iv](https://github.com/pharmstock-ads/pharmastock-pim-iv)).

No III entregamos:

- Domínio de farmácia (produto, fornecedor, lote, venda, funcionário)
- Apps Console + WinForms (persistência **em memória**)
- Scripts PostgreSQL de **referência** (não conectados à aplicação) — hoje só em `legacy/`
- Protótipo HTML e artefatos de UX

No IV o manual exige, em resumo:

- Solution em camadas: **Modelos → Repositório → Negócio → Web (MVC) + Api**
- Banco **SQL Server** com **trigger + stored procedure** (procedure chamada pelo sistema)
- App **Flutter** consumindo a API
- Documento ABNT cobrindo as etapas do manual

Código legado do III está em `legacy/`. A solution nova está em `src/PharmaStock.sln`.

---

## 2. Decisões já tomadas

| # | Tema | Decisão | Status |
|---|------|--------|--------|
| D1 | Negócio fictício | **Continuar PharmaStock / farmácia** | **Confirmado** |
| D2 | Escopo MVP funcional | Fluxos prioritários (ver §3) | **Confirmado** |
| D3 | Papel Anderson | **Núcleo C#** (Modelos, Negócio, Repositório) **+ API** | **Confirmado** |
| D4 | Papel Geovanna | **Engenharia de Software Ágil** | **Confirmado** |
| D5 | Banco (SGBD) | **SQL Server** (conforme manual) — **não** PostgreSQL | **Confirmado** (03/11/2026) |
| D6 | Procedures / triggers | Entrada, venda e movimentação de estoque em T-SQL | **Parcialmente entregue** (ver §4) |
| D7 | Papel banco (código) | **Geovanni** — procedures/triggers no repo | **Confirmado** |
| D8 | Papel Web | **Vinicius** — MVC + responsivo | **Confirmado** |
| D9 | Papel Mobile | **Roberto** — Flutter | **Confirmado** |

---

## 3. MVP funcional sugerido

1. **Autenticação / funcionário** (login)  
2. **Cadastro de fornecedor e produto**  
3. **Entrada de lote** (estoque com validade) — apoiada por `prc_RegistrarEntrada*`  
4. **Venda com baixa de estoque** (FIFO no Negócio C#; movimentação via `prc_RegistrarVenda*` + trigger no lote)  
5. **Listagens/consultas essenciais na Web**  
6. **Uma funcionalidade complementar no app Flutter**

---

## 4. Banco de dados — situação atual (SQL Server)

### Decisão fechada

O grupo segue com **SQL Server**. O material PostgreSQL do III fica apenas como legado/referência.

### O que já entrou no repo (PR / branch `feature/procedures-triggers`)

Pasta `database/procedure/`:

| Procedure | Papel |
|-----------|--------|
| `prc_RegistrarEntrada` | Cria cabeçalho `entrada` |
| `prc_RegistrarEntradaItem` | Item + movimentação tipo `'e'` |
| `prc_RegistrarVenda` | Cria cabeçalho `venda` |
| `prc_RegistrarVendaItem` | Item + movimentação tipo `'s'` |
| `prc_MovimentaItens` | Insere em `movimentacao` |

Pasta `database/trigger/`:

| Trigger | Papel |
|---------|--------|
| `trg_MovimentaItens` | Atualiza `lote.quantidade` após insert/delete em `movimentacao` |
| `trg_RecalculaEntrada` | Recalcula `entrada.valor_total` |
| `trg_RecalculaVenda` | Recalcula `venda.valor_total` |

### Modelo físico implícito (ainda falta `schema.sql`)

Os scripts assumem tabelas próprias de documento:

- `entrada` / `entradaitem`
- `venda` / `vendaitem`
- `movimentacao` (liga item de entrada ou de venda)
- `lote` (quantidade mantida pela trigger)

Isso **resolve a dúvida antiga** “venda vs só movimentação”: no IV há **Venda** (e **Entrada**) como documentos, e `movimentacao` como efeito no estoque.

### Ainda pendente no banco

- [ ] `database/schema.sql` completo (CREATE TABLE, PK/FK, CHECKs)
- [ ] `database/seed.sql` com dados de demo
- [ ] Homogeneizar nomes (`CREATE` vs `CREATE OR ALTER`, casing de procedures)
- [ ] Documentar no ABNT (DER + físico + justificativa)

### Impacto no núcleo C# (Anderson)

- Incluir no domínio: `Entrada`, `EntradaItem` (além de `Venda` / `ItemVenda` já esboçados)
- Repositório deve chamar as procedures acima (Dapper `CommandType.StoredProcedure` ou EF `ExecuteSql`)
- Entidades com trigger: mapear `HasTrigger` se usar EF Core 7+

---

## 5. Divisão de papéis

| Papel | Responsável | Entrega |
|-------|-------------|---------|
| Núcleo C# + API | Anderson | Camadas + endpoints REST |
| Banco | **Geovanni** | Schema, procedures, triggers, seed |
| Web (MVC + responsivo) | **Vinicius** | Telas Razor |
| Mobile (Flutter) | **Roberto** | App consumindo a API |
| Eng. Software Ágil | **Geovanna** | RF/RNF, backlog, sprints, aceite |
| Documentação ABNT | _a definir_ | Documento final |
| UX/UI | _a definir_ | Personas, fluxos, protótipos |

**ML e LIBRAS:** seções curtas no documento (Documentação), sem dono dedicado.

> Manual: até **6** integrantes na postagem UNIP — validar com coordenação.

---

## 6. Ordem de trabalho

```
[1] Negócio + Ágil
[2] Banco + Modelos (em paralelo)     ← procedures/triggers já avançaram
[3] Negócio + Repositório (chamar SPs)
[4] API
[5] Web e Mobile
[6] UX
[7] Integração + documento final
```

---

## 7. Checklist Ágil (Geovanna)

- [ ] Formalizar D1 no board + texto do negócio fictício
- [ ] Transformar o MVP do §3 em backlog com aceite
- [ ] Stakeholders, RF e RNF
- [ ] 2–3 sprints propostas
- [x] SGBD: **SQL Server** (não precisa mais validar PostgreSQL)
- [ ] Alinhar com Geovanni o `schema.sql` + seed
- [ ] Limite de 6 integrantes na plataforma
- [ ] Preencher nomes/RAs na divisão

---

## 8. Próximos passos do Anderson

1. Alinhar `PharmaStock.Modelos` com `Entrada` / `EntradaItem` + campos das SPs  
2. Repositório: wrappers das procedures de entrada/venda  
3. Negócio: FIFO (escolha de lote) antes de chamar `prc_RegistrarVendaItem`  
4. API: endpoints que orquestram esses fluxos  

---

## 9. Pendências

| ID | Pendência | Status | Quem |
|----|-----------|--------|------|
| P1 | PostgreSQL vs SQL Server | **Fechado → SQL Server** | — |
| P2 | Procedures/triggers iniciais | **Feito** (refinar + schema) | Geovanni |
| P3 | Venda vs movimentação | **Fechado** (venda/entrada + movimentacao) | Banco + Anderson |
| P4 | Nomes Docs / UX (Web=Vinicius, Mobile=Roberto já ok) | Parcial | Líder / Geovanna |
| P5 | Feature complementar Flutter | Aberto | Roberto + Anderson + Geovanna |
| P6 | Limite de 6 na UNIP | Aberto | Líder / Geovanna |
| P7 | `schema.sql` + `seed.sql` | Aberto | Geovanni |

---

## 10. Referências no repo

| Caminho | Para quê |
|---------|----------|
| `docs/manual/Manual_PIM_IV_2026.docx` | Manual oficial |
| `docs/pim_IV/DIVISAO_EQUIPE_E_ROTEIRO.md` | Divisão + checklists |
| `database/` | SQL Server (procedures/triggers atuais) |
| `legacy/database-postgresql/` | Apenas referência do III |
| `src/PharmaStock.sln` | Solution do IV |

---

*Atualizar este arquivo quando P4–P7 forem fechados.*
