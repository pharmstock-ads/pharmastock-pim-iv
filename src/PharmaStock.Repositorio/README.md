# PharmaStock.Repositorio

**Única** camada que acessa o SQL Server (Dapper).

- Interfaces em `Abstracoes/`
- Implementações: CRUD + chamada de stored procedures
- Connection string injetada via `ISqlConnectionFactory` (chave **`Padrao`** no host)

Não roda sozinha. Consumida só pelo Negócio. Scripts do banco: [`../../database/`](../../database/).
