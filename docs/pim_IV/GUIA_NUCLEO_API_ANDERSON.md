# Guia — Núcleo C# + API (Anderson)

**Atualizado:** 05/11/2026 (pós-merge: `preco_unitario` + inserts)  
**Papéis:** Líder UNIP · Modelos · Negócio · Repositório · Api  
**Complementares:** [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) · [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md) · [GUIA_BANCO_GEOVANNI.md](./GUIA_BANCO_GEOVANNI.md)

---

## 1. Contexto

Solution: `src/PharmaStock.sln`

```
PharmaStock.Modelos  →  entidades
PharmaStock.Repositorio  →  único acesso ao SQL Server (CRUD + SPs)
PharmaStock.Negocio  →  regras (FIFO, validações)  ← “Service” do doc do Geovanni
PharmaStock.Api  →  REST + Swagger (Flutter)
PharmaStock.Web  →  Vinicius (só chama Negócio)
PharmaStock.Tests
```

Regra do manual: **Web e Api não acessam Repositório/DB**.

Fonte da verdade: `database/` (Geovanni). Orientação de fluxo: `database/-- Orientações para a camada Service.pas` (Service = **Negócio**).

---

## 2. Decisões que te afetam

| Tema | Decisão |
|------|---------|
| SGBD | SQL Server |
| Acesso a dados (proposta) | **Dapper** (CRUD + `CommandType.StoredProcedure`) |
| FIFO/FEFO | No **Negócio** (ordenar lotes por `validade ASC` com qtd > 0) |
| Auth MVP | Login retorna `funcionarioId`; sem JWT por enquanto (A2 adiado) |
| Feature Flutter (API) | Endpoints de **lotes a vencer** e **estoque baixo** (A7) |
| Preço na venda | No DTO da API (sem tabela de preços no MVP) |
| Desconto | Fora do MVP |

---

## 3. Passo a passo de desenvolvimento

### Fase A — Alinhar Modelos ao schema

1. Ler `database/tabelas/tabelas.txt` (e `schema.sql` quando Geovanni unificar).  
2. Ajustar `PharmaStock.Modelos` para bater com colunas reais — em item de entrada usar **`PrecoUnitario`** (não `CustoUnitario`).  
3. Incluir entidades faltantes: `Entrada`, `EntradaItem`, `Movimentacao`, `Categoria` se ainda não existirem.  
4. Seed de referência para testes: `database/inserts/inserts.txt` (logins demo; lotes começam com qtd 0).  
5. Testes unitários leves de construção de objetos (opcional mas útil na nota).

### Fase B — Repositório (SPs + consultas)

Implementar wrappers (Dapper), um método por SP:

| Método sugerido | SP |
|-----------------|----|
| `ObterOuCadastrarLoteAsync(...)` | `prc_ObterOuCadastrarLote` |
| `RegistrarEntradaAsync(...)` | `prc_RegistrarEntrada` |
| `RegistrarEntradaItemAsync(...)` | `prc_RegistrarEntradaItem` (`@pPrecoUnitario`) |
| `RegistrarVendaAsync(...)` | `prc_RegistrarVenda` |
| `RegistrarVendaItemAsync(...)` | `prc_RegistrarVendaItem` |

Mais CRUDs simples: listar/obter fornecedor, produto, lote, funcionário (login por CPF/e-mail).

Connection string só em `appsettings.json` (Api e Web).

### Fase C — Negócio (Service)

Seguir o doc do Geovanni:

1. Validar funcionário ativo, produto existente, qtd > 0, `precoUnitario` ≥ 0.  
2. **Entrada:** para cada item → `ObterOuCadastrarLote` → `RegistrarEntrada` → itens (campo **`precoUnitario`**).  
3. **Venda:**  
   - receber só `produtoId` + `quantidade` (+ preço unitário no MVP);  
   - escolher lote(s) por validade crescente;  
   - se estoque insuficiente → exceção de domínio (API vira 400);  
   - chamar `RegistrarVenda` + `RegistrarVendaItem` **com** `id_lote`.  
4. **Não** atualizar `lote.quantidade` no C# — trigger faz isso.

### Fase D — API

Base `/api`, JSON, Swagger em Development.

| Método | Rota | Notas |
|--------|------|-------|
| POST | `/api/auth/login` | `{ cpfOuEmail, senha }` → `{ funcionarioId, nome, cargo }` |
| GET/POST | `/api/fornecedores` | Cadastro |
| GET/POST | `/api/produtos` | Cadastro / listagem |
| GET | `/api/lotes?produtoId=` | Listagem |
| POST | `/api/entradas` | Orquestra Negócio |
| POST | `/api/vendas` | Sem `loteId` no body do cliente |
| GET | `/api/vendas` · `/api/vendas/{id}` | Histórico |
| GET | `/api/alertas/lotes-a-vencer?dias=` | **A7** Flutter |
| GET | `/api/alertas/estoque-baixo?limite=` | **A7** Flutter |

Controllers finos → só Negócio. DTOs separados das entidades.

### Fase E — Liberar consumidores

Quando login + 1 fluxo (entrada ou venda) + alertas estiverem no Swagger:

1. Avisar **Vinicius** (Web pode espelhar os mesmos serviços de Negócio).  
2. Avisar **Roberto** (base URL + exemplos JSON).  
3. Atualizar checkboxes em [ORDEM_DESENVOLVIMENTO.md](./ORDEM_DESENVOLVIMENTO.md).

---

## 4. Checklist pessoal

- [ ] Modelos = schema  
- [ ] Repositório chama ≥ 1 SP de verdade  
- [ ] Negócio: FIFO + validações do doc Service  
- [ ] API: login, cadastros, entrada, venda, alertas A7  
- [ ] Swagger documentado  
- [ ] Sem referência de Api → Repositorio  
- [ ] Postagem UNIP / coordenação do grupo (líder)

---

## 5. Dependências e bloqueios

| Você precisa de | De quem |
|-----------------|---------|
| `schema.sql` estável + assinaturas SP | Geovanni |
| Prioridade de histórias / aceite | Geovanna |
| Telas | Vinicius (depois do Negócio) |
| App | Roberto (depois da API) |

Se Geovanni mudar nome/parâmetro de SP: atualizar Repositório **no mesmo PR/semana**.

---

## 6. Referências rápidas

- SPs/triggers: `database/procedure/`, `database/trigger/`  
- Inventário: `database/README.md`  
- Rotas detalhadas: [CONTRATO_TECNICO.md](./CONTRATO_TECNICO.md) §7  
- Relatório Ágil: [RELATORIO_ENG_SOFTWARE_AGIL.md](./RELATORIO_ENG_SOFTWARE_AGIL.md)
