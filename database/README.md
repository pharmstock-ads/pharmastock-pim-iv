# database — SQL Server (PIM IV)

Scripts do banco **SQL Server** do PharmaStock.

> PostgreSQL do PIM III permanece só em `legacy/database-postgresql/` (referência histórica).  
> **Decisão do grupo:** SGBD oficial deste semestre = **SQL Server** (alinhado ao manual).

## Estrutura

```
database/
├── tabelas/tabelas.txt        ← CREATE TABLE (base; consolidar em schema.sql)
├── inserts/inserts.txt        ← seed parcial (cadastros + lotes qtd 0)
├── schema.sql                 ← modelo físico unificado (ainda a completar)
├── seed.sql                   ← placeholder (usar inserts.txt por enquanto)
├── procedures.sql / triggers.sql  ← placeholders de índice
├── procedure/                 ← stored procedures (T-SQL)
│   ├── prc_ObterOuCadastrarLote.txt
│   ├── prc_RegistrarEntrada.txt
│   ├── prc_RegistrarEntradaItem.txt   ← @pPrecoUnitario
│   ├── prc_RegistrarVenda.txt
│   ├── prc_RegistrarVendaItem.txt
│   └── prc_MovimentaItens.txt
├── trigger/
│   ├── trg_MovimentaItens.txt
│   ├── trg_RecalculaEntrada.txt       ← SUM(preco_unitario * quantidade)
│   └── trg_RecalculaVenda.txt
└── -- Orientações para a camada Service.pas  ← regras p/ Negócio C#
```

Guia do dono: [`docs/pim_IV/GUIA_BANCO_GEOVANNI.md`](../docs/pim_IV/GUIA_BANCO_GEOVANNI.md).

## Status

| Entrega | Situação |
|---------|----------|
| CREATE TABLE em `tabelas/tabelas.txt` | Entregue (`entradaitem.preco_unitario`) |
| Procedures de entrada/venda/movimentação/lote | Entregues (Geovanni) |
| Triggers de estoque e totais | Entregues (Geovanni) |
| Inserts de demo | **Parcial** — `inserts/inserts.txt` (sem estoque ainda) |
| `schema.sql` unificado | Pendente |
| `seed.sql` formal | Pendente (espelhar inserts + entrada demo) |

## Procedures (contrato para o Repositório C#)

| Procedure | Função | Parâmetros principais |
|-----------|--------|------------------------|
| `prc_ObterOuCadastrarLote` | Localiza ou cria lote; devolve `@pIdLote` | produto, número, validade |
| `prc_RegistrarEntrada` | Abre cabeçalho de entrada | `@pIdFuncionario`, `@pIdEntrada OUTPUT` |
| `prc_RegistrarEntradaItem` | Item de entrada + mov. tipo `'e'` | entrada, produto, lote, qtd, **`@pPrecoUnitario`** |
| `prc_RegistrarVenda` | Abre cabeçalho de venda | `@pIdFuncionario`, `@pIdVenda OUTPUT` |
| `prc_RegistrarVendaItem` | Item de venda + chama movimentação tipo `'s'` | venda, produto, lote, qtd, preço |
| `prc_MovimentaItens` | Insere em `movimentacao` | funcionário, produto, lote, preço, qtd, tipo, FKs opcionais |

Fluxo típico:

1. `prc_RegistrarEntrada` / `prc_RegistrarVenda` → obtém o id  
2. Para cada item → `prc_RegistrarEntradaItem` / `prc_RegistrarVendaItem`  
3. Essas procedures chamam `prc_MovimentaItens`  
4. Triggers atualizam `lote.quantidade` e `valor_total` do cabeçalho  

## Triggers

| Trigger | Tabela | Efeito |
|---------|--------|--------|
| `trg_MovimentaItens` | `Movimentacao` (AFTER INSERT/DELETE) | Soma/subtrai quantidade no `lote` (`e` = entrada, `s` = saída) |
| `trg_RecalculaEntrada` | `EntradaItem` (AFTER INSERT/UPDATE/DELETE) | Recalcula `entrada.valor_total` com `preco_unitario` (+ `COALESCE` 0) |
| `trg_RecalculaVenda` | `VendaItem` (AFTER INSERT/UPDATE/DELETE) | Recalcula `venda.valor_total` |

## Tabelas inferidas pelos scripts (schema ainda não versionado)

Além do domínio do III, os scripts assumem:

- `entrada` / `entradaitem`
- `venda` / `vendaitem`
- `movimentacao` (com `id_entrada_item`, `id_venda_item`)
- `lote` (campo `quantidade` atualizado pela trigger)

## Regras do manual

- Connection string em `appsettings.json` (Web e Api), nunca no código-fonte
- Chave oficial: **`Padrao`**. O valor de `Server=` muda por máquina (ex. `localhost\SQLEXPRESS`, `(localdb)\mssqllocaldb`)
- Procedure executada pela camada `PharmaStock.Repositorio` (Dapper; sem EF/`HasTrigger`)
- Consultas parametrizadas (evitar SQL Injection)

### Connection string (Dev)

```json
"ConnectionStrings": {
  "Padrao": "Server=localhost\\SQLEXPRESS;Database=PharmaStock;Trusted_Connection=True;TrustServerCertificate=True"
}
```

Troque só o `Server=` se usar LocalDB ou outro host. Banco: criar `PharmaStock` e rodar `tabelas` → `procedure` → `trigger` → `inserts` (nessa ordem).
