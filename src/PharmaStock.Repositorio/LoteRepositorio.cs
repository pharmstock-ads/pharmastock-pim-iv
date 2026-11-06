// Consultas Dapper de lote.
using Dapper;
using PharmaStock.Modelos;
using PharmaStock.Repositorio.Abstracoes;

namespace PharmaStock.Repositorio;

public class LoteRepositorio : ILoteRepositorio
{
    private readonly ISqlConnectionFactory _factory;

    public LoteRepositorio(ISqlConnectionFactory factory) => _factory = factory;

    public async Task<IReadOnlyList<Lote>> ListarPorProdutoAsync(int idProduto, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<Lote>(new CommandDefinition(
            """
            SELECT id AS Id, id_produto AS IdProduto, numero_lote AS NumeroLote,
                   validade AS Validade, quantidade AS Quantidade
            FROM dbo.lote
            WHERE id_produto = @idProduto
            ORDER BY validade
            """,
            new { idProduto },
            cancellationToken: ct));
        return rows.ToList();
    }

    public async Task<IReadOnlyList<Lote>> ListarComEstoquePorProdutoAsync(int idProduto, CancellationToken ct = default)
    {
        await using var conn = _factory.Create();
        var rows = await conn.QueryAsync<Lote>(new CommandDefinition(
            """
            SELECT id AS Id, id_produto AS IdProduto, numero_lote AS NumeroLote,
                   validade AS Validade, quantidade AS Quantidade
            FROM dbo.lote
            WHERE id_produto = @idProduto AND quantidade > 0
            ORDER BY validade
            """,
            new { idProduto },
            cancellationToken: ct));
        return rows.ToList();
    }
}
