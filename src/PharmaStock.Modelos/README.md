# PharmaStock.Modelos

Entidades do domínio (sem dependências de outras camadas).

| Classe | Origem |
|--------|--------|
| `Fornecedor` | Legado + SQL III |
| `Funcionario` | Legado + SQL III (`email` do SQL) |
| `Categoria` | SQL III |
| `Produto` | Legado + SQL III (`Codigo`, preços em `ProdutoPreco`) |
| `ProdutoPreco` | SQL III (`produtos_precos`) |
| `Lote` | Legado + SQL III |
| `Movimentacao` / `TipoMovimentacao` | SQL III |
| `UsuarioLog` | SQL III |
| `Venda` / `ItemVenda` | Legado (fluxo de venda; mapeamento físico com o banco) |
