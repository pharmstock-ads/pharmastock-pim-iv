# database — SQL Server (PIM IV)

Scripts do banco **SQL Server** usados pelo PharmaStock neste semestre.

> Os scripts PostgreSQL do PIM III ficaram em `legacy/database-postgresql/` só como referência de domínio.

## Arquivos planejados

| Arquivo | Conteúdo |
|---|---|
| `schema.sql` | Criação das tabelas (modelo físico) |
| `triggers.sql` | Ao menos 1 trigger (obrigatório no manual) |
| `procedures.sql` | Ao menos 1 stored procedure (obrigatória; chamada pelo Repositório) |
| `seed.sql` | Dados de exemplo |

## Regras do manual

- Connection string em `appsettings.json` (Web e Api), nunca no código-fonte
- Procedure executada pela camada `PharmaStock.Repositorio`
- Consultas parametrizadas (evitar SQL Injection)
