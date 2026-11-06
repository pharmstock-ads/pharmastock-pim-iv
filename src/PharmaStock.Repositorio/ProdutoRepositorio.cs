// Acesso Dapper a produto.
using Dapper;
using PharmaStock.Modelos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class ProdutoRepositorio : IProdutoRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public ProdutoRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<IReadOnlyList<Produto>> ListarAsync(CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<Produto>(new CommandDefinition(
            """
            SELECT id AS Id, codigo AS Codigo, codbarra AS CodBarra, nome AS Nome,
                   id_categoria AS IdCategoria, id_fornecedor AS IdFornecedor
            FROM dbo.produto
            ORDER BY nome
            """,
            cancellationToken: ct));
        return rows.ToList();
    }

    public async Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.QuerySingleOrDefaultAsync<Produto>(new CommandDefinition(
            """
            SELECT id AS Id, codigo AS Codigo, codbarra AS CodBarra, nome AS Nome,
                   id_categoria AS IdCategoria, id_fornecedor AS IdFornecedor
            FROM dbo.produto
            WHERE id = @id
            """,
            new { id },
            cancellationToken: ct));
    }

    public async Task<int> InserirAsync(Produto produto, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            """
            INSERT INTO dbo.produto (codigo, codbarra, nome, id_categoria, id_fornecedor)
            OUTPUT INSERTED.id
            VALUES (@Codigo, @CodBarra, @Nome, @IdCategoria, @IdFornecedor)
            """,
            produto,
            cancellationToken: ct));
    }
}
