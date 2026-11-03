# database — SQL Server (PIM IV)

Scripts do banco **SQL Server** do PharmaStock.

> PostgreSQL do PIM III permanece só em `legacy/database-postgresql/` (referência histórica).  
> **Decisão do grupo:** SGBD oficial deste semestre = **SQL Server** (alinhado ao manual).

## Estrutura

```
database/
├── schema.sql                 ← modelo físico (ainda a completar)
├── seed.sql                   ← dados de exemplo (ainda a completar)
├── procedures.sql / triggers.sql  ← placeholders de índice
├── procedure/                 ← stored procedures (T-SQL)
│   ├── prc_RegistrarEntrada.txt
│   ├── prc_RegistrarEntradaItem.txt
│   ├── prc_RegistrarVenda.txt
│   ├── prc_RegistrarVendaItem.txt
│   └── prc_MovimentaItens.txt
└── trigger/
    ├── trg_MovimentaItens.txt
    ├── trg_RecalculaEntrada.txt
    └── trg_RecalculaVenda.txt
```

## Status

| Entrega | Situação |
|---------|----------|
| Procedures de entrada/venda/movimentação | Entregues (Geovanni) |
| Triggers de estoque e totais | Entregues (Geovanni) |
| `schema.sql` (CREATE TABLE) | Pendente |
| `seed.sql` | Pendente |

## Procedures (contrato para o Repositório C#)

| Procedure | Função | Parâmetros principais |
|-----------|--------|------------------------|
| `prc_RegistrarEntrada` | Abre cabeçalho de entrada | `@pIdFuncionario`, `@pIdEntrada OUTPUT` |
| `prc_RegistrarEntradaItem` | Item de entrada + chama movimentação tipo `'e'` | entrada, produto, lote, qtd, custo |
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
| `trg_RecalculaEntrada` | `EntradaItem` (AFTER INSERT/UPDATE/DELETE) | Recalcula `entrada.valor_total` |
| `trg_RecalculaVenda` | `VendaItem` (AFTER INSERT/UPDATE/DELETE) | Recalcula `venda.valor_total` |

## Tabelas inferidas pelos scripts (schema ainda não versionado)

Além do domínio do III, os scripts assumem:

- `entrada` / `entradaitem`
- `venda` / `vendaitem`
- `movimentacao` (com `id_entrada_item`, `id_venda_item`)
- `lote` (campo `quantidade` atualizado pela trigger)

## Regras do manual

- Connection string em `appsettings.json` (Web e Api), nunca no código-fonte
- Procedure executada pela camada `PharmaStock.Repositorio`
- Consultas parametrizadas (evitar SQL Injection)
- Com EF Core 7+: mapear `HasTrigger` nas entidades que têm trigger
